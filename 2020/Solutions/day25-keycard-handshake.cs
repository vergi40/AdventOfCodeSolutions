using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solutions
{
    class Day25 : DayBase
    {
        public Day25() : base("25")
        {
        }

        public override long SolveA()
        {
            long cardPublicKey = int.Parse(Content[0]);
            long doorPublicKey = int.Parse(Content[1]);

            //long exampleCardPubl = 5764801;
            //long exampleDoorPubl = 17807724;

            var cardLoopSize = GetLoopSize(cardPublicKey);
            var doorLoopSize = GetLoopSize(doorPublicKey);
            
            var c = CreateEncryption(cardLoopSize, doorPublicKey);
            var d = CreateEncryption(doorLoopSize, cardPublicKey);

            if (c == d)
            {
                // Success
                return c;
            }
            
            return 0;
        }
        
        private long CreateEncryption(long loopSize, long subjectNumber = 7)
        {
            long value = 1;
            const int divider = 20201227;
            
            for (int i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value = value % divider;
            }

            return value;
        }

        private long GetLoopSize(long publicKey, long subjectNumber = 7)
        {
            // So basically same as create encryption but instead of starting from 0, continue same loop till key is gotten
            long value = 1;
            const int divider = 20201227;

            for (int i = 1; i < 100000000; i++)
            {
                value *= subjectNumber;
                value = value % divider;
                if (value == publicKey) return i;
            }

            throw new ArgumentException($"Need more loops");
        }

        public override long SolveB()
        {
            return 0;
        }
    }
}
