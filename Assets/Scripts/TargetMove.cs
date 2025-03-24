using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Направление движения
    public float speed = 5f; // Скорость движения

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}