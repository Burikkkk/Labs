using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject vertexPrefab; // префаб вершины, который будем клонировать
    public Transform start; // начальная точка для генерации вершин
    public int amountX; // количество вершин по оси X
    public int amountZ; // количество вершин по оси Z
    public float paddingX;  // отступ между вершинами по X
    public float paddingZ; // отступ между вершинами по Z
    public float maxNeighbourDistance = 8.0f;   // максимальное расстояние, при котором вершины считаются соседями
    public LayerMask wallsLayer; // слой, на котором находятся стены (чтобы не создавать вершины внутри препятствий)
    public bool drawConnections;    // если true, будет рисоваться соединения между вершинами для визуализации графа

    [HideInInspector]
    public Graph graph; // сам граф, содержащий вершины и рёбра
    private List<Vertex> vertices;  // список вершин графа

    private void Awake()
    {
        GenerateVertices(); // создаем вершины
        InitializeGraph(); // инициализируем граф, находя соседей для вершин
    }

    public void GenerateVertices()
    {
        vertices = new List<Vertex>(); // создаем пустой список вершин
        Vector3 startPosition = start.position, currentPosition = startPosition;
        int currentVertexId = 0; // счетчик id вершин
        for (int i = 0; i < amountX; i++)   // двойной цикл для создания сетки вершин
        {
            for (int j = 0; j < amountZ; j++)
            {
                // проверяем, нет ли препятствия в месте, где хотим создать вершину
                Collider[] colliders = Physics.OverlapSphere(currentPosition, 0.5f, wallsLayer);
                if (colliders.Length == 0) // если нет коллизий, создаем вершину
                {
                    GameObject newVertex = Instantiate(vertexPrefab, currentPosition, Quaternion.identity, transform);
                    Vertex vertexComponent = newVertex.GetComponent<Vertex>();  // получаем компонент вершины
                    vertexComponent.id = currentVertexId; // присваиваем id
                    currentVertexId++;
                    vertices.Add(vertexComponent);  // добавляем вершину в список
                }
                currentPosition.z -= paddingZ;  // сдвигаем позицию для следующей вершины по оси Z
            }
            currentPosition.z = startPosition.z; // возвращаем Z к начальной позиции
            currentPosition.x -= paddingX; // сдвигаем по оси X
        }
    }

    private void InitializeGraph()
    {
        graph = GetComponent<Graph>(); // получаем компонент графа
        graph.vertices = vertices; // передаем список вершин в граф
        graph.wallsLayer = wallsLayer; // передаем слой стен
        graph.neighbours = new List<List<Edge>>(vertices.Count); // создаем список списков соседей для каждой вершины
        FindAllNeighbours(); // ищем соседей для всех вершин
    }

    public void FindAllNeighbours() // ищет соседей для каждой вершины в графе
    {
        foreach (var vertex in vertices)
        {
            vertex.neighbours = new List<Edge>();   // создаем пустой список соседей для вершины
            FindNeighboursOf(vertex); // находим соседей для данной вершины
            graph.neighbours.Add(vertex.neighbours); // добавляем список соседей в граф
        }
    }

    public void FindNeighboursOf(Vertex vertex) // определяет соседей для конкретной вершины
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
                vertex.neighbours.Add(new Edge(other, distance));   // добавляем вершину в список соседей
                if (drawConnections) // если нужно отрисовать соединения, рисуем линию
                    Debug.DrawLine(position, otherPosition, Color.red, 1000f);
            }
        }
    }
}