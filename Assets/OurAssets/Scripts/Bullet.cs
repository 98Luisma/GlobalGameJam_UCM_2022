using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 shootPoint;
    float shootRadius;
    float speed = 15f;

    [SerializeField] private ParticleSystem explosion;

    public void setupBullet (Vector3 shootP, float shootR)
    {
        shootPoint = shootP;
        shootRadius = shootR;
        transform.LookAt(shootPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.MoveTowards(transform.position, shootPoint, speed * Time.deltaTime) == transform.position)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Collider[] sphereCast;
            sphereCast = Physics.OverlapSphere(shootPoint, shootRadius);

            if (sphereCast.Length > 0)
            {
                foreach (Collider enemy in sphereCast)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        Destroy(enemy.gameObject);
                    }
                }
            }
            Destroy(gameObject);
        } else
        {
        transform.position = Vector3.MoveTowards(transform.position, shootPoint, speed * Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shootPoint, shootRadius);
        Gizmos.DrawLine(transform.position, shootPoint);
    }

}
