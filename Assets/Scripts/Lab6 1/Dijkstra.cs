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

    // получаем объект VertexInfo для заданной вершины
    private VertexInfo GetVertexInfo(NewVertex v)
    {
        return infos.Find(i => i.Vertex == v); // ищем в списке infos объект, соответствующий переданной вершине
    }

    // находим непосещённую вершину с минимальной суммой весов
    private VertexInfo FindUnvisitedVertexWithMinSum()
    {
        float minValue = float.MaxValue; // изначально минимальное значение устанавливаем как бесконечность
        VertexInfo minVertexInfo = null; // переменная для хранения вершины с наименьшим значением
        foreach (var i in infos) // проходим по всем вершинам
        {
            if (i.IsUnvisited && i.EdgesWeightSum < minValue) // если вершина не посещена и её сумма весов меньше текущего минимума
            {
                minVertexInfo = i; // обновляем минимальную вершину
                minValue = i.EdgesWeightSum; // обновляем минимальное значение суммы весов
            }
        }
        return minVertexInfo; // возвращаем найденную вершину или null, если таких больше нет
    }

    // метод поиска кратчайшего пути между двумя вершинами
    public List<NewVertex> FindShortestPath(NewVertex startVertex, NewVertex finishVertex)
    {
        InitInfo(); // инициализируем информацию о вершинах
        var first = GetVertexInfo(startVertex);
        first.EdgesWeightSum = 0; // устанавливаем начальную вершину с нулевой суммой весов

        while (true)
        {
            var current = FindUnvisitedVertexWithMinSum(); // выбираем ближайшую непосещённую вершину
            if (current == null) // если таких нет, завершаем алгоритм
            {
                break;
            }
            SetSumToNextVertex(current); // обновляем информацию о соседних вершинах
        }

        // получаем кратчайший путь из списка VertexInfo
        List<NewVertex> path = GetPath(startVertex, finishVertex);
        return path;
    }

    // обновляем суммы весов соседних вершин
    private void SetSumToNextVertex(VertexInfo info)
    {
        info.IsUnvisited = false; // отмечаем текущую вершину как посещённую
        foreach (var edge in info.Vertex.neighbours) // проходим по всем соседним рёбрам
        {
            var nextInfo = GetVertexInfo(edge.vertex); // получаем информацию о соседней вершине
            if (nextInfo != null && nextInfo.IsUnvisited) // проверяем, что она существует и не была посещена
            {
                float sum = info.EdgesWeightSum + edge.cost; // вычисляем новую сумму весов
                if (sum < nextInfo.EdgesWeightSum) // если найден более короткий путь, обновляем информацию
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex; // запоминаем предыдущую вершину для восстановления пути
                }
            }
        }
    }

    // восстанавливаем кратчайший путь, двигаясь от конечной вершины к начальной
    private List<NewVertex> GetPath(NewVertex startVertex, NewVertex endVertex)
    {
        List<NewVertex> path = new List<NewVertex>(); // создаём список для хранения пути
        var current = endVertex; // начинаем с конечной вершины
        while (current != null)
        {
            path.Insert(0, current); // добавляем вершину в начало списка
            current = GetVertexInfo(current).PreviousVertex; // переходим к предыдущей вершине в пути
            if (current == startVertex) // если достигли начальной вершины, добавляем её и завершаем
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

