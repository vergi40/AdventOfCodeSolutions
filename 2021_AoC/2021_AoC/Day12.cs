using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021_AoC
{
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

    internal class Day12:DayBase
    {
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

            // 3037 - too low
            foreach (var resultLine in ResultsAsString)
            {
                Console.WriteLine(resultLine);
            }
            return result;
        }

        // TODO DELETE ------------
        private List<List<Node>> Results { get; set; } = new();
        private List<string> ResultsAsString { get; set; } = new();
        private const bool ConstDebugPrint = false;
        // ----------------------

        private void SearchTillEnd(IReadOnlyList<Node> path)
        {
            var current = path.Last();
            foreach (var next in current.Nodes)
            {
                if (next.Name == "end")
                {
                    var endPath = new List<Node>(path);
                    endPath.Add(next);
                    DebugPrint(endPath);
                    ResultsAsString.Add(NodesToString(endPath));
                }
                else if (next.CanVisit(path))
                {
                    var nextPath = new List<Node>(path);
                    nextPath.Add(next);
                    DebugPrint(nextPath);

                    SearchTillEnd(nextPath);
                }
            }
        }


        public override long SolveB()
        {
            return 0;
        }

        private static void DebugPrint(List<Node> nodes)
        {
            if(ConstDebugPrint) Console.WriteLine(NodesToString(nodes));
        }
        private static string NodesToString(List<Node> nodes)
        {
            return $"{string.Join(", ", nodes.Select(n => n.Name))}";
        }

        class Node
        {
            public string Name { get; }
            public List<Node> Nodes { get; set; } = new List<Node>();


            private readonly bool _multiTravel;
            private readonly bool _start;
            private readonly bool _end;

            public Node(string name)
            {
                Name = name;

                if (char.IsUpper(name[0]))
                {
                    _multiTravel = true;
                }
                else
                {
                    _multiTravel = false;
                }

                if (name == "start") _start = true;
                if (name == "end") _end = true;
            }
            
            public override string ToString()
            {
                return $"{Name}: {NodesToString(Nodes)}";
            }

            public bool CanVisit(IReadOnlyList<Node> history)
            {
                if (_multiTravel)
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
}
