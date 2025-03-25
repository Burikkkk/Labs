using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewGraphGenerator : MonoBehaviour
{
    public List<NewVertex> vertices; // Массив вершин, передаваемый из Unity
    public bool drawConnections; // Нужно ли визуализировать связи
    public float maxNeighbourDistance = 8.0f;
    public LayerMask wallsLayer;
    public Transform start;
    [HideInInspector]
    public NewGraph graph; // Граф, содержащий вершины и рёбра

    private void Awake()
    {
        InitializeGraph(); // Инициализация графа с учетом переданных вершин
    }

    private void InitializeGraph()
    {
        graph = GetComponent<NewGraph>(); // Создаем компонент через AddComponent
        graph.vertices = new List<NewVertex>(vertices); // Загружаем вершины из массива
        graph.wallsLayer = wallsLayer;
        graph.neighbours = new List<List<NewEdge>>(vertices.Count); // Создаем списки соседей

        FindAllNeighbours(); // Определяем соседей

    }
    public void FindAllNeighbours() // ищет соседей для каждой вершины в графе
    {
        foreach (var vertex in vertices)
        {
            vertex.neighbours = new List<NewEdge>();   // создаем пустой список соседей для вершины
            FindNeighboursOf(vertex); // находим соседей для данной вершины
            graph.neighbours.Add(vertex.neighbours); // добавляем список соседей в граф
        }
    }

    private void FindNeighboursOf(NewVertex vertex)
    {
        foreach (var other in vertices) // проверяем все вершины в графе
        {
            if (vertex.id == other.id) // если вершина проверяет саму себя, пропускаем
                continue;

            Vector3 position = vertex.transform.position;
            Vector3 otherPosition = other.transform.position;
            position.y += 0.5f; // немного поднимаем точку, чтобы избежать проблем с лучами
            otherPosition.y += 0.5f;

            Vector3 direction = (otherPosition - position).normalized; // направление к другой вершине
            float distance = (otherPosition - position).magnitude; // расстояние до нее

            if (distance > maxNeighbourDistance)    // если вершина слишком далеко, пропускаем
            {
                continue;
            }

            Ray ray = new Ray(position, direction); // создаем луч от одной вершины к другой
            if (!Physics.Raycast(ray, distance, wallsLayer))    // если луч не пересекает стену
            {
                vertex.neighbours.Add(new NewEdge(other, distance));   // добавляем вершину в список соседей
                if (drawConnections) // если нужно отрисовать соединения, рисуем линию
                    Debug.DrawLine(position, otherPosition, Color.red, 1000f);
            }
        }
    }
}
