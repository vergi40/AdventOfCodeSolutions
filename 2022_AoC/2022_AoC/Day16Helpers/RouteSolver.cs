using System.Diagnostics;
using static _2022_AoC.Day16;

namespace _2022_AoC.Day16Helpers;

internal class RouteSolver
{
    private readonly int _timeLimit;
    private string StartNodeName { get; }
    public IReadOnlyDictionary<string, Node> Graph { get; }
    public int ValveCount { get; }

    /// <summary>
    /// For each node, save all possible routes
    /// </summary>
    private Dictionary<Node, List<RouteData>> _routes = new();

    public RouteSolver(IReadOnlyDictionary<string, Node> graph, string startNodeName, int timeLimit)
    {
        _timeLimit = timeLimit;
        Graph = graph;
        var valves = graph.Values.Where(n => n.FlowRate > 0 || n.Name == startNodeName).ToList();
        ValveCount = valves.Count;
        foreach (var node in valves)
        {
            _routes.Add(node, new());
        }

        StartNodeName = startNodeName;
    }

    public void GenerateRouteEvaluation()
    {
        Console.WriteLine($"Generate shortest routes - start.");
        var clock = Stopwatch.StartNew();
        // Include start node, but only as start point
        var nodesWithFlow = Graph.Values.Where(n => n.FlowRate > 0 || n.Name == StartNodeName).ToList();

        var rawRoutes = new List<List<Node>>();
        for (int i = 0; i < nodesWithFlow.Count - 1; i++)
        {
            for (int j = i + 1; j < nodesWithFlow.Count; j++)
            {
                var endPoint1 = nodesWithFlow[i];
                var endPoint2 = nodesWithFlow[j];

                var shortest = ShortestRouteTo(endPoint2, new List<Node> { endPoint1 });
                
                if (!shortest.Any()) throw new ArgumentException("No route found");
                rawRoutes.Add(shortest);

                // Skip creating routes to start node
                if (endPoint2.Name == StartNodeName) continue;
                var reverse = shortest.ToList();
                reverse.Reverse();
                rawRoutes.Add(reverse);
            }
        }

        Console.WriteLine(
            $"Generate shortest routes - stop. Time elapsed: {clock.Elapsed.ToString()}. Total route count: {rawRoutes.Count}");
        Console.WriteLine($"Generate detailed routes - start.");
        clock = Stopwatch.StartNew();
        foreach (var rawRoute in rawRoutes)
        {
            GenerateDetailedRoute(rawRoute);
        }

        Console.WriteLine($"Generate detailed routes - stop. Time elapsed: {clock.Elapsed.ToString()}.");
    }

    /// <summary>
    /// Go through every combination with distance 30, return largest 
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    public IReadOnlyList<RouteStep> Solve(string startNode, bool debugPrintAll = false)
    {
        // Try every combination
        Console.WriteLine($"Solve all combinations - start.");
        var clock = Stopwatch.StartNew();

        var debugList = new DebugHelper();
        var max = 0.0;
        IReadOnlyList<RouteStep> best = new List<RouteStep>();

        var allPossibilities = new List<List<RouteData>>();
        foreach (var data in _routes[Graph[startNode]])
        {
            var routesList = EnumerateAllRec(new List<RouteData> { data }, new HashSet<Node>());
            allPossibilities.AddRange(routesList);
        }

        foreach (var routeList in allPossibilities)
        {
            if (!routeList.Any()) continue;
            var path = CompilePath(routeList);
            var eval = CalculateAccumulationForPath(path);
            if (eval > max)
            {
                max = eval;
                best = path;
            }

            if(debugPrintAll)
            {
                debugList.Results.Add(path);
                debugList.Evals.Add(eval);
            }
        }
        
        Console.WriteLine($"Solve all combinations - stop. Time elapsed: {clock.Elapsed.ToString()}. Possibilities: {allPossibilities.Count}");

        if(debugPrintAll) debugList.Print();
        return best;
    }

    public IReadOnlyList<RouteStep> SolveDuo(string startNode, bool debugPrintAll = false)
    {
        // Try every combination
        Console.WriteLine($"Solve all combinations - start.");
        var clock = Stopwatch.StartNew();

        var debugList = new DebugHelper();
        var max = 0.0;
        IReadOnlyList<RouteStep> best = new List<RouteStep>();

        var allPossibilities = new List<List<RouteData>>();
        foreach (var data in _routes[Graph[startNode]])
        {
            var routesList = EnumerateAllRec(new List<RouteData> { data }, new HashSet<Node>());
            allPossibilities.AddRange(routesList);
        }

        Console.WriteLine($"Time elapsed: {clock.Elapsed.ToString()}. " +
                          $"Single path possibilities: {allPossibilities.Count}");
        clock.Restart();

        Console.WriteLine($"Estimated possibility count: {Math.Pow(allPossibilities.Count, 2)}");
        var iter = 0;
        // Create every possible combination, compile and evaluate
        for (int i = 0; i < allPossibilities.Count - 1; i++)
        {
            var routeA = allPossibilities[i];

            // Find all diverging routes
            var openedA = routeA.Select(r => r.Valve).ToList();

            for (int j = i + 1; j < allPossibilities.Count; j++)
            {
                var routeB = allPossibilities[j];
                var openedB = routeB.Select(r => r.Valve).ToList();
                if (openedA[0] != openedB[0])
                {
                    // Only compile paths that have different opened valves
                    var compiled = CompileDuoPath(routeA, routeB);
                    var eval = CalculateAccumulationForPath(compiled);

                    if (eval > max)
                    {
                        max = eval;
                        best = compiled;
                    }
                }

                if (iter % 1_000_000 == 0)
                {
                    Console.WriteLine($"{iter} iterated. Elapsed: {clock.Elapsed.ToString()}. Max: {max}. ");
                }

                iter++;
            }
        }

        Console.WriteLine($"Solve all combinations - stop. Time elapsed: {clock.Elapsed.ToString()}. " +
                          $"Duo path possibilities: {allPossibilities.Count}");

        if (debugPrintAll) debugList.Print();
        return best;
    }


