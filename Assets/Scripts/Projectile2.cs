using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    private Vector3 direction; 
    private float speed;       

    public void SetDirection(Vector3 newDirection, float newSpeed)
    {
        direction = newDirection;
        speed = newSpeed;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
    }
}