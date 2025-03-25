using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject vertexPrefab;
    public Transform start;
    public int amountX; //количество вершин по осям
    public int amountZ;
    public float paddingX;  // отступ между ними
    public float paddingZ;
    public float maxNeighbourDistance = 8.0f;   // максимальное расстояние, при котором считаем вершину соседом. Больше - более густой граф. Минимум 3.0
    public LayerMask wallsLayer;
    public bool drawConnections;    // нарисует граф

    [HideInInspector]
    public Graph graph; // наш граф
    private List<Vertex> vertices;  // вершины
    
    private void Awake()
    {
        GenerateVertices();
        InitializeGraph();
    }
    
    public void GenerateVertices()
    {
        vertices = new List<Vertex>();
        Vector3 startPosition = start.position, currentPosition = startPosition;
        int currentVertexId = 0;
        for (int i = 0; i < amountX; i++)   // в двойном цикле создаем вершины
        {
            for (int j = 0; j < amountZ; j++)
            {
                Collider[] colliders = Physics.OverlapSphere(currentPosition, 0.5f, wallsLayer);    // проверяем, не появится ли вершина в стене
                if (colliders.Length == 0)
                {
                    GameObject newVertex = Instantiate(vertexPrefab, currentPosition, Quaternion.identity, transform);
                    Vertex vertexComponent = newVertex.GetComponent<Vertex>();  // настройка компонента, ставим ид
                    vertexComponent.id = currentVertexId;
                    currentVertexId++;
                    vertices.Add(vertexComponent);  // сразу сохраняем созданные вершины, чтобы потом не искать
                }
                currentPosition.z -= paddingZ;  // меняем позиции для следующих верщин
            }

            currentPosition.z = startPosition.z;
            currentPosition.x -= paddingX;
        }
    }
    
    private void InitializeGraph()
    {
        graph = GetComponent<Graph>();
        graph.vertices = vertices;
        graph.wallsLayer = wallsLayer;
        graph.neighbours = new List<List<Edge>>(vertices.Count);    // общий список соседей
        FindAllNeighbours();
    }

    public void FindAllNeighbours() // ищет соседей для каждой вершины в графе
    {
        foreach (var vertex in vertices)
        {
            vertex.neighbours = new List<Edge>();   // список соседей вершины
            FindNeighboursOf(vertex);

            graph.neighbours.Add(vertex.neighbours);
        }
    }

    public void FindNeighboursOf(Vertex vertex)
    {
        foreach (var other in vertices) // проверяем все вершины
        {
            if(vertex.id == other.id)
                continue;

            Vector3 position = vertex.transform.position;
            Vector3 otherPosition = other.transform.position;
            position.y += 0.5f;
            otherPosition.y += 0.5f;

            Vector3 direction = (otherPosition - position).normalized;
            float distance = (otherPosition - position).magnitude;

            if (distance > maxNeighbourDistance)    // если далеко, то скип
            {
                continue;
            }

            Ray ray = new Ray(position, direction);
            if (!Physics.Raycast(ray, distance, wallsLayer))    // если пуская луч в сторону другой вершины не попадаем в стену
            {
                vertex.neighbours.Add(new Edge(other, distance));   // то добавляем соседа
                if(drawConnections)
                    Debug.DrawLine(position, otherPosition, Color.red, 1000f);
            }

        }
    }
}
