using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject vertexPrefab;
    public List<Vertex> vertices;
    public List<List<Edge>> neighbours;

    public void Start()
    {
        
    }

    public virtual void Load()
    {
        
    }

    public virtual int GetSize()
    {
        if (vertices == null)
            return 0;
        
        return vertices.Count;
    }

    public virtual Vertex GetNearestVertex(Vector3 position)
    {
        return null;
    }

    public virtual Vertex GetVertexObj(int id)
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
