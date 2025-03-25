using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVertex : MonoBehaviour
{
    public int id;
    public List<NewEdge> neighbours;   // список соседей вершины
    [HideInInspector] public NewVertex prev;   // нужно для алгоритмов
}
