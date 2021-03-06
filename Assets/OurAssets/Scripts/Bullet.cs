using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 15f;
    [SerializeField] Vector3 shootPoint;
    [SerializeField] float shootRadius;
    [SerializeField] float upSpeed = 5f;
    [SerializeField] float maxDistance;

    [SerializeField] private ParticleSystem explosion;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterSeconds(10f));
    }

    public void SetupBullet (Vector3 shootP, float shootR)
    {
        shootPoint = shootP;
        shootRadius = shootR;
        transform.LookAt(shootPoint);
        maxDistance = Vector3.SqrMagnitude(shootPoint - transform.position);
    }

    void Update()
    {
        if (Vector3.MoveTowards(transform.position, shootPoint, speed * Time.deltaTime) == transform.position)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameManager.Instance.OnPlayerBulletExplosion();
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

            StopAllCoroutines();
            gameObject.SetActive(false);
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

    private IEnumerator DeactivateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shootPoint, shootRadius);
        Gizmos.DrawLine(transform.position, shootPoint);
    }

}
