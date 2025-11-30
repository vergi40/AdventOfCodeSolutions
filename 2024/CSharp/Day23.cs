using NUnit.Framework;
using static CSharp.Day23;

namespace CSharp;

/// <summary>
/// Very over-engineered
/// Testing some equality functionality with records
/// </summary>
public class Day23 : DayBase
{
    protected override string DayNumber { get; set; } = "23";

    [Test]
    public void TestVerifyConnectionEquality()
    {
        var n1 = new Node("ab");
        var n2 = new Node("ba");
        var c1 = new Connection(n1, n2);
        var c2 = new Connection(n2, n1);

        var set = new HashSet<Connection>();
        set.Add(c1);
        Assert.IsFalse(set.Add(c2));

        Assert.That(c1, Is.EqualTo(c2));
    }

    [Test]
    public void TestVerifyConnectionTrioEquality()
    {
        var n1 = new Node("ab");
        var n2 = new Node("ba");
        var n3 = new Node("bc");
        var c1 = new Connection(n1, n2, n3);
        var c2 = new Connection(n2, n3, n1);

        var temp = new Connection(n1, n2);
        var c3 = new Connection(temp, n3);

        var set = new HashSet<Connection>();
        set.Add(c1);
        Assert.IsFalse(set.Add(c2));
        Assert.IsFalse(set.Add(c3));

        Assert.That(c1, Is.EqualTo(c2));
        Assert.That(c1, Is.EqualTo(c3));
    }

    [Test]
    public void TestVerifyRecordNodeEquality()
    {
        var n1a = new Node("ab");
        var n1b = new Node("ab");

        Assert.True(n1a.Equals(n1b));
        Assert.True(n1a == n1b);
        Assert.That(n1a, Is.EqualTo(n1b));

        var set = new HashSet<Node>();
        set.Add(n1a);
        Assert.IsFalse(set.Add(n1b));
    }

    [Test]
    public void Test1()
    {
        var groupsOfTwo = new HashSet<Connection>();
        var nodeList = new HashSet<Node>();
        var partOfCyclicGroup = new HashSet<Node>();
        var groupsOfThree = new HashSet<Connection>();
        var groups = new List<Group>();

        foreach (var line in Input)
        {
            var split = line.Split("-");
            var (n1, n2) = (new Node(split[0]), new Node(split[1]));

            var conn = new Connection(n1, n2);
            if (groupsOfTwo.Add(conn))
            {
                // Only adds new, not existing (as its hashset and records)
                nodeList.Add(n1);
                nodeList.Add(n2);
            }
        }

        //foreach (var newNode in nodeList)
        //{
        //    // ab-ba
        //    // ba-bc
        //    // ab -> add to new group. add ba
        //    // ba -> dont add
        //    // bc -> add to new group
        //    if (!partOfCyclicGroup.Contains(newNode))
        //    {
        //        var newGroup = new Group();
        //        newGroup.Nodes.Add(newNode);

        //        partOfCyclicGroup.Add(newNode);
        //        groups.Add(newGroup);

        //        foreach (var connection in groupsOfTwo)
        //        {
        //            if (connection.Nodes[0] == newNode)
        //            {
        //                var pair = connection.Nodes[1];
        //                newGroup.Nodes.Add(pair);
        //                partOfCyclicGroup.Add(pair);
        //            }
        //            else if (connection.Nodes[1] == newNode)
        //            {
        //                var pair = connection.Nodes[0];
        //                newGroup.Nodes.Add(pair);
        //                partOfCyclicGroup.Add(pair);
        //            }
        //        }
        //    }
        //}

        // Go through each node and combine it's links to trios
        foreach (var node in nodeList)
        {
            var linked = GetAllLinkedToNode(node, groupsOfTwo);
            foreach (var leftNode in linked)
            {
                foreach (var rightNode in linked)
                {
                    if (leftNode == rightNode)
                    {
                        continue;
                    }

                    var trio = new Connection(leftNode, node, rightNode);
                    var reverse = new Connection(rightNode, node, leftNode);
                    if (!groupsOfThree.Contains(trio) && !groupsOfThree.Contains(reverse))
                    {
                        groupsOfThree.Add(trio);
                    }
                }
            }
        }

        //foreach (var groupOfTwo in groupsOfTwo)
        //{
        //    // aa-ab
        //    // get connections to aa and create trio
        //    // get connections to ab and create trio
        //    var leftLinks = GetAllLinkedToNode(groupOfTwo.Nodes[0], groupsOfTwo);
        //    foreach (var link in leftLinks)
        //    {
        //        // Insert to "left"
        //        var newTrio = new Connection(link, groupOfTwo.Nodes[0], groupOfTwo.Nodes[1]);
        //        if (!newTrio.IsUnique()) continue;
        //        groupsOfThree.Add(newTrio);
        //    }
        //    var rightLinks = GetAllLinkedToNode(groupOfTwo.Nodes[1], groupsOfTwo);
        //    foreach (var link in rightLinks)
        //    {
        //        var newTrio = new Connection(groupOfTwo, link);
        //        if (!newTrio.IsUnique()) continue;
        //        groupsOfThree.Add(newTrio);
        //    }
        //}

        var withT = groupsOfThree.Where(c => c.AnyStartsWithT).ToList();
        // 1749 too high
        Assert.That(withT.Count, Is.EqualTo(10));
    }

