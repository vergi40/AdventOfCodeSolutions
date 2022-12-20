using System.Diagnostics;

namespace _2022_AoC.Day16Helpers;

internal class RouteSolver
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

    public List<(Day16.StepType next, Node nextNode)> Solve(string startNode)
    {
        // Try every combination
        Console.WriteLine($"Solve all combinations - start.");
        var clock = Stopwatch.StartNew();


        Console.WriteLine($"Solve all combinations - stop. Time elapsed: {clock.Elapsed.ToString()}.");
        return new List<(Day16.StepType, Node)>();
    }

    /// <summary>
    /// Generate detailed routes
    /// </summary>
    private void EvaluateAndSave(List<Node> rawRoute)
    {
        if (rawRoute.Count < 2) return;

        // Generate 2 versions: Only large flows opened or all opened
        var cutoff = 10;
        var shortRoute = new List<(Day16.StepType, Node)>();
        foreach (var node in rawRoute)
        {
            shortRoute.Add((Day16.StepType.MoveToNext, node));
            if (node.FlowRate > cutoff)
            {
                shortRoute.Add((Day16.StepType.OpenValve, node));
            }
        }

        _routes[rawRoute[0]].Add(new RouteData(shortRoute));

        if (rawRoute.Any(n => n.FlowRate > 0 && n.FlowRate < 10))
        {
            var longRoute = new List<(Day16.StepType, Node)>();
            foreach (var node in rawRoute)
            {
                longRoute.Add((Day16.StepType.MoveToNext, node));
                if (node.FlowRate > 0)
                {
                    longRoute.Add((Day16.StepType.OpenValve, node));
                }
            }

            _routes[rawRoute[0]].Add(new RouteData(longRoute));
        }
    }

    public static int Evaluate(IReadOnlyList<(Day16.StepType, Node)> route)
    {
        // TODO naive
        var sum = 0;
        foreach (var (stepType, node) in route)
        {
            if (stepType == Day16.StepType.OpenValve)
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

    //private Node? SelectBest(Dictionary<string, Node> graph, Node currentNode, int iteration)
    //{
    //    // Naive
    //    // * Select node that accumulates largest total value (minus the path it takes there)
    //    var left = 30 - iteration;

    //    var bestValue = 0;
    //    Node? bestNode = null;
    //    foreach (var node in graph.Values.Where(n => !n.Opened))
    //    {
    //        var distanceToTarget = ShortestDistanceTo(node, new List<Node> { currentNode });

    //        // Weight. Could improve with some path eval
    //        var total = node.FlowRate * left - distanceToTarget;

    //        if (total > bestValue)
    //        {
    //            bestValue = total;
    //            bestNode = node;
    //        }
    //    }

    //    return bestNode;
    //}
}