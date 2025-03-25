using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex1 : MonoBehaviour
{
    public int id;
    public List<Edge> neighbours;   // список соседей вершины
    [HideInInspector] public Vertex prev;   // нужно для алгоритмов
}
