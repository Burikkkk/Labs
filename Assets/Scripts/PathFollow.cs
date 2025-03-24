using UnityEngine;
using System.Collections.Generic;  // Нужно для использования List<Transform>

public class FollowPath : MonoBehaviour
{
    public List<Transform> waypoints; // Используем List<Transform> вместо массива
    public float speed = 2f;
    public float reachDistance = 0.5f;
    public Transform[] waypoints1;

    private int currentIndex = 0;

    private void Update()
    {
        ClosestPoint();
    }

    private void ClosestPoint()
    {
        Transform closestWaypoint = GetClosestWaypoint();

        if (closestWaypoint != null)
        {
            // Движение объекта к ближайшей точке
            transform.position = Vector3.MoveTowards(
                transform.position,
                closestWaypoint.position,
                speed * Time.deltaTime
            );

            // Если объект приблизился к ближайшей точке
            if (Vector3.Distance(transform.position, closestWaypoint.position) < reachDistance)
            {
                // Удаление ближайшей точки из списка
                waypoints.Remove(closestWaypoint);

                // Если список точек пуст, можно завершить движение или сделать что-то еще
                if (waypoints.Count == 0)
                {
                    Debug.Log("Все точки пройдены!");
                }
            }
        }
    }

    private Transform GetClosestWaypoint()
    {
        Transform closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);
            if (distance < closestDistance)
            {
                closestWaypoint = waypoint;
                closestDistance = distance;
            }
        }

        return closestWaypoint;
    }


    private void NextPoint()
    {
        if (currentIndex < waypoints1.Length)
        {
            //текущая целевая точка
            Transform targetWaypoint = waypoints1[currentIndex];

            //движение объекта
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWaypoint.position,
                speed * Time.deltaTime   //расстояние, пройденное за кадр
            );

            //если объект приблизился к целевой точке
            if (Vector3.Distance(transform.position, targetWaypoint.position) < reachDistance)
            {
                currentIndex++;

                if (currentIndex >= waypoints1.Length)
                {
                    currentIndex = 0;
                }
            }
        }
    }

}