    private static List<Node> GetAllLinkedToNode(Node node, HashSet<Connection> groupsOfTwo)
    {
        var result = new HashSet<Node>();
        foreach (var two in groupsOfTwo)
        {
            if (two.Nodes[0] == node)
            {
                var pair = two.Nodes[1];
                result.Add(pair);
            }
            else if (two.Nodes[1] == node)
            {
                var pair = two.Nodes[0];
                result.Add(pair);
            }
        }
        return result.ToList();
    }

    

    public record Group
    {
        public HashSet<Node> Nodes { get; } = [];
    }

    public record Node(string Name)
    {
    }

    /// <summary>
    /// Sort node names by name. Concat sorted names for comparable Hash
    /// </summary>
    public sealed class Connection
    {
        public List<Node> Nodes { get; }
        public string Hash { get; }
        public int NodeCount => Nodes.Count;
        public bool AnyStartsWithT { get; }

        public Connection(Node n1, Node n2)
        {
            Nodes = new List<Node> { n1, n2};
            Nodes = Nodes.OrderBy(c => c.Name).ToList();

            Hash = Nodes[0].Name + Nodes[1].Name;
            AnyStartsWithT = StartsWithT(Nodes);
        }

        public Connection(Node n1, Node n2, Node n3)
        {
            Nodes = new List<Node> { n1, n2, n3 };
            //if (string.Compare(n3.Name, n1.Name) < 0)
            //{
            //    Nodes = new List<Node> { n3, n2, n1 };
            //}
            //Nodes = Nodes.OrderBy(c => c.Name).ToList();

            Hash = Nodes[0].Name + Nodes[1].Name + Nodes[2].Name;
            AnyStartsWithT = StartsWithT(Nodes);
        }

        public Connection(Connection duo, Node n3)
        {
            Nodes = new List<Node> { duo.Nodes[0], duo.Nodes[1], n3 };
            //Nodes = Nodes.OrderBy(c => c.Name).ToList();

            Hash = Nodes[0].Name + Nodes[1].Name + Nodes[2].Name;
            AnyStartsWithT = StartsWithT(Nodes);
        }

        private static bool StartsWithT(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Name.StartsWith('t'))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// False if e.g. agageg or agegag
        /// </summary>
        /// <returns></returns>
        public bool IsUnique()
        {
            if (Nodes[0] == Nodes[1]) return false;
            if (NodeCount > 2)
            {
                if (Nodes[0] == Nodes[2]) return false;
                if (Nodes[1] == Nodes[2]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Connection connection && Equals(connection);
        }

        public bool Equals(Connection? p)
        {
            return Hash == p?.Hash;
        }

        public override string ToString()
        {
            return Hash.ToString();
        }
    }

        
}