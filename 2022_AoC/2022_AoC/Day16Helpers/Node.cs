namespace _2022_AoC.Day16Helpers;

internal class Node
{
    public int FlowRate { get; }
    public string Name { get; }

    public List<Node> Links { get; set; } = new();


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