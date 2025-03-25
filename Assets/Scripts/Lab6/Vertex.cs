using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public int id;
    public List<Edge> neighbours;   // список соседей вершины
    [HideInInspector] public Vertex prev;   // нужно для алгоритмов
}