    private List<List<RouteData>> EnumerateAllRec(List<RouteData> current, HashSet<Node> opened)
    {
        var result = new List<List<RouteData>>();
        //
        if (opened.Count == ValveCount)
        {
            result.Add(current);
            return result;
        }

        var length = current.Sum(c => c.Length);
        var last = current.Last().End;
        var routesNotOpened = _routes[last]
            .Where(r => !opened.Contains(r.End)).ToList();

        foreach (var next in routesNotOpened)
        {
            if (length + next.Length <= _timeLimit)
            {
                var newOpened = new HashSet<Node>(opened);
                newOpened.UnionWith(next.Valves);
                var newList = new List<RouteData>(current) { next };
                
                var nextRoutes = EnumerateAllRec(newList, newOpened);
                result.AddRange(nextRoutes);
            }
        }
        if(!result.Any()) result.Add(current);
        return result;
    }
    
    /// <summary>
    /// Generate detailed routes. Open valve step only for last node
    /// </summary>
    private void GenerateDetailedRoute(List<Node> rawRoute)
    {
        if (rawRoute.Count < 2) return;

        // Special handling for the start node, as it'll be deleted later
        var route = new List<(StepType, Node)> { (StepType.DuplicateStart, rawRoute[0]) };

        var lastNode = rawRoute.Last();
        foreach (var node in rawRoute.Skip(1))
        {
            route.Add((StepType.MoveToNext, node));
            if (node == lastNode)
            {
                route.Add((StepType.OpenValve, node));
            }
        }

        _routes[rawRoute[0]].Add(new RouteData(route));
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

    private IReadOnlyList<RouteStep> CompilePath(List<RouteData> subRoutes)
    {
        var result = new List<RouteStep>();
        var opened = new HashSet<Node>();
        foreach (var routeData in subRoutes)
        {
            foreach (var (stepType, node) in routeData.Route)
            {
                if (stepType == StepType.DuplicateStart) continue;
                if (stepType == StepType.OpenValve)
                {
                    if (opened.Contains(node)) continue;
                    else opened.Add(node);
                }
                result.Add(new RouteStep(stepType, node));
            }
        }

        var operativeLength = result.Count;
        var lastNode = result.Last().Node;
        for (int i = 0; i < _timeLimit - operativeLength; i++)
        {
            result.Add(new RouteStep(StepType.Idle, lastNode));
        }
        return result;
    }

    private IReadOnlyList<RouteStep> CompileDuoPath(List<RouteData> subRoutes, List<RouteData> subRoutes2)
    {
        var compiled1 = CompilePath(subRoutes);
        var compiled2 = CompilePath(subRoutes2);

        var opened = new HashSet<Node>();
        var result = new List<RouteStep>();
        var iterA = 0;
        var iterB = 0;

        RouteStep stepA;
        RouteStep stepB;
        RouteStep resA;
        RouteStep resB;
        for (int i = 0; i < _timeLimit; i++)
        {
            if(iterA < compiled1.Count)
            {
                stepA = compiled1[iterA];
                if (stepA.StepType == StepType.OpenValve)
                {
                    if (opened.Contains(stepA.Node))
                    {
                        iterA++;
                    }
                    else
                    {
                        opened.Add(stepA.Node);
                    }
                }
            }
            if(iterB < compiled2.Count)
            {
                stepB = compiled2[iterB];
                if (stepB.StepType == StepType.OpenValve)
                {
                    if (opened.Contains(stepB.Node))
                    {
                        iterB++;
                    }
                    else
                    {
                        opened.Add(stepB.Node);
                    }
                }
            }

            if (iterA < compiled1.Count)
            {
                resA = compiled1[iterA];
            }
            else
            {
                resA = new RouteStep(StepType.Idle, compiled1.Last().Node);
            }
            if (iterB < compiled2.Count)
            {
                resB = compiled2[iterB];
            }
            else
            {
                resB = new RouteStep(StepType.Idle, compiled2.Last().Node);
            }
            result.Add(new RouteStep(resA.StepType, resA.Node, resB.StepType, resB.Node));

            iterA++;
            iterB++;
        }
        return result;
    }

    private int CalculateAccumulationForPath(IReadOnlyList<RouteStep> path)
    {
        // Same as SolveA but without logging
        var opened = new HashSet<Node>();

        var acc = 0;
        for (int i = 0; i < _timeLimit; i++)
        {
            // Sum up opened valves
            acc += opened.Sum(n => n.FlowRate);

            foreach (var nextStep in path[i].Iterate)
            {
                var (nextOperation, nextNode) = nextStep;
                if (nextOperation == StepType.OpenValve)
                {
                    // 1. Already at valve -> open
                    opened.Add(nextNode);
                }
            }
        }

        return acc;
    }

    class DebugHelper
    {
        public List<IReadOnlyList<RouteStep>> Results { get; } = new();
        public List<double> Evals { get; } = new();

        public void Print()
        {
            Console.WriteLine("DEBUG");
            foreach (var valueTuple in Results.Zip(Evals))
            {
                Console.WriteLine($"{valueTuple.Second}: {PrintPath(valueTuple.First)}");
            }
        }

        private string PrintPath(IReadOnlyList<RouteStep> path)
        {
            return String.Join(", ", path.Select(p => p.Node).Select(n => n.Name));
        }
    }

}