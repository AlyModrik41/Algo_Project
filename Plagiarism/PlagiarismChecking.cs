using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Problem
{
    public static class PlagiarismChecking
    {
       public static int CheckPlagiarism(Tuple<string, string>[] matches, Tuple<string, string> query)
    {
        // Handle edge cases
        if (matches == null || matches.Length == 0 || query == null)
            return 0;

        // Check if query nodes are the same
        if (query.Item1 == query.Item2)
            return 0;

        // Build an adjacency list representation of the graph
        Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();

        foreach (var match in matches)
        {
            // Add edge from Item1 to Item2
            if (!graph.ContainsKey(match.Item1))
                graph[match.Item1] = new List<string>();
            graph[match.Item1].Add(match.Item2);

            // Add edge from Item2 to Item1 (undirected graph)
            if (!graph.ContainsKey(match.Item2))
                graph[match.Item2] = new List<string>();
            graph[match.Item2].Add(match.Item1);
        }

        // Check if both query nodes exist in the graph
        if (!graph.ContainsKey(query.Item1) || !graph.ContainsKey(query.Item2))
            return 0;

        // Use BFS to find the shortest path
        // Queue for BFS traversal
        Queue<string> queue = new Queue<string>();
        
        // Dictionary to track visited nodes and their distances from the start
        Dictionary<string, int> distances = new Dictionary<string, int>();
        
        // Add the start node to the queue and mark it as visited
        queue.Enqueue(query.Item1);
        distances[query.Item1] = 0;
        
        // BFS loop
        while (queue.Count > 0)
        {
            string current = queue.Dequeue();
            
            // Found the destination node
            if (current == query.Item2)
                return distances[current];
            
            // Process all neighbors
            foreach (string neighbor in graph[current])
            {
                // If this node hasn't been visited yet
                if (!distances.ContainsKey(neighbor))
                {
                    // Mark it as visited with distance = parent's distance + 1
                    distances[neighbor] = distances[current] + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        // If we've exhausted the queue and haven't found the end node,
        // then there's no path from start to end
        return 0;
    }
    }
}
