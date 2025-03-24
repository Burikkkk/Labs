using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject vertexPrefab;
    public Transform start;
    public int amountX;
    public int amountZ;
    public float paddingX;
    public float paddingZ;
    public float maxNeighbourDistance = 8.0f;
    public LayerMask wallsLayer;
    public bool drawConnections;

    private Graph graph;
    private List<Vertex> vertices;
    
    private void Awake()
    {
        GenerateVertices();
        InitializeGraph();
    }

    private void InitializeGraph()
    {
        graph = GetComponent<Graph>();
        graph.vertices = vertices;
        graph.neighbours = new List<List<Edge>>(vertices.Count);
        FindAllNeighbours();
    }

    public void GenerateVertices()
    {
        vertices = new List<Vertex>();
        Vector3 startPosition = start.position, currentPosition = startPosition;
        int currentVertexId = 0;
        for (int i = 0; i < amountX; i++)
        {
            for (int j = 0; j < amountZ; j++)
            {
                Collider[] colliders = Physics.OverlapSphere(currentPosition, 0.5f, wallsLayer);
                if (colliders.Length == 0)
                {
                    GameObject newVertex = Instantiate(vertexPrefab, currentPosition, Quaternion.identity, transform);
                    Vertex vertexComponent = newVertex.GetComponent<Vertex>();
                    vertexComponent.id = currentVertexId;
                    currentVertexId++;
                    vertices.Add(vertexComponent);
                }
                currentPosition.z -= paddingZ;
            }

            currentPosition.z = startPosition.z;
            currentPosition.x -= paddingX;
        }
    }

    public void FindAllNeighbours()
    {
        foreach (var vertex in vertices)
        {
            vertex.neighbours = new List<Edge>();
            FindNeighboursOf(vertex);

            graph.neighbours.Add(vertex.neighbours);
        }
    }

    public void FindNeighboursOf(Vertex vertex)
    {
        foreach (var other in vertices)
        {
            if(vertex.id == other.id)
                continue;

            Vector3 position = vertex.transform.position;
            Vector3 otherPosition = other.transform.position;
            position.y += 0.5f;
            otherPosition.y += 0.5f;

            Vector3 direction = (otherPosition - position).normalized;
            float distance = (otherPosition - position).magnitude;

            if (distance > maxNeighbourDistance)
            {
                continue;
            }

            Ray ray = new Ray(position, direction);
            if (!Physics.Raycast(ray, distance, wallsLayer))    // не попал в стену
            {
                vertex.neighbours.Add(new Edge(other, distance));
                if(drawConnections)
                    Debug.DrawLine(position, otherPosition, Color.red, 1000f);
            }

        }
    }
}
