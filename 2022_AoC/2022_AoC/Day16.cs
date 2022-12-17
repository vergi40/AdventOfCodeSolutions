using _2021_AoC;
using vergiCommon;

namespace _2022_AoC
{
    internal class Day16 : DayBase
    {
        public Day16() : base("16") { }

        public override long SolveA()
        {
            var graph = CreateGraph(ReadTestContent());

            Visualize(graph);

            var currentNode = graph["AA"];
            var acc = 0;
            for (int i = 0; i < 30; i++)
            {
                var message = $"Minute {i + 1}: ";

                // Sum up opened valves
                acc += graph.Values.Where(n => n.Opened).Sum(n => n.FlowRate);
                
                var targetNode = SelectBest(graph, currentNode, i);
                if (targetNode == null)
                {
                    // All opened already
                    message += $"Total acc: {acc}";
                    Console.WriteLine(message);
                    continue;
                }
                else if (currentNode == targetNode)
                {
                    // 1. Already at valve -> open
                    currentNode.Opened = true;
                    message += $"Opening {currentNode}. ";
                }
                else
                {
                    // Route result: current + path
                    var route = ShortestRouteTo(targetNode, new List<Node> { currentNode });

                    // 2. Move to next
                    var nextNode = route[1];
                    message += $"Travelling to {nextNode}. ";
                    currentNode = nextNode;
                }

                message += $"Total acc: {acc}";
                Console.WriteLine(message);
            }

            return acc;
        }

        private Node? SelectBest(Dictionary<string, Node> graph, Node currentNode, int iteration)
        {
            // Naive
            // * Select node that accumulates largest total value (minus the path it takes there)
            var left = 30 - iteration;

            var bestValue = 0;
            Node? bestNode = null;
            foreach (var node in graph.Values.Where(n => !n.Opened))
            {
                var distanceToTarget = ShortestDistanceTo(node, new List<Node> { currentNode });

                // Weight. Could improve with some path eval
                var total = node.FlowRate * left - distanceToTarget;
                
                if (total > bestValue)
                {
                    bestValue = total;
                    bestNode = node;
                }
            }

            return bestNode;
        }

        private List<Node> ShortestRouteTo(Node target, List<Node> travelled)
        {
            var current = travelled.Last();
            var shortest = 100000;
            var shortestRoute = new List<Node>();

            foreach (var link in current.Links)
            {
                if (link == target)
                {
                    return new List<Node>(travelled) { link };
                }
                if (!travelled.Contains(link))
                {
                    var nextTravelled = new List<Node>(travelled) { link };

                    var route = ShortestRouteTo(target, nextTravelled);
                    if (!route.Any()) continue;
                    if (route.Count < shortest)
                    {
                        shortest = route.Count;
                        shortestRoute = route;
                    }
                }

                // Else circular or dead end, continue
            }
            return shortestRoute;
        }

        private int ShortestDistanceTo(Node target, List<Node> travelled)
        {
            if (travelled.Last() == target) return 0;
            var current = travelled.Last();
            var shortest = 100000;

            foreach (var link in current.Links)
            {
                if (link == target) return travelled.Count;
                if (!travelled.Contains(link))
                {
                    var nextTravelled = new List<Node>(travelled) { link };

                    var distance = ShortestDistanceTo(target, nextTravelled);
                    if(distance < shortest) shortest = distance;
                }
            }
            return shortest;
        }

        public override long SolveB()
        {
            return 0;
        }

        private void Visualize(Dictionary<string, Node> graph)
        {
            // https://github.com/harshsikhwal/csdot
            // Well that's one buggy package, scrap that. Do not waste your time in the future!

            // Compile direct dot code
            // http://www.graphviz.org/doc/info/lang.html
            // Run with: dot -Tpng graph.dot
            // Or open in Visual Code
            var graphType = "graph";
            var linkType = "--";
            var dotCode = new List<string>()
            {
                $"strict {graphType} 1",
                "{"
            };
            foreach (var node in graph.Values)
            {
                var line = $" {node.Name}_{node.FlowRate}";
                dotCode.Add(line);
                foreach (var link in node.Links)
                {
                    var linkLine = $"{line} {linkType} {link.Name}_{link.FlowRate}";
                    dotCode.Add(linkLine);
                }
            }

            dotCode.Add("}");
            File.WriteAllLines(GetPath.ThisAssembly() + "\\graph.dot", dotCode);
        }

        private Dictionary<string, Node> CreateGraph(IReadOnlyList<string> content)
        {
            var graph = new Dictionary<string, Node>();
            foreach (var line in content)
            {
                // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
                // On purpose use C#11 list patterns instead just hard indexes
                if (line.Split(' ') is ["Valve", var name, _, _, var flowString, _, _, _, _, .. var valves])
                {
                    // 
                    var flow = int.Parse(flowString.Split('=', ';')[1]);
                    var node = new Node(name, flow, string.Join(' ', valves));
                    graph.Add(name, node);
                }
                else
                {
                    throw new ArgumentException($"Unknown syntax {line}");
                }
            }

            foreach (var node in graph.Values)
            {
                node.CreateLinks(graph);
            }

            return graph;
        }

        class Node
        {
            public int FlowRate { get; }
            public string Name { get; }
            public bool Visited { get; set; }
            public bool Opened { get; set; }

            public List<Node> Links { get; set; } = new ();


            private readonly string _valveData;// "FF,DD"
            public Node(string name, int flowRate, string valveData)
            {
                Name = name;
                FlowRate = flowRate;
                _valveData = valveData;
            }

            public override string ToString() => $"{Name}: {FlowRate}";

            public void CreateLinks(IReadOnlyDictionary<string, Node> graph)
            {
                foreach (var valveId in _valveData.Split(',').Select(v => v.Trim()))
                {
                    Links.Add(graph[valveId]);
                }
            }
        }
    }
}
