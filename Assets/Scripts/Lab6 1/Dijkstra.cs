using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    private NewGraph graph;
    private List<VertexInfo> infos;

    public Transform endPoint;
    public NewGraphGenerator graphGenerator;
    public bool drawPath;

    private void Start()
    {
        
        graph = graphGenerator.graph;
   
        NewVertex startVertex = graph.GetNearestVertex(transform.position);
        NewVertex endVertex = graph.GetNearestVertex(endPoint.position);
        if (startVertex == null || endVertex == null)
        {
            Debug.LogError("Одна из вершин (startVertex или endVertex) равна null!");
            return;
        }
        List<NewVertex> path = FindShortestPath(startVertex, endVertex);
        DrawPath(path);
    }



    private void InitInfo()
    {
        infos = new List<VertexInfo>();
        foreach (var v in graph.vertices)
        {
            infos.Add(new VertexInfo(v));
        }
    }

    private VertexInfo GetVertexInfo(NewVertex v)
    {
        return infos.Find(i => i.Vertex == v);
    }

    private VertexInfo FindUnvisitedVertexWithMinSum()
    {
        float minValue = float.MaxValue;
        VertexInfo minVertexInfo = null;
        foreach (var i in infos)
        {
            if (i.IsUnvisited && i.EdgesWeightSum < minValue)
            {
                minVertexInfo = i;
                minValue = i.EdgesWeightSum;
            }
        }
        return minVertexInfo;
    }

    public List<NewVertex> FindShortestPath(NewVertex startVertex, NewVertex finishVertex)
    {
        InitInfo();
        var first = GetVertexInfo(startVertex);
        first.EdgesWeightSum = 0;

        while (true)
        {
            var current = FindUnvisitedVertexWithMinSum();
            if (current == null)
            {
                break;
            }
            SetSumToNextVertex(current);
        }
        List<NewVertex> path = GetPath(startVertex, finishVertex);
        DrawPath(path); // Вызов метода рисования пути
        return path;
    }

    private void SetSumToNextVertex(VertexInfo info)
    {
        info.IsUnvisited = false;
        foreach (var edge in info.Vertex.neighbours)
        {
            var nextInfo = GetVertexInfo(edge.vertex);
            if (nextInfo != null && nextInfo.IsUnvisited)
            {
                float sum = info.EdgesWeightSum + edge.cost;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }
    }

    private List<NewVertex> GetPath(NewVertex startVertex, NewVertex endVertex)
    {
        List<NewVertex> path = new List<NewVertex>();
        var current = endVertex;
        while (current != null)
        {
            path.Insert(0, current);
            current = GetVertexInfo(current).PreviousVertex;
            if (current == startVertex)
            {
                path.Insert(0, startVertex);
                break;
            }
        }
        return path;
    }

    private void DrawPath(List<NewVertex> path)
    {
        if (path.Count < 2) return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].transform.position, path[i + 1].transform.position, Color.green, 1000f);
        }
    }
}

