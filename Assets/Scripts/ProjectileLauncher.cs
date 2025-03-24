using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float launchSpeed = 10f;
    public GameObject target;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire(target.transform.position);
    }

    public void Fire(Vector3 targetPosition)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector3 direction = Projectile.GetFireDirection(firePoint.position, targetPosition, launchSpeed);
            projectile.Set(firePoint.position, direction, launchSpeed);
        }
    }
}
