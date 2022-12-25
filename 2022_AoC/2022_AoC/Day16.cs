using _2021_AoC;
using _2022_AoC.Day16Helpers;
using vergiCommon;

namespace _2022_AoC
{
    internal partial class Day16 : DayBase
    {
        public Day16() : base("16") { }

        public override long SolveA()
        {
            var (startNodeName, content) = ("AA", Content);
            //var (startNodeName, content) = ("AA", ReadTestContent());
            var graph = GraphCreator.Create(content);
            GraphCreator.Visualize(graph);

            var solver = new RouteSolver(graph, startNodeName);
            solver.GenerateRouteEvaluation();
            var route = solver.Solve(startNodeName);

            var opened = new HashSet<Node>();
            var acc = 0;

            for (int i = 0; i < 30; i++)
            {
                var message = $"Minute {i + 1}: ";
                // Sum up opened valves
                acc += opened.Sum(n => n.FlowRate);

                var (nextOperation, nextNode) = route[i];
                if (nextOperation == StepType.Idle)
                {
                    // All opened already
                }
                else if(nextOperation == StepType.OpenValve)
                {
                    // 1. Already at valve -> open
                    opened.Add(nextNode);
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

            // 1195
            return acc;
        }
        

        public override long SolveB()
        {
            return 0;
        }


        public enum StepType{OpenValve, MoveToNext, Idle, DuplicateStart}

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
