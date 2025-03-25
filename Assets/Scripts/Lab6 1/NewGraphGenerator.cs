using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NewGraphGenerator : MonoBehaviour
{
    public List<NewVertex> vertices; // Массив вершин, передаваемый из Unity
    public bool drawConnections; // Нужно ли визуализировать связи
    public LayerMask wallsLayer;
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

        FindAllNeighbours(); // записываем соседей

    }
    public void FindAllNeighbours() // записывает соседей для каждой вершины в графе
    {
        foreach (var vertex in vertices)
        {
            vertex.neighbours = new List<NewEdge>();   // создаем пустой список соседей для вершины
            FindNeighboursOf(vertex); // записывает соседей данной вершины
            graph.neighbours.Add(vertex.neighbours); // добавляем список соседей в граф
        }
    }

    private void FindNeighboursOf(NewVertex vertex)
    {
        GameObject vertexObject = vertex.gameObject;
        NewEdge[] edges = vertexObject.GetComponents<NewEdge>();
        foreach (var edge in edges) // записываем все ребра вершины
        {
            vertex.neighbours.Add(edge); // добавляем вершину в список соседей
        }
    }
    
    private void OnDrawGizmos()
    {
        if(!drawConnections)
            return;
        
        foreach (var vertex in vertices)
        {
            foreach (var edge in vertex.neighbours)
            {
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(edge.cost * 3, edge.transform.position, edge.vertex.transform.position);
            }
        }
    }
}
