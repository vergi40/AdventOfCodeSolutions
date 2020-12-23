using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day20 : DayBase
    {
        public Day20() : base("20")
        {
        }

        public override long SolveA()
        {
            var tiles = new List<Tile>();
            var tileContent = new List<string>();
            foreach (var line in Content)
            {
                if (!line.Any())
                {
                    tiles.Add(new Tile(tileContent));
                    tileContent = new List<string>();
                }
                else
                {
                    tileContent.Add(line);
                }
            }
            if(tileContent.Any()) tiles.Add(new Tile(tileContent));

            for(int i = 0; i < tiles.Count - 1; i++)
            {
                var tile = tiles[i];
                // Link similar edges
                for (int j = 0; j < tile.UnmatchedSides.Count; j++)
                {
                    // Edge on target tile might be same or reversed, depends if original is flipped
                    var matchFound = false;
                    var side = tile.UnmatchedSides[j];
                    for (int k = i + 1; k < tiles.Count; k++)
                    {
                        var next = tiles[k];
                        if (next.UnmatchedSides.Contains(side))
                        {
                            tile.Link(side, next);
                            next.Link(side, tile);

                            // Unmatched count dropped by one
                            j--;
                            matchFound = true;
                            break;
                        }
                    }

                    if (matchFound) continue;
                    
                    var reversed = string.Concat(side.Reverse());
                    for (int k = i + 1; k < tiles.Count; k++)
                    {
                        var next = tiles[k];
                        if (next.UnmatchedSides.Contains(reversed))
                        {
                            tile.Link(side, next);
                            next.Link(reversed, tile);
                            
                            // Unmatched count dropped by one
                            j--;
                            break;
                        }
                    }
                }
            }

            var corners = tiles.Where(t => t.UnmatchedSides.Count == 2).ToList();

            long result = corners.First().IdToInt;
            foreach (var corner in corners.Skip(1))
            {
                result *= corner.IdToInt;
            }

            return result;
        }

        class Tile
        {
            public string Id { get; }
            public int IdToInt => int.Parse(Id);
            
            /// <summary>
            /// [row][column]
            /// </summary>
            public char[,] CharArray { get; set; }

            public List<string> ImageContent { get; set; } = new ();
            
            public int Size { get; }

            /// <summary>
            /// Top, left, bottom, right
            /// </summary>
            public List<string> Sides { get; set; } = new();

            public List<(string, Tile)> LinkedSides { get; set; } = new();
            
            public List<string> UnmatchedSides { get; set; }
            
            
            public Tile(List<string> content)
            {
                var title = content.First().Skip(5);
                foreach (var number in title)
                {
                    if (char.IsDigit(number))
                    {
                        Id += number;
                    }
                }
                
                
                // Catch is to record edges with same rotation direction for each tile.
                // Then the rotation factor becomes irrelevant.
                //  ->
                // ^  \/
                //  <-
                ImageContent = content.Skip(1).ToList();
                

                Size = ImageContent.Count;

                var arrayContent = new List<string>(ImageContent);
                arrayContent.Reverse();
                CharArray = new char[Size, Size];
                for (int i = 0; i < arrayContent.Count; i++)
                {
                    var line = arrayContent[i];
                    for (int j = 0; j < line.Length; j++)
                    {
                        CharArray[i, j] = line[j];
                    }
                }
                
                // Top
                Sides.Add(ImageContent.First());
                
                // Right
                var right = "";
                foreach (var line in Enumerable.Reverse(ImageContent))
                {
                    right += line.Last();
                }
                Sides.Add(right);
                
                // Bottom
                var bottom = ImageContent.Last();
                Sides.Add(string.Concat(bottom.Reverse()));
                
                // Left
                Sides.Add(string.Concat(ImageContent.Select(i => i.First())));

                foreach (var side in Sides)
                {
                    LinkedSides.Add((side, null));
                }
                UnmatchedSides = new List<string>(Sides);
            }

            public void Link(string side, Tile link)
            {
                var index = UnmatchedSides.FindIndex(s => s == side);
                UnmatchedSides.Remove(side);
                LinkedSides[index] = (side, link);
            }
        }

        public override long SolveB()
        {
            return 0;
        }
    }
}
