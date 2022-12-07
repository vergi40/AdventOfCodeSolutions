from pathlib import Path
rootFolder = Path(__file__).parent
inputTxt = rootFolder / 'Input' / '04.txt'

f = inputTxt.open()
lines = list(map(str.strip, f.readlines()))

# 2-4,6-8
counter = 0
for i in range(len(lines)):
    line = lines[i]

    (a,b) = line.split(',')
    aRange = list(map(int, a.split('-')))
    (aMin, aMax) = (min(aRange), max(aRange))
    
    bRange = list(map(int, b.split('-')))
    (bMin, bMax) = (min(bRange), max(bRange))

    if aMin >= bMin and aMax <= bMax: counter += 1
    elif bMin >= aMin and bMax <= aMax: counter += 1

print('Result a: ' + str(counter))

counter = 0
for i in range(len(lines)):
    line = lines[i]
    # 7-7,8-42

    (a,b) = line.split(',')    
    
    # 7-7
    aRange = list(map(int, a.split('-')))
    (aMin, aMax) = (min(aRange), max(aRange))
    aList = [x for x in range(aMin, aMax+1)]
    
    bRange = list(map(int, b.split('-')))
    (bMin, bMax) = (min(bRange), max(bRange))
    bList = [x for x in range(bMin, bMax+1)]

    if aMin in bList or aMax in bList: counter += 1
    elif bMin in aList or bMax in aList: counter += 1

print('Result b: ' + str(counter))

def test():
    print('ok')