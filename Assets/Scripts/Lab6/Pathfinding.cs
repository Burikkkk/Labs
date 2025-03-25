using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform endPoint;
    public GraphGenerator graphGenerator;
    public bool drawPath;

    private Graph graph;

    private void Start()
    {
        graph = graphGenerator.graph;
        FindPathDFS();
        FindPathBFS();
    }
    
    public List<Vertex> FindPathDFS()   // просто находит путь, не особо кратчайший
    {
        Vertex startVertex = graph.GetNearestVertex(transform.position);
        Vertex endVertex = graph.GetNearestVertex(endPoint.position);
        
        List<Vertex> path = new List<Vertex>();

        bool[] visited = new bool[graph.vertices.Count];    // тут посещенные вершины, чтобы не ходить по кругу
        visited[startVertex.id] = true; 
        
        Stack<Vertex> toVisit = new Stack<Vertex>();    // отсюда берем следующего соседа
        foreach (var neighbour in startVertex.neighbours)   // добавляем изначальных соседей
        {
            toVisit.Push(neighbour.vertex);
            neighbour.vertex.prev = startVertex;    // записываем в вершину, откуда в нее пришли, чтобы потом с конца восстановить путь
        }

        while (toVisit.Count != 0)
        {
            Vertex currentVertex = toVisit.Pop();   // берем текущую вершину для посещения

            if (currentVertex == endVertex) // однажды попадет сюда и выйдет
                break;

            foreach (var nextNeighbour in currentVertex.neighbours) // добавляем в стек всех соседей текущей вершины, дальше пойдем по ним
            {
                if(visited[nextNeighbour.vertex.id] == true)
                    continue;
                toVisit.Push(nextNeighbour.vertex);
                nextNeighbour.vertex.prev = currentVertex;
            }
            visited[currentVertex.id] = true;
            
        }

        Vertex currentPathVertex = endVertex;
        while (currentPathVertex != startVertex)    // восстанавливаем путь с конца, можно бы в отдельную функцию, тут везде такое
        {
            if(drawPath)
                Debug.DrawLine(currentPathVertex.transform.position, 
                    currentPathVertex.prev.transform.position, Color.green, 1000f);
            path.Add(currentPathVertex);
            currentPathVertex = currentPathVertex.prev;

        }
        path.Reverse();
        return path;
    }
    
    public List<Vertex> FindPathBFS()   // находит путь, кратчайший ТОЛЬКО если все расстояния между вершинами одинавые (maxNeighbourDistance = 3)
    {
        Vertex startVertex = graph.GetNearestVertex(transform.position);
        Vertex endVertex = graph.GetNearestVertex(endPoint.position);
        
        List<Vertex> path = new List<Vertex>();

        bool[] visited = new bool[graph.vertices.Count];
        visited[startVertex.id] = true;

        Queue<Vertex> toVisit = new Queue<Vertex>();    // все то же самое, но тут очередь
        foreach (var neighbour in startVertex.neighbours)
        {
            toVisit.Enqueue(neighbour.vertex);
            neighbour.vertex.prev = startVertex;
        }

        while (toVisit.Count != 0)
        {
            Vertex currentVertex = toVisit.Dequeue();

            if (currentVertex == endVertex)
                break;

            foreach (var nextNeighbour in currentVertex.neighbours)
            {
                if(visited[nextNeighbour.vertex.id] == true)
                    continue;
                toVisit.Enqueue(nextNeighbour.vertex);
                nextNeighbour.vertex.prev = currentVertex;
            }
            visited[currentVertex.id] = true;
            
        }

        Vertex currentPathVertex = endVertex;
        while (currentPathVertex != startVertex)
        {
            if(drawPath)
                Debug.DrawLine(currentPathVertex.transform.position, 
                    currentPathVertex.prev.transform.position, Color.blue, 1000f);
            path.Add(currentPathVertex);
            currentPathVertex = currentPathVertex.prev;

        }

        path.Reverse();
        return path;
    }
}
