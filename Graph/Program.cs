using System.Linq;
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
            Console.WriteLine("1 - Add values \n2 - Add edges \n3 - Display values \n4 - Search value \n5 - DFS \n6 - Preset \n7 - BFS \n8 - Shortest Path \n9 - Exit");
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
                            bool Weighted = false;
                            Console.WriteLine("Is the edge directed? (y/n):");
                            string directedInput = Console.ReadLine();
                            if (directedInput.ToLower() == "y")
                            {
                                Directed = true;
                            }
                            Console.WriteLine("Is the edge weighted? (y/n):");
                            string weightedInput = Console.ReadLine();
                            if (weightedInput.ToLower() == "y")
                            {
                                Weighted = true;
                            }
                            Console.WriteLine("Enter 'from' value:");
                            string fromValue = Console.ReadLine();
                            Console.WriteLine("Enter 'to' value:");
                            if(Weighted)
                            {
                                Console.WriteLine("Enter weight:");
                                string toValueWithWeight = Console.ReadLine();
                                var parts = toValueWithWeight.Split(',');
                                string toValue = parts[0];
                                int weight = int.Parse(parts[1]);
                                // Note: Weight handling is not implemented in the Graph class
                                graph.AddEdge(fromValue, toValue, Directed, Weighted, weight);
                            }
                            else
                            {
                                string toValue = Console.ReadLine();
                                graph.AddEdge(fromValue, toValue, Directed);
                            }
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

                    graph.AddEdge("A", "B", false, true, 12);
                    graph.AddEdge("A", "F", false, true, 5);
                    graph.AddEdge("A", "H", false, true, 8);
                    graph.AddEdge("B", "E", false, true, 9);
                    graph.AddEdge("B", "G", false, true, 3);
                    graph.AddEdge("B", "H", false, true, 9);
                    graph.AddEdge("E", "F", false, true, 18);
                    graph.AddEdge("E", "G", false, true, 7);
                    graph.AddEdge("E", "M", false, true, 26);
                    graph.AddEdge("G", "H", false, true, 4);
                    graph.AddEdge("G", "M", false, true, 13);
                    graph.AddEdge("H", "K", false, true, 47);
                    graph.AddEdge("K", "M", false, true, 34);

                    Console.WriteLine("Preset graph created.");
                    break;
                case "7":
                    Console.WriteLine("Where to start BFS:");
                    string bfsStartNode = Console.ReadLine();
                    graph.BFS(bfsStartNode);
                    Console.WriteLine("BFS complete.");
                    break;
                case "8":
                    Console.WriteLine("Enter start node for shortest path:");
                    string spStartNode = Console.ReadLine();
                    Console.WriteLine("Enter end node for shortest path:");
                    string spEndNode = Console.ReadLine();
                    graph.ShortestPath(spStartNode, spEndNode);
                    break;
                case "9":
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
        public Dictionary<Node, int> Neighbors;

        public Node(T value)
        {
            Value = value;
            Neighbors = new Dictionary<Node, int>();
        }

        public void AddEdge(Node to, int weight = 0)
        {
            Neighbors.Add(to, weight);
            
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

    public void AddEdge(T from, T to, bool Directed = false, bool Weighted = false, int weight = 0)
    {
        Node fromNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(from));
        Node toNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(to));

        if (fromNode != null && toNode != null)
        {
            fromNode.AddEdge(toNode);
            if (!Directed)
            {
                toNode.AddEdge(fromNode);
            }
            if (Weighted)
            {
                
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
                Console.WriteLine("  -> " + neighbor.Key.Value);
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


    private void DFS(Node node, HashSet<Node> visited)
    {
        visited.Add(node);
        foreach (var neighbor in node.Neighbors)
        {
            if (!visited.Contains(neighbor.Key))
            {
                DFS(neighbor.Key, visited);
            }
        }
    }

    public void BFS(string StartNode)
    {
        List<Node> visited = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        Node startNode = _Nodes.FirstOrDefault(kvp => kvp.Value.Equals(StartNode)).Key;
        queue.Enqueue(startNode);
        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            if (!visited.Contains(current))
            {
                visited.Add(current);
                Console.WriteLine("Visiting: " + current.Value.ToString());
                foreach (var neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor.Key) && !queue.Contains(neighbor.Key))
                    {
                        queue.Enqueue(neighbor.Key);
                    }
                }
            }
        }
    }

    public void ShortestPath(string startValue, string endValue)
    {
        Node startNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(startValue));
        Node endNode = _Nodes.Keys.FirstOrDefault(n => n.Value.Equals(endValue));

        if (startNode == null || endNode == null)
            return;

        Dictionary<Node, int> distances = new Dictionary<Node, int>();
        Dictionary<Node, Node> predecessors = new Dictionary<Node, Node>();
        PriorityQueue<Node, int> PriorityQueue = new PriorityQueue<Node, int>();

        foreach (var node in _Nodes.Keys)
            distances[node] = int.MaxValue;

        distances[startNode] = 0;
        predecessors[startNode] = null;
        PriorityQueue.Enqueue(startNode, 0);

        while (PriorityQueue.Count > 0)
        {
            Node current = PriorityQueue.Dequeue();

            if (current.Equals(endNode))
                break;

            foreach (var neighbor in current.Neighbors)
            {
                Node neighborNode = neighbor.Key;
                int weight = neighbor.Value;

                int newDist = distances[current] + weight;

                if (newDist < distances[neighborNode])
                {
                    distances[neighborNode] = newDist;
                    predecessors[neighborNode] = current;
                    PriorityQueue.Enqueue(neighborNode, newDist);
                }
            }
        }

        if (!predecessors.ContainsKey(endNode))
        {
            Console.WriteLine("No path found.");
            return;
        }

        List<T> path = new List<T>();
        for (Node at = endNode; at != null; at = predecessors[at])
            path.Add(at.Value);

        path.Reverse();
        Console.WriteLine(
            $"Shortest path (cost {distances[endNode]}): " +
            string.Join(" -> ", path)
        );
    }

}