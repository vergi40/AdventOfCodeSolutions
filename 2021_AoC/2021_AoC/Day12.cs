using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
    internal class Day12:DayBase
    {
        private List<string> ResultsAsString { get; set; } = new();

        public Day12() : base("12")
        {
        }

        public override long SolveA()
        {
            var map = new Dictionary<string, Node>();
            //foreach (var line in example1)
            //foreach (var line in example2)
            foreach (var line in Content)
            {
                var lineArray = line.Split('-');

                var name = lineArray[0];
                var name2 = lineArray[1];

                if (!map.TryGetValue(name, out var node))
                {
                    node = new Node(name);
                    map.Add(name, node);
                }
                if (!map.TryGetValue(name2, out var node2))
                {
                    node2 = new Node(name2);
                    map.Add(name2, node2);
                }

                node.Nodes.Add(node2);
                node2.Nodes.Add(node);
            }
            
            var start = map["start"];
            SearchTillEnd(new List<Node>() { start });

            var result = ResultsAsString.Count();
            
            //foreach (var resultLine in ResultsAsString)
            //{
            //    Console.WriteLine(resultLine);
            //}

            ResultsAsString.Clear();

            // 4304
            return result;
        }

        public override long SolveB()
        {
            var map = new Dictionary<string, Node2>();
            //foreach (var line in example1)
            //foreach (var line in example2)
            foreach (var line in Content)
            {
                var lineArray = line.Split('-');

                var name = lineArray[0];
                var name2 = lineArray[1];

                if (!map.TryGetValue(name, out var node))
                {
                    node = new Node2(name);
                    map.Add(name, node);
                }
                if (!map.TryGetValue(name2, out var node2))
                {
                    node2 = new Node2(name2);
                    map.Add(name2, node2);
                }

                node.Nodes.Add(node2);
                node2.Nodes.Add(node);
            }

            var start = map["start"];
            SearchTillEnd(new List<Node2>() { start });

            var result = ResultsAsString.Count();

            //foreach (var resultLine in ResultsAsString)
            //{
            //    Console.WriteLine(resultLine);
            //}

            ResultsAsString.Clear();
            return result;
        }

        private void SearchTillEnd(IReadOnlyList<Node> path)
        {
            var current = path.Last();
            foreach (var next in current.Nodes)
            {
                if (next.Name == "end")
                {
                    var endPath = new List<Node>(path) { next };
                    ResultsAsString.Add(NodesToString(endPath));
                }
                else if (next.CanVisit(path))
                {
                    var nextPath = new List<Node>(path) { next };
                    SearchTillEnd(nextPath);
                }
            }
        }
        private static string NodesToString(List<Node> nodes)
        {
            return $"{string.Join(", ", nodes.Select(n => n.Name))}";
        }

        class Node
        {
            public string Name { get; }
            public List<Node> Nodes { get; set; } = new();


            public bool MultiTravel { get; }

            public Node(string name)
            {
                Name = name;

                if (char.IsUpper(name[0]))
                {
                    MultiTravel = true;
                }
                else
                {
                    MultiTravel = false;
                }
            }
            
            public override string ToString()
            {
                return $"{Name}: {NodesToString(Nodes)}";
            }

            public virtual bool CanVisit(IReadOnlyList<Node> history)
            {
                if (MultiTravel)
                {
                    return true;
                }
                if (!history.Select(item => item.Name).Contains(Name))
                {
                    return true;
                }

                return false;
            }
        }

        class Node2 : Node
        {
            public Node2(string name) : base(name)
            {
            }

            public override bool CanVisit(IReadOnlyList<Node> history)
            {
                if (MultiTravel)
                {
                    return true;
                }

                if (Name == "start") return false;

                // This is probably slow as _____, but works!
                var dict = new Dictionary<string, int>();
                var singleTravelList = history.Where(item => !item.MultiTravel).ToList();
                singleTravelList.Add(this);
                foreach (var node in singleTravelList)
                {
                    if (!dict.TryAdd(node.Name, 1))
                    {
                        dict[node.Name]++;
                    }
                }

                var existsTwice = false;
                foreach (var counter in dict.Values)
                {
                    if (counter > 2) return false;
                    if (counter > 1)
                    {
                        if (existsTwice)
                        {
                            return false;
                        }
                        existsTwice = true;
                    }
                }
                return true;
            }
        }

        private static List<string> example1 = new()
        {
            "start-A",
            "start-b",
            "A-c",
            "A-b",
            "b-d",
            "A-end",
            "b-end",
        };

        private static List<string> example2 = new()
        {
            "dc-end",
            "HN-start",
            "start-kj",
            "dc-start",
            "dc-HN",
            "LN-dc",
            "HN-end",
            "kj-sa",
            "kj-HN",
            "kj-dc"
        };
    }


    static class TupleExtensions
    {
        public static IEnumerable<T> Enumerate<T>(this (T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
        }

        public static IEnumerable Enumerate2(this object tuple)
        {
            return tuple.GetType().GetProperties()
                .Select(property => property.GetValue(tuple));
        }

        public static IList<T> ToList<T>(this (T, T) tuple)
        {
            return new List<T>() { tuple.Item1, tuple.Item2 };
        }
    }
}
