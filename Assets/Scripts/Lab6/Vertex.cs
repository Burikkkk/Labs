using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public int id;
    public List<Edge> neighbours;
    [HideInInspector] public Vertex prev;
}
