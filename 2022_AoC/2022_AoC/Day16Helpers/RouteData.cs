namespace _2022_AoC.Day16Helpers;

internal class RouteData
{
    public Guid Id { get; } = Guid.NewGuid();
    public IReadOnlyList<(Day16.StepType, Node)> Route { get; }

    public Node Start { get; }
    public Node End { get; }

    public int Evaluation { get; set; }

    public RouteData(IReadOnlyList<(Day16.StepType, Node)> route)
    {
        Route = route;
        Start = route.First().Item2;
        End = route.Last().Item2;

        Evaluation = RouteSolver.Evaluate(route);
    }
}