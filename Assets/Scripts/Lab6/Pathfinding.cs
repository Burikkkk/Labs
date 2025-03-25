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

    public List<Vertex> FindPathDFS()   // метод ищет путь с помощью поиска в глубину (dfs), но не обязательно кратчайший
    {
        // находим ближайшую вершину к стартовой и конечной точке
        Vertex startVertex = graph.GetNearestVertex(transform.position);
        Vertex endVertex = graph.GetNearestVertex(endPoint.position);

        // список для хранения пути
        List<Vertex> path = new List<Vertex>();

        // массив для хранения посещенных вершин, чтобы не ходить по кругу
        bool[] visited = new bool[graph.vertices.Count];
        visited[startVertex.id] = true; // сразу помечаем стартовую вершину как посещенную

        // стек для хранения вершин, которые нужно посетить (стек используется в dfs)
        Stack<Vertex> toVisit = new Stack<Vertex>();

        // добавляем всех соседей стартовой вершины в стек
        foreach (var neighbour in startVertex.neighbours)
        {
            toVisit.Push(neighbour.vertex); // добавляем соседа в стек
            neighbour.vertex.prev = startVertex; // записываем, откуда пришли, чтобы потом восстановить путь
        }

        // начинаем обход графа в глубину
        while (toVisit.Count != 0)
        {
            Vertex currentVertex = toVisit.Pop();   // берем вершину из стека

            if (currentVertex == endVertex) // если дошли до конечной вершины, выходим из цикла
                break;

            // перебираем всех соседей текущей вершины
            foreach (var nextNeighbour in currentVertex.neighbours)
            {
                if (visited[nextNeighbour.vertex.id] == true) // если уже посещали, пропускаем
                    continue;

                toVisit.Push(nextNeighbour.vertex); // добавляем соседа в стек
                nextNeighbour.vertex.prev = currentVertex; // запоминаем, откуда пришли
            }

            visited[currentVertex.id] = true; // отмечаем текущую вершину как посещенную
        }

        // начинаем восстановление пути от конечной вершины к стартовой
        Vertex currentPathVertex = endVertex;
        while (currentPathVertex != startVertex)
        {
            if (drawPath) // если включена отрисовка пути, рисуем линию между вершинами в unity
                Debug.DrawLine(currentPathVertex.transform.position,
                    currentPathVertex.prev.transform.position, Color.green, 1000f);

            path.Add(currentPathVertex); // добавляем вершину в путь
            currentPathVertex = currentPathVertex.prev; // идем к предыдущей вершине
        }

        path.Reverse(); // так как путь восстанавливался с конца, переворачиваем его

        // выводим id всех вершин пути в консоль
        //foreach (var v in path)
        //{
        //    Debug.Log(v.id);
        //}

        return path; // возвращаем найденный путь
    }

    public List<Vertex> FindPathBFS()   // находит путь, кратчайший только если все расстояния между вершинами одинаковые
    {
        // находим ближайшие вершины к стартовой и конечной точке
        Vertex startVertex = graph.GetNearestVertex(transform.position);
        Vertex endVertex = graph.GetNearestVertex(endPoint.position);

        // создаем список для хранения пути
        List<Vertex> path = new List<Vertex>();

        // массив для хранения посещенных вершин
        bool[] visited = new bool[graph.vertices.Count];
        visited[startVertex.id] = true; // сразу помечаем стартовую вершину как посещенную

        // создаем очередь для BFS (поиск в ширину)
        Queue<Vertex> toVisit = new Queue<Vertex>();

        // добавляем соседей стартовой вершины в очередь
        foreach (var neighbour in startVertex.neighbours)
        {
            toVisit.Enqueue(neighbour.vertex);
            neighbour.vertex.prev = startVertex; // запоминаем, откуда пришли
        }

        // запускаем обход графа в ширину
        while (toVisit.Count != 0)
        {
            Vertex currentVertex = toVisit.Dequeue(); // берем вершину из очереди

            if (currentVertex == endVertex) // если дошли до конечной вершины, выходим из цикла
                break;

            // добавляем соседей текущей вершины в очередь
            foreach (var nextNeighbour in currentVertex.neighbours)
            {
                if (visited[nextNeighbour.vertex.id] == true) // если уже посещена, пропускаем
                    continue;

                toVisit.Enqueue(nextNeighbour.vertex); // добавляем в очередь
                nextNeighbour.vertex.prev = currentVertex; // запоминаем, откуда пришли
            }
            visited[currentVertex.id] = true; // отмечаем вершину как посещенную
        }

        // начинаем восстановление пути от конечной вершины к стартовой
        Vertex currentPathVertex = endVertex;
        while (currentPathVertex != startVertex)
        {
            if (drawPath) // если включена отрисовка пути, рисуем линию между вершинами в unity
                Debug.DrawLine(currentPathVertex.transform.position,
                    currentPathVertex.prev.transform.position, Color.blue, 1000f);

            path.Add(currentPathVertex); // добавляем вершину в путь
            currentPathVertex = currentPathVertex.prev; // идем к предыдущей вершине
        }

        path.Reverse(); // разворачиваем путь, чтобы он шел от начала до конца

        return path; // возвращаем найденный путь
    }
}
