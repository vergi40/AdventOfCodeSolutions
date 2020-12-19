using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Solutions
{
    class Day17 : DayBase
    {
        public Day17() : base("17")
        {
        }

        public override int SolveA()
        {
            var startMaxY = Content.Count;
            var startMaxX = Content.First().Length;
            
            // Let's define our grid to start from [0,0,0]
            var grid = new Grid(0, startMaxX - 1, 0, startMaxY - 1, 0, 0);
            grid.Read(Content);
            
            // Cycles
            // Stays active if 2 or 3 active
            // Inactive -> active, if 3 neighbors active
            for (int i = 0; i < 6; i++)
            {
                grid.IncreaseSize();
                grid.UpdateStates();

                Console.WriteLine($"Cycle {i+1}. Active cubes: {grid.Cubes.Values.Count(c => c)}");
            }


            return grid.Cubes.Values.Count(c => c);
        }
        
        
        
        class Grid
        {
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
            public int MinZ { get; set; }
            public int MaxZ { get; set; }

            public Dictionary<Position, bool> Cubes { get; set; } = new();

            public Grid(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
            {
                MinX = minX;
                MaxX = maxX;
                MinY = minY;
                MaxY = maxY;
                MinZ = minZ;
                MaxZ = maxZ;
                
                Cubes = InitializeNew();
            }

            private Dictionary<Position, bool> InitializeNew()
            {
                var cubes = new Dictionary<Position, bool>();
                for (int i = MinX; i <= MaxX; i++)
                {
                    for (int j = MinY; j <= MaxY; j++)
                    {
                        for (int k = MinZ; k <= MaxZ; k++)
                        {
                            cubes.Add(new Position(i, j, k), false);
                        }
                    }
                }

                return cubes;
            }

            public void Read(List<string> content)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    var line = content[i];
                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '#')
                        {
                            Cubes[new Position(i, j, 0)] = true;
                        }
                    }
                }
            }

            public void IncreaseSize()
            {
                MinX -= 1;
                MaxX += 1;
                MinY -= 1;
                MaxY += 1;
                MinZ -= 1;
                MaxZ += 1;
                var newCubes = InitializeNew();

                foreach (var cube in Cubes)
                {
                    if (Cubes[cube.Key])
                    {
                        // Active
                        newCubes[cube.Key] = true;
                    }
                }

                Cubes = newCubes;
            }

            public void UpdateStates()
            {
                var newCubes = InitializeNew();
                foreach (var cube in Cubes)
                {
                    var count = CalculateSurroundingActive(cube.Key);
                    if (cube.Value && (count == 2 || count == 3))
                    {
                        // Active
                        newCubes[cube.Key] = true;
                    }
                    else if (!cube.Value && count == 3)
                    {
                        newCubes[cube.Key] = true;
                    }
                }

                Cubes = newCubes;
            }

            private int CalculateSurroundingActive(Position pos)
            {
                var count = 0;

                for (int i = pos.X - 1; i <= pos.X + 1; i++)
                {
                    for (int j = pos.Y - 1; j <= pos.Y + 1; j++)
                    {
                        for (int k = pos.Z - 1; k <= pos.Z + 1; k++)
                        {
                            // Skip self position
                            if (pos.X == i && pos.Y == j && pos.Z == k) continue;

                            if (IsActive(new Position(i, j, k))) count++;
                        }
                    }
                }

                return count;
            }

            private bool IsActive(Position pos)
            {
                // Whitelist only if inside borders
                if (pos.X < MinX || pos.X > MaxX) return false;
                if (pos.Y < MinY || pos.Y > MaxY) return false;
                if (pos.Z < MinZ || pos.Z > MaxZ) return false;
                return Cubes[pos];
            }
        }

        record Position(int X, int Y, int Z)
        {
            public override string ToString()
            {
                return $"{X} {Y} {Z}";
            }
        }
        
        
        public override int SolveB()
        {
            return 0;
        }
    }
}
