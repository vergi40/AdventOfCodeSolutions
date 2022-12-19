using System.Diagnostics;
using _2021_AoC;
using vergiCommon;

namespace _2022_AoC
{
    internal class Day16 : DayBase
    {
        public Day16() : base("16") { }

        public override long SolveA()
        {
            var startNodeName = "AA";
            var graph = GraphCreator.Create(ReadTestContent());
            GraphCreator.Visualize(graph);

            var solver = new RouteSolver(graph, startNodeName);
            solver.GenerateRouteEvaluation();
            var route = solver.Solve(startNodeName);

            var acc = 0;
            for (int i = 0; i < 30; i++)
            {
                var message = $"Minute {i + 1}: ";
                // Sum up opened valves
                acc += graph.Values.Where(n => n.Opened).Sum(n => n.FlowRate);

                var (nextOperation, nextNode) = route[i];
                if (nextOperation == StepType.Idle)
                {
                    // All opened already
                }
                else if(nextOperation == StepType.OpenValve)
                {
                    // 1. Already at valve -> open
                    nextNode.Opened = true;
                    message += $"Opening {nextNode}. ";
                }
                else
                {
                    // 2. Move to next
                    message += $"Travelling to {nextNode}. ";
                }

                message += $"Total acc: {acc}";
                Console.WriteLine(message);
            }

            return acc;
        }
        

        public override long SolveB()
        {
            return 0;
        }

        
        internal class Node
        {
            public int FlowRate { get; }
            public string Name { get; }
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

        public enum StepType{OpenValve, MoveToNext, Idle}

        class RouteSolver
        {
            private string StartNodeName { get; }
            public IReadOnlyDictionary<string, Node> Graph { get; }

            /// <summary>
            /// For each node, save all possible routes
            /// </summary>
            private Dictionary<Node, List<RouteData>> _routes = new();

            public RouteSolver(IReadOnlyDictionary<string, Node> graph, string startNodeName)
            {
                Graph = graph;
                foreach (var node in graph.Values)
                {
                    _routes.Add(node, new());
                }

                StartNodeName = startNodeName;
            }

            public void GenerateRouteEvaluation()
            {
                Console.WriteLine($"Generate shortest routes - start.");
                var clock = Stopwatch.StartNew();
                // Include start node
                var nodesWithFlow = Graph.Values.Where(n => n.FlowRate > 0 || n.Name == StartNodeName).ToList();

                var rawRoutes = new List<List<Node>>();
                for (int i = 0; i < nodesWithFlow.Count - 1; i++)
                {
                    for (int j = 1; j < nodesWithFlow.Count; j++)
                    {
                        var start = nodesWithFlow[i];
                        var target = nodesWithFlow[j];
                        var shortest = ShortestRouteTo(target, new List<Node> { start });
                        if (!shortest.Any()) throw new ArgumentException("No route found");
                        rawRoutes.Add(shortest);
                    }
                }

                Console.WriteLine(
                    $"Generate shortest routes - stop. Time elapsed: {clock.Elapsed.ToString()}. Total route count: {rawRoutes.Count}");
                Console.WriteLine($"Generate route evals - start.");
                clock = Stopwatch.StartNew();
                foreach (var rawRoute in rawRoutes)
                {
                    EvaluateAndSave(rawRoute);
                }

                Console.WriteLine($"Generate route evals - stop. Time elapsed: {clock.Elapsed.ToString()}.");
            }

            public List<(StepType next, Node nextNode)> Solve(string startNode)
            {
                // Try every combination
                Console.WriteLine($"Solve all combinations - start.");
                var clock = Stopwatch.StartNew();


                Console.WriteLine($"Solve all combinations - stop. Time elapsed: {clock.Elapsed.ToString()}.");
                return new List<(StepType, Node)>();
            }

            /// <summary>
            /// Generate detailed routes
            /// </summary>
            private void EvaluateAndSave(List<Node> rawRoute)
            {
                if (rawRoute.Count < 2) return;

                // Generate 2 versions: Only large flows opened or all opened
                var cutoff = 10;
                var shortRoute = new List<(StepType, Node)>();
                foreach (var node in rawRoute)
                {
                    shortRoute.Add((StepType.MoveToNext, node));
                    if (node.FlowRate > cutoff)
                    {
                        shortRoute.Add((StepType.OpenValve, node));
                    }
                }

                _routes[rawRoute[0]].Add(new RouteData(shortRoute));

                if (rawRoute.Any(n => n.FlowRate > 0 && n.FlowRate < 10))
                {
                    var longRoute = new List<(StepType, Node)>();
                    foreach (var node in rawRoute)
                    {
                        longRoute.Add((StepType.MoveToNext, node));
                        if (node.FlowRate > 0)
                        {
                            longRoute.Add((StepType.OpenValve, node));
                        }
                    }

                    _routes[rawRoute[0]].Add(new RouteData(longRoute));
                }
            }

            private static int Evaluate(IReadOnlyList<(StepType, Node)> route)
            {
                // TODO naive
                var sum = 0;
                foreach (var (stepType, node) in route)
                {
                    if (stepType == StepType.OpenValve)
                    {
                        sum += node.FlowRate;
                    }
                }

                // Standardize
                return sum / route.Count;
            }


            // Algorithms
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
                        if (distance < shortest) shortest = distance;
                    }
                }

                return shortest;
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

            class RouteData
            {
                public Guid Id { get; } = Guid.NewGuid();
                public IReadOnlyList<(StepType, Node)> Route { get; }

                public Node Start { get; }
                public Node End { get; }

                public int Evaluation { get; set; }

                public RouteData(IReadOnlyList<(StepType, Node)> route)
                {
                    Route = route;
                    Start = route.First().Item2;
                    End = route.Last().Item2;

                    Evaluation = Evaluate(route);
                }
            }
        }

        public static class GraphCreator
        {
            public static IReadOnlyDictionary<string, Node> Create(IReadOnlyList<string> content)
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

            public static void Visualize(IReadOnlyDictionary<string, Node> graph, string fileName = "graph.dot")
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
                File.WriteAllLines(GetPath.ThisAssembly() + $"\\{fileName}", dotCode);
            }
        }
    }
}
