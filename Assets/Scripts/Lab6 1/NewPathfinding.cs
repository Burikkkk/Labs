using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class NewPathfinding : MonoBehaviour
{
    public Transform endPoint;
    public NewGraphGenerator graphGenerator;
    public bool drawPath;

    private NewGraph graph;

    private void Start()
    {
        graph = graphGenerator.graph;
        FindPathDFSWeight();
    }

    public List<NewVertex> FindPathDFSWeight() // метод ищет путь с помощью поиска в глубину
    {
        // Находим ближайшую вершину к стартовой и конечной точке
        NewVertex startVertex = graph.GetNearestVertex(transform.position);
        NewVertex endVertex = graph.GetNearestVertex(endPoint.position);

        if (startVertex == null || endVertex == null)
        {
            Debug.LogError("Start or End vertex not found!");
            return new List<NewVertex>();
        }

        // Список для хранения пути
        List<NewVertex> path = new List<NewVertex>();

        // Массив для хранения посещенных вершин, чтобы не ходить по кругу
        bool[] visited = new bool[graph.vertices.Count];
        visited[startVertex.id] = true; // сразу помечаем стартовую вершину как посещенную

        // Стек для хранения вершин, которые нужно посетить (используется в DFS)
        Stack<NewVertex> toVisit = new Stack<NewVertex>();

        // Добавляем всех соседей стартовой вершины в стек
        foreach (var neighbour in startVertex.neighbours)
        {
            toVisit.Push(neighbour.vertex); // добавляем соседа в стек
            visited[neighbour.vertex.id] = true; // отмечаем вершину соседа как посещенную
            neighbour.vertex.prev = startVertex; // записываем, откуда пришли, чтобы потом восстановить путь
        }

        // Начинаем обход графа в глубину
        while (toVisit.Count > 0)
        {
            NewVertex currentVertex = toVisit.Pop(); // берем вершину из стека

            if (currentVertex == endVertex) // если дошли до конечной вершины, выходим из цикла
                break;

            // Перебираем всех соседей текущей вершины
            foreach (var nextNeighbour in currentVertex.neighbours)
            {
                if (visited[nextNeighbour.vertex.id]) // если уже посещали, пропускаем
                    continue;
                visited[nextNeighbour.vertex.id] = true; // отмечаем вершину соседа как посещенную
                toVisit.Push(nextNeighbour.vertex); // добавляем соседа в стек
                nextNeighbour.vertex.prev = currentVertex; // запоминаем, откуда пришли
            }
        }

        // Восстанавливаем путь от конечной вершины к стартовой
        NewVertex currentPathVertex = endVertex;
        while (currentPathVertex != startVertex)
        {
            if (drawPath) // если включена отрисовка пути, рисуем линию между вершинами в Unity
                Debug.DrawLine(currentPathVertex.transform.position,
                    currentPathVertex.prev.transform.position, Color.green, 1000f);

            path.Add(currentPathVertex); // добавляем вершину в путь
            currentPathVertex = currentPathVertex.prev; // идем к предыдущей вершине
        }

        path.Reverse(); // так как путь восстанавливался с конца, переворачиваем его

        // выводим id всех вершин пути в консоль
        foreach (var v in path)
        {
            Debug.Log(v.id);
        }

        return path; // возвращаем найденный путь
    }
}
