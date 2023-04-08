using System;
using System.Collections.Generic;
using System.IO;
/***************************************************************
*Name : Priority Queue
* Author: Alexander Kelly
* Created : 4/8/ 2023
* Course: CIS 152 - Data Structure
* Version: 1.0
* OS: Windows 11
* IDE: Visual Studio 2019
* Copyright : Cannot copyright Academic Material.
* Description : Dijkstra Algorithm
* Academic Honesty: I attest that this is my original work.
* I have not used unauthorized source code, either modified or
* unmodified. I have not given other fellow student(s) access
* to my program.
***************************************************************/
public class Dijkstra
{
    // Method to find the shortest path between two vertices in a weighted graph
    public static Tuple<int, List<string>> FindShortestPath(Dictionary<string, Dictionary<string, int>> graph, string start, string end)
    {
        // Initialize dictionaries and sets to store shortest path values, previous vertices, and unvisited nodes
        var shortestPath = new Dictionary<string, int>();
        var previous = new Dictionary<string, string>();
        var unvisitedNodes = new HashSet<string>();

        // Set initial values for all vertices
        foreach (var vertex in graph.Keys)
        {
            shortestPath[vertex] = int.MaxValue;
            unvisitedNodes.Add(vertex);
        }

        // Set the start vertex distance to 0
        shortestPath[start] = 0;

        // Iterate until all nodes are visited
        while (unvisitedNodes.Count != 0)
        {
            // Find the vertex with the smallest distance
            string currentMinVertex = null;
            foreach (var vertex in unvisitedNodes)
            {
                if (currentMinVertex == null || shortestPath[vertex] < shortestPath[currentMinVertex])
                {
                    currentMinVertex = vertex;
                }
            }

            // Remove the current vertex from unvisited nodes
            unvisitedNodes.Remove(currentMinVertex);

            // Stop the loop if the end vertex is reached
            if (currentMinVertex == end)
            {
                break;
            }

            // Update the distances of neighboring vertices
            foreach (var neighbor in graph[currentMinVertex])
            {
                int tentativeDistance = shortestPath[currentMinVertex] + neighbor.Value;
                if (tentativeDistance < shortestPath[neighbor.Key])
                {
                    shortestPath[neighbor.Key] = tentativeDistance;
                    previous[neighbor.Key] = currentMinVertex;
                }
            }
        }

        // Reconstruct the shortest path
        var path = new List<string>();
        var current = end;
        while (current != null)
        {
            path.Add(current);
            previous.TryGetValue(current, out current);
        }

        // Reverse the path to get the correct order
        path.Reverse();

        // Return the shortest path distance and the path itself
        return new Tuple<int, List<string>>(shortestPath[end], path);
    }

    // Main method
    public static void Main(string[] args)
    {
        // Read the graph from the text file
        var graph = ReadGraphFromFile("graph.txt");
        string start = "A";
        string end = "L";

        // Find the shortest path and print the result
        Tuple<int, List<string>> result = FindShortestPath(graph, start, end);
        Console.WriteLine($"Shortest path from {start} to {end} is {result.Item1}");
        Console.WriteLine($"Path: {string.Join(" -> ", result.Item2)}");
    }



    // Method to read the graph from a file and return a dictionary representation
    private static Dictionary<string, Dictionary<string, int>> ReadGraphFromFile(string filename)
    {
        var graph = new Dictionary<string, Dictionary<string, int>>();

        // Open the file and read it line by line
        using (var file = new StreamReader(filename))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                // Split the line into tokens (city1, city2, distance)
                var tokens = line.Split(' ');
                string city1 = tokens[0];
                string city2 = tokens[1];
                int distance = int.Parse(tokens[2]);

                // Add the connection to the graph, initializing a new dictionary for the city if it doesn't exist
                if (!graph.ContainsKey(city1))
                {
                    graph[city1] = new Dictionary<string, int>();
                }
                graph[city1][city2] = distance;
            }
        }

        // Return the graph as a dictionary of dictionaries
        return graph;
    }
}

