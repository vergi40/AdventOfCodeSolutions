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

    public double Evaluation { get; set; }

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
        Evaluation = RouteSolver.Evaluate(route);
    }

    public override string ToString()
    {
        return $"{Start.Name} -> {End.Name}, l:{Length}, e:{Evaluation:F2}";
    }
}