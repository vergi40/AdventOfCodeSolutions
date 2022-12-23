namespace _2022_AoC.Day16Helpers;

internal class RouteData
{
    public IReadOnlyList<(Day16.StepType, Node)> Route { get; }

    public Node Start { get; }
    public Node End { get; }

    /// <summary>
    /// From start to end, including valve opening, -1
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Nodes in path that stop to open valve.
    /// </summary>
    public HashSet<Node> Valves { get; }

    public RouteData(IReadOnlyList<(Day16.StepType, Node)> route)
    {
        Route = route;
        Start = route.First().Item2;
        End = route.Last().Item2;
        Length = route.Count -1;

        Valves = route.Where(n => n.Item1 == Day16.StepType.OpenValve).Select(n => n.Item2).ToHashSet();
    }

    public override string ToString()
    {
        return $"{Start.Name} -> {End.Name}, l:{Length}";
    }
}

internal class RouteStep
{
    public Node Node { get; }
    public Day16.StepType StepType { get; }

    public Node Node2 { get; }
    public Day16.StepType StepType2 { get; }

    public (Day16.StepType, Node) Single => (StepType, Node);
    public List<(Day16.StepType, Node)> Iterate => new List<(Day16.StepType, Node)>() { (StepType, Node), (StepType2, Node2) };

    public RouteStep(Day16.StepType stepType, Node node)
    {
        StepType = stepType;
        Node = node;

        StepType2 = stepType;
        Node2 = node;
    }

    public RouteStep(Day16.StepType stepType, Node node, Day16.StepType stepType2, Node node2)
    {
        StepType = stepType;
        Node = node;

        StepType2 = stepType2;
        Node2 = node2;
    }

}