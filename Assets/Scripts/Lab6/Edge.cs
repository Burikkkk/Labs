using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : IComparable<Edge>
{
    public float cost;  // расстояние до соседа
    public Vertex vertex;   // сосед

    public Edge(Vertex vertex = null, float cost = 1.0f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    public int CompareTo(Edge other)    // нужно только в алгоритме дейкстры
    {
        float result = cost - other.cost;
        if (vertex.GetInstanceID() == other.vertex.GetInstanceID()) 
        {
            return 0;
        }
        if ((int)result == 0)   // если ребро идет в другого соседа, то не должно быть равно нашему (можно ретурн -1 или 1)
            return -1;
        return (int)result;
    }

    public bool Equals(Edge other)  // эти не знаю зачем
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
