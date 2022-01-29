using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 shootPoint;
    float shootRadius;
    float speed = 15f;
    float upSpeed = 5f;
    float maxDistance;

    [SerializeField] private ParticleSystem explosion;

    public void setupBullet (Vector3 shootP, float shootR)
    {
        shootPoint = shootP;
        shootRadius = shootR;
        transform.LookAt(shootPoint);
        maxDistance = Vector3.SqrMagnitude(shootPoint - transform.position);
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
                        // Debug.Log("HIT");
                        enemy.GetComponent<Enemy>().Damage();
                    }
                }
            }
            Destroy(gameObject);
        } else
        {
            if (Vector3.SqrMagnitude(shootPoint - transform.position) < maxDistance/2 * maxDistance/2)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (upSpeed * Time.deltaTime), transform.position.z);
            } else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (upSpeed * Time.deltaTime), transform.position.z);
            }
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
