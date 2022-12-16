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


            return 0;
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

            public override string ToString() => $"{Name}: {FlowRate}. Visited: {Visited}";

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
