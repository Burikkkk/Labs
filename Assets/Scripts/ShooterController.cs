using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform target;           
    public Transform firePoint;        
    public float projectileSpeed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
      
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile2 projectileScript = projectile.GetComponent<Projectile2>();

        // ������������ ����������� � ����
        Vector3 direction = (target.position - firePoint.position).normalized;

        // ������������� ��������� �������
        projectileScript.SetDirection(direction, projectileSpeed);
    }
}