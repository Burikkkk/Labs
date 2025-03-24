using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1.0f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    public int CompareTo(Edge other)
    {
        float result = cost - other.cost;
        if (vertex.GetInstanceID() == other.vertex.GetInstanceID())
            return 0;
        return (int)result;
    }

    public bool Equals(Edge other)
    {
        return other.vertex.id == vertex.id;
    }
    
    public override bool Equals(object obj)
    {
        Edge other = (Edge)obj;
        return other.vertex.id == vertex.id;
    }

    public override int GetHashCode()
    {
        return vertex.GetHashCode();
    }
    
}
