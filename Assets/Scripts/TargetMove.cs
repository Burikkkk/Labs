using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // ����������� ��������
    public float speed = 5f; // �������� ��������

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}