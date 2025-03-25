using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGraph : MonoBehaviour
{
   
    public List<NewVertex> vertices;   // список вершин графа
    public List<List<NewEdge>> neighbours; // список списков соседей для каждой вершины

    [HideInInspector]
    public LayerMask wallsLayer;

    public int GetSize()
    {
        if (vertices == null) // если вершины не созданы, возвращаем 0
            return 0;

        return vertices.Count; // возвращаем количество вершин
    }

    public NewVertex GetNearestVertex(Vector3 position)
    {
        NewVertex nearestVertex = null;
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

    public virtual NewVertex GetVertexObj(int id)  // получает вершину по id
    {
        if (vertices == null || vertices.Count == 0) // если список вершин пуст, возвращаем null
            return null;
        if (id < 0 || id >= vertices.Count) // проверяем, что id находится в допустимых границах
            return null;
        return vertices[id]; // возвращаем вершину с нужным id
    }

    public virtual NewEdge[] GetNeighbours(NewVertex v) // возвращает список соседей для заданной вершины
    {
        if (neighbours == null || neighbours.Count == 0) // если список соседей пуст, возвращаем пустой массив
            return new NewEdge[0];
        if (v.id < 0 || v.id >= neighbours.Count) // проверяем, что id вершины корректен
            return new NewEdge[0];
        return neighbours[v.id].ToArray(); // возвращаем массив соседей данной вершины
    }
}
