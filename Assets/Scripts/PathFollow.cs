using UnityEngine;
using System.Collections.Generic;  // ����� ��� ������������� List<Transform>

public class FollowPath : MonoBehaviour
{
    public List<Transform> waypoints; // ���������� List<Transform> ������ �������
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
            // �������� ������� � ��������� �����
            transform.position = Vector3.MoveTowards(
                transform.position,
                closestWaypoint.position,
                speed * Time.deltaTime
            );

            // ���� ������ ����������� � ��������� �����
            if (Vector3.Distance(transform.position, closestWaypoint.position) < reachDistance)
            {
                // �������� ��������� ����� �� ������
                waypoints.Remove(closestWaypoint);

                // ���� ������ ����� ����, ����� ��������� �������� ��� ������� ���-�� ���
                if (waypoints.Count == 0)
                {
                    Debug.Log("��� ����� ��������!");
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
            //������� ������� �����
            Transform targetWaypoint = waypoints1[currentIndex];

            //�������� �������
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWaypoint.position,
                speed * Time.deltaTime   //����������, ���������� �� ����
            );

            //���� ������ ����������� � ������� �����
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
