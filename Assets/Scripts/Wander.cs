using UnityEngine;

public class Wander : MonoBehaviour
{

    public Vector3 startPosition;
    public float speed = 2f;
    public float wanderRadius = 5f;

    private Vector3 targetPosition;

    private void Start()
    {
        SetNewPosition();
    }

    private void Update()
    {
        // ������������ ������� � ������� ������� ������� � �������� ���������
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //����� ��������� ������� ��� ���������� ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewPosition();
        }
    }

    private void SetNewPosition()
    {
        //��������� ���������� � �������� ������� ������ startPosition
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, wanderRadius);
        float randomX = startPosition.x + Mathf.Cos(angle) * distance;
        float randomZ = startPosition.z + Mathf.Sin(angle) * distance;


        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    public void ResetWander()
    {
        SetNewPosition();
    }
}
