using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject vertexPrefab;
    public List<Vertex> vertices;   // вершины
    public List<List<Edge>> neighbours; // их соседи
    
    [HideInInspector]
    public LayerMask wallsLayer;
    
    public int GetSize()
    {
        if (vertices == null)
            return 0;
        
        return vertices.Count;
    }

    public Vertex GetNearestVertex(Vector3 position)
    {
        Vertex nearestVertex = null;
        float nearestDistance = Mathf.Infinity;
        foreach (var vertex in vertices)    // просто перебираем и ищем самую близкую вершину
        {
            float distance = (vertex.transform.position - position).magnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestVertex = vertex;
            }
        }
        return nearestVertex;
    }

    public virtual Vertex GetVertexObj(int id)  // не знаю зач
    {
        if (vertices == null || vertices.Count == 0)
            return null;
        if (id < 0 || id >= vertices.Count)
            return null;
        return vertices[id];
    }

    public virtual Edge[] GetNeighbours(Vertex v)
    {
        if (neighbours == null || neighbours.Count == 0)
            return new Edge[0];
        if (v.id < 0 || v.id > neighbours.Count)
            return new Edge[0];
        return neighbours[v.id].ToArray();
    }
}
