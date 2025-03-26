using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VertexInfo
{
    public NewVertex Vertex { get; set; }
    public bool IsUnvisited { get; set; }
    public float EdgesWeightSum { get; set; } // сумма весов рёбер от стартовой вершины до данной
    public NewVertex PreviousVertex { get; set; } // предыдущая вершина в кратчайшем пути


    public VertexInfo(NewVertex vertex)
    {
        Vertex = vertex;
        IsUnvisited = true;
        EdgesWeightSum = float.MaxValue; // устанавливаем максимальное значение суммы весов рёбер
        PreviousVertex = null;
    }
}
