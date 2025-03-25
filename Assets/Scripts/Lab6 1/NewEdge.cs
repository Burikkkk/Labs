using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEdge : MonoBehaviour, IComparable<NewEdge> // класс, представляющий ребро графа
{
    public float cost;  // расстояние до соседней вершины
    public NewVertex vertex;   // ссылка на соседнюю вершину

    public NewEdge(NewVertex vertex = null, float cost = 1.0f) // конструктор, создающий ребро
    {
        this.vertex = vertex; // устанавливаем ссылку на вершину
        this.cost = cost; // устанавливаем стоимость перехода
    }

    public int CompareTo(NewEdge other)    // нужен для алгоритма дейкстры, сравнивает два ребра по стоимости
    {
        float result = cost - other.cost; // вычисляем разницу в стоимости
        if (vertex.GetInstanceID() == other.vertex.GetInstanceID()) // если вершины одинаковые, возвращаем 0
        {
            return 0;
        }
        if ((int)result == 0)   // если стоимость одинаковая, но вершины разные, возвращаем -1 (можно 1)
            return -1;
        return (int)result; // иначе возвращаем разницу в стоимости как целое число
    }

    public bool Equals(NewEdge other)  // проверяет, ведут ли два ребра в одну и ту же вершину
    {
        return other.vertex.id == vertex.id; // сравниваем id вершин
    }

    public override bool Equals(object obj) // перегрузка Equals для работы с object
    {
        NewEdge other = (NewEdge)obj;
        return other.vertex.id == vertex.id; // сравниваем по id вершины
    }

    public override int GetHashCode() // переопределяем метод GetHashCode
    {
        return vertex.GetHashCode(); // используем хэш код вершины
    }
}
