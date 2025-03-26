using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VertexInfo
{
    public NewVertex Vertex { get; set; }
    public bool IsUnvisited { get; set; }
    public float EdgesWeightSum { get; set; }
    public NewVertex PreviousVertex { get; set; }

    public VertexInfo(NewVertex vertex)
    {
        Vertex = vertex;
        IsUnvisited = true;
        EdgesWeightSum = float.MaxValue;
        PreviousVertex = null;
    }
}
