using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool set = false; // ����, �����������, ��� �� ���������� ������
    private Vector3 firePos; // ��������� ������� ��������
    private Vector3 direction; // ����������� �������� �������
    private float speed; // �������� �������
    private float timeElapsed; // �����, ��������� � ������� ��������

    void Update()
    {
        if (!set) return;

        timeElapsed += Time.deltaTime; // ��������� ����� ������

        // ������������ ����� ������� � ������ ���������� �������� � ����������
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;

        // ���� ������ ���� ���� ������������ ������, ���������� ���
        if (transform.position.y < -1.0f)
            Destroy(gameObject);
    }

    // ����� ��������� ���������� ������ �������
    public void Set(Vector3 firePos, Vector3 direction, float speed)
    {
        this.firePos = firePos; 
        this.direction = direction.normalized; 
        this.speed = speed; 
        transform.position = firePos; 
        set = true;
    }

    // ����� ��� ���������� ������� ������� ������� �� �������� ������
    public float GetLandingTime(float height = 0.0f)
    {
        Vector3 position = transform.position;

        // ��������� ������������ ����������� ��������� ��� ������� �������
        float valueInt = (direction.y * direction.y) * (speed * speed) - (Physics.gravity.y * 2 * (position.y - height));
        if (valueInt < 0) return -1.0f; // ���� ������������ ������ 0, ������� ���

        float sqrtValue = Mathf.Sqrt(valueInt);

        // ������� ��� ��������� ������� �������
        float valueAdd = (-direction.y * speed + sqrtValue) / Physics.gravity.y;
        float valueSub = (-direction.y * speed - sqrtValue) / Physics.gravity.y;

        // �������� ���������� �������� �������
        if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub)) return valueSub;
        if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub)) return valueAdd;
        if (float.IsNaN(valueAdd) && float.IsNaN(valueSub)) return -1.0f;

        return Mathf.Max(valueAdd, valueSub);
    }

    // ����� ���������� ����� ������� �������
    public Vector3 GetLandingPos(float height = 0.0f)
    {
        float time = GetLandingTime(); // �������� ����� �������
        if (time < 0.0f) return Vector3.zero; // ���� ������� ����������, ���������� ������ (0,0,0)

        // ��������� ���������� ����� �����������
        return new Vector3(
            firePos.x + direction.x * speed * time,
            height,
            firePos.z + direction.z * speed * time
        );
    }

    // ����������� ����� ���������� ����������� �������� ��� ��������� � ����
    public static Vector3 GetFireDirection(Vector3 startPos, Vector3 endPos, float speed)
    {
        Vector3 direction = Vector3.zero; // ��������� �������� �����������
        Vector3 delta = endPos - startPos; // ������� ����� ������� � ��������� ��������

        // ������������ ����������� ��������� ��� ������� ������
        float a = Vector3.Dot(Physics.gravity, Physics.gravity);
        float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
        float c = 4 * Vector3.Dot(delta, delta);

        // ���������, ���� �� �������� �����
        if (4 * a * c > b * b) return direction; // ���� ��� �������, ���������� ������ (0,0,0)

        // ��������� ��� ��������� ������� ������
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));

        // �������� ����������� ������������� �����
        float time = time0 < 0 ? time1 : (time1 < 0 ? time0 : Mathf.Min(time0, time1));

        // ��������� ����������� ������
        direction = (2 * delta - Physics.gravity * (time * time)) / (2 * speed * time);
        return direction;
    }
}
