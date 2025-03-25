using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph1 : MonoBehaviour
{
    public GameObject vertexPrefab; // префаб вершины, который будем клонировать
    public List<Vertex> vertices;   // список вершин графа
    public List<List<Edge>> neighbours; // список списков соседей для каждой вершины

    [HideInInspector]
    public LayerMask wallsLayer; // слой, на котором находятся стены (чтобы не учитывать их в связях)

    public int GetSize()
    {
        if (vertices == null) // если вершины не созданы, возвращаем 0
            return 0;

        return vertices.Count; // возвращаем количество вершин
    }

    public Vertex GetNearestVertex(Vector3 position)
    {
        Vertex nearestVertex = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var vertex in vertices)    // перебираем все вершины и ищем ближайшую
        {
            float distance = (vertex.transform.position - position).magnitude; // считаем расстояние
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestVertex = vertex; // обновляем ближайшую вершину
            }
        }
        return nearestVertex; // возвращаем ближайшую найденную вершину
    }

    public virtual Vertex GetVertexObj(int id)  // получает вершину по id
    {
        if (vertices == null || vertices.Count == 0) // если список вершин пуст, возвращаем null
            return null;
        if (id < 0 || id >= vertices.Count) // проверяем, что id находится в допустимых границах
            return null;
        return vertices[id]; // возвращаем вершину с нужным id
    }

    public virtual Edge[] GetNeighbours(Vertex v) // возвращает список соседей для заданной вершины
    {
        if (neighbours == null || neighbours.Count == 0) // если список соседей пуст, возвращаем пустой массив
            return new Edge[0];
        if (v.id < 0 || v.id >= neighbours.Count) // проверяем, что id вершины корректен
            return new Edge[0];
        return neighbours[v.id].ToArray(); // возвращаем массив соседей данной вершины
    }
}
