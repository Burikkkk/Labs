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
        FindPathDFS();  // хз правильно ли работает по нарисованному пути, было норм, стало хз
        FindPathBFS();
        //FindPathDijkstra(); // я думал, что крутой и еще могу такое, но не
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
        foreach (var v in path)
        {
            Debug.Log(v.id);
        }
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
        // foreach (var v in path)
        // {
        //     Debug.Log(v.id);
        // }
        return path;
    }

    public List<Vertex> FindPathDijkstra()  // не работает и запускать в таком виде не надо, зациклится и надо будет перезапускать юнити
    {
        Vertex startVertex = graph.GetNearestVertex(transform.position);
        Vertex endVertex = graph.GetNearestVertex(endPoint.position);
        
        List<Vertex> path = new List<Vertex>();
        
        SortedSet<Edge> distances = new SortedSet<Edge>();  // это по идее можно вместо бинарной кучи из книги (а можно и без нее и без этого)
        // тут массив ребер, потому что хранит кратчайшее расстояние от старта до какого-то другого (не важно, что не соседи)
        
        bool[] visited = new bool[graph.vertices.Count];
        visited[startVertex.id] = true;
        
        foreach (var neighbour in startVertex.neighbours)
        {
            distances.Add(new Edge(neighbour.vertex, neighbour.cost));
            neighbour.vertex.prev = startVertex;
        }

        Vertex currentVertex = startVertex;
        while (currentVertex.id != endVertex.id)
        {
            Edge currentEdge = distances.Min;   // всегда начинаем с вершины, до которой идти меньше всего.
            currentVertex = currentEdge.vertex; // когда в цикле доходим до этой вершины, это значит, что сейчас дошли до нее кратчайшим путем

            if (currentVertex.id == endVertex.id)
            {
                break;
            }

            if (visited[currentVertex.id] == true)
            {
                distances.Remove(currentEdge);  // это из-за возможных повторок на строчке 179
                continue;
            }

            visited[currentVertex.id] = true;
            
            foreach (var neighbour in currentVertex.neighbours)
            {
                if (visited[neighbour.vertex.id] == true)
                {
                    continue;
                }
                float neighbourDistance = currentEdge.cost + neighbour.cost;    // считаем как далеко идти от старта до этого соседа
                Edge savedEdge;
                if (distances.TryGetValue(neighbour, out savedEdge))    // если он уже сохранен в том сете, то проверяем, короче ли текущий путь, чем сохраненный
                {
                    if (savedEdge.cost > neighbourDistance)
                    {
                        savedEdge.vertex.prev = currentVertex;  // если да, то в соседе обновим, что пришли в него из текущей вершины
                    }
                }
                else
                {
                    neighbour.vertex.prev = currentVertex;  // если не сохранен, то просто так записваем текущую вершину как предыдущую
                }
                distances.Add(new Edge(neighbour.vertex, neighbourDistance));   // может добавить лишнее, но должен скипнуть из-за ифа вверху
            }

            distances.Remove(currentEdge);  // посмотрели соседей текущей вершины - удаляем
        }
        
        Vertex currentPathVertex = endVertex;   // как везде
        while (currentPathVertex != startVertex)
        {
            if(drawPath)
                Debug.DrawLine(currentPathVertex.transform.position, 
                    currentPathVertex.prev.transform.position, Color.yellow, 1000f);
            path.Add(currentPathVertex);
            currentPathVertex = currentPathVertex.prev;

        }
        return path;
    }
}
