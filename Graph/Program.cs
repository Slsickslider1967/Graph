using System.Linq.Expressions;
using System.Runtime.InteropServices;

class MainProgram
{
    static Graph<string> graph;
    static void Main(string[] args)
    {
        graph = new Graph<string>();
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("--------------------------");
            Console.WriteLine("1 - Add values \n2 - Add edges \n3 - Display values \n4 - Search value \n5 - DFS \n6 - Preset \n7 - BFS \n8 - Exit");
            Console.WriteLine("--------------------------");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                        Console.WriteLine("How many values do you want to add?");
                        Console.WriteLine("--------------------------");
                    int numberOfValues;
                    if (int.TryParse(Console.ReadLine(), out numberOfValues))
                    {
                        for (int i = 0; i < numberOfValues; i++)
                        {
                            Console.WriteLine("Enter value to add:");
                            string valueToAdd = Console.ReadLine();
                            graph.AddNode(valueToAdd);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid number.");
                    }
                    break;
                case "2":
                    Console.WriteLine("How many edges do you want to add?");
                    Console.WriteLine("--------------------------");
                    int numberOfEdges;
                    if (int.TryParse(Console.ReadLine(), out numberOfEdges))
                    {
                        for (int i = 0; i < numberOfEdges; i++)
                        {
                            bool Directed = false;
                            Console.WriteLine("Is the edge directed? (y/n):");
                            string directedInput = Console.ReadLine();
                            if (directedInput.ToLower() == "y")
                            {
                                Directed = true;
                            }
                            Console.WriteLine("Enter 'from' value:");
                            string fromValue = Console.ReadLine();
                            Console.WriteLine("Enter 'to' value:");
                            
                            string toValue = Console.ReadLine();
                            graph.AddEdge(fromValue, toValue, Directed);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid number.");
                    }
                    break;
                case "3":
                    Console.WriteLine("Graph structure:");
                    Console.WriteLine("--------------------------");
                    graph.BuildList();
                    break;
                case "4":
                    Console.WriteLine("Enter value to search:");
                    Console.WriteLine("--------------------------");
                    string valueToSearch = Console.ReadLine();
                    if (false) // Placeholder for search functionality
                        Console.WriteLine("Value found.");
                    else
                        Console.WriteLine("Value not found.");
                    break;
                case "5":
                    List<string> Visited = new List<string>();
                    Console.WriteLine("Where to start DFS:");
                    string startNode = Console.ReadLine();
                    graph.DFS(startNode, Visited);
                    Console.WriteLine("DFS complete.");
                    break;
                case "6":
                    graph.AddNode("A");
                    graph.AddNode("B");
                    graph.AddNode("E");
                    graph.AddNode("F");
                    graph.AddNode("G");
                    graph.AddNode("H");
                    graph.AddNode("K");
                    graph.AddNode("M");

                    graph.AddEdge("A", "B");
                    graph.AddEdge("A", "F");
                    graph.AddEdge("A", "H");
                    graph.AddEdge("B", "E");
                    graph.AddEdge("B", "G");
                    graph.AddEdge("B", "H");
                    graph.AddEdge("E", "F");
                    graph.AddEdge("E", "G");
                    graph.AddEdge("E", "M");
                    graph.AddEdge("G", "H");
                    graph.AddEdge("G", "M");
                    graph.AddEdge("H", "K");    
                    graph.AddEdge("K", "M");

                    Console.WriteLine("Preset graph created.");
                    break;
                case "7":
                    Console.WriteLine("Where to start BFS:");
                    string bfsStartNode = Console.ReadLine();
                    graph.BFS(bfsStartNode);
                    Console.WriteLine("BFS complete.");
                    break;
                case "8":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

}

class Graph<T>
{
    class Node
    {
        public T Value;
        public List<Node> Neighbors;

        public Node(T value)
        {
            Value = value;
            Neighbors = new List<Node>();
        }

        public void AddEdge(Node to)
        {
            Neighbors.Add(to);
        }
    }

    private Dictionary<Node, List<Node>> _Nodes;

    public Graph()
    {
        _Nodes = new Dictionary<Node, List<Node>>();
    }

    public void AddNode(T value)
    {
        var newNode = new Node(value);
        _Nodes.Add(newNode, new List<Node>());
    }

    public void AddEdge(T from, T to, bool Directed = false)
    {
        Node fromNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(from));
        Node toNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(to));

        if (fromNode != null && toNode != null )
        {
            fromNode.AddEdge(toNode);
            if (!Directed)
            {
                toNode.AddEdge(fromNode);
            }
        }
    }

    public void BuildList()
    {
        ListNodes();
    }

    private void ListNodes()
    {
        foreach (var node in _Nodes)
        {
            Console.WriteLine(node.Key.Value);
            foreach (var neighbor in node.Key.Neighbors)
            {
                Console.WriteLine("  -> " + neighbor.Value);
            }
        }
    }

    private void DFS(Node node, HashSet<Node> visited)
    {
        visited.Add(node);
        foreach (var neighbor in node.Neighbors)
        {
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, visited);
            }
        }
    }

    public void DFS(string StartNode, List<string> visitedValues)
    {
        HashSet<Node> visited = new HashSet<Node>();
        DFS(_Nodes.Keys.FirstOrDefault(n => n.Value.Equals(StartNode)), visited);
        foreach (var node in visited)
        {
            visitedValues.Add(node.Value.ToString());
            Console.WriteLine(node.Value);
        }
    }

    public void BFS(string StartNode)
    {
        HashSet<Node> visited = new HashSet<Node>();
        Queue<Node> queue = new Queue<Node>();
        Node startNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(StartNode));
        queue.Enqueue(startNode);
        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            if (!visited.Contains(current))
            {
                visited.Add(current);
                Console.WriteLine(current.Value);
                foreach (var neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor) && !queue.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
    }
}