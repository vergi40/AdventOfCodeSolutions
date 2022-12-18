using _2021_AoC;

namespace _2022_AoC
{
    internal class Day18 : DayBase
    {
        public Day18() : base("18")
        {
        }

        public override long SolveA()
        {
            var cubes = new List<Cube>();
            var positions = new HashSet<(int, int, int)>();

            foreach (var line in Content)
            {
                var split = line.Split(',');
                var xyz = split.Select(i => int.Parse(i)).ToList();

                var hash = (xyz[0], xyz[1], xyz[2]);
                positions.Add(hash);
                cubes.Add(new Cube(xyz[0], xyz[1], xyz[2], hash));
            }

            var sum = 0;
            foreach (var cube in cubes)
            {
                sum += ConnectedSides(cube, positions);
            }

            return sum;
        }

        

        public override long SolveB()
        {
            var cubes = new List<Cube>();
            var positions = new HashSet<(int, int, int)>();

            foreach (var line in Content)
            {
                var split = line.Split(',');
                var xyz = split.Select(i => int.Parse(i)).ToList();

                var hash = (xyz[0], xyz[1], xyz[2]);
                positions.Add(hash);
                cubes.Add(new Cube(xyz[0], xyz[1], xyz[2], hash));
            }

            var sum = 0;
            foreach (var cube in cubes)
            {
                var connected = ConnectedSides(cube, positions);
                if (sum == 6)
                {
                    // TODO need some search algorithm to check if cubes form internal cave
                    continue;
                }
                sum += connected;
            }

            return sum;
        }

        record Cube(int X, int Y, int Z, (int,int,int) xyz) { }

        private int ConnectedSides(Cube cube, HashSet<(int, int, int)> positions)
        {
            var connected = 0;
            if (!positions.Contains((cube.X - 1, cube.Y, cube.Z)))
            {
                connected++;
            }
            if (!positions.Contains((cube.X + 1, cube.Y, cube.Z)))
            {
                connected++;
            }

            if (!positions.Contains((cube.X, cube.Y - 1, cube.Z)))
            {
                connected++;
            }
            if (!positions.Contains((cube.X, cube.Y + 1, cube.Z)))
            {
                connected++;
            }

            if (!positions.Contains((cube.X, cube.Y, cube.Z - 1)))
            {
                connected++;
            }
            if (!positions.Contains((cube.X, cube.Y, cube.Z + 1)))
            {
                connected++;
            }
            return connected;
        }
    }
}
