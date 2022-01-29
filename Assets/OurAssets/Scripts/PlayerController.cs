using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed = 10f;
    public Camera camera;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    float shootRadius = 2f;

    Vector3 currentHitPosition;

    float offset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameManager.Instance.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 25, Color.yellow, 5);
            RaycastHit rayCast;
            if (Physics.Raycast(ray, out rayCast, 25))
            {
                if (rayCast.collider.CompareTag("RayCastPlane"))
                {
                    Debug.Log("Hit con el plano " + rayCast.point);
                    
                    Collider[] sphereCast;

                    currentHitPosition = rayCast.point;
                    sphereCast = Physics.OverlapSphere(rayCast.point, shootRadius);

                    if (sphereCast.Length > 0)
                    {
                        foreach (Collider enemy in sphereCast)
                        {
                            if (enemy.CompareTag("Enemy"))
                            {
                                Debug.Log("Hit con el enemigo");
                            }
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (horizontalMove != 0.0f)
        {
            Quaternion target = Quaternion.Euler(0, 0, (-1) * Mathf.Sign(horizontalMove) * 40);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5.0f);
            Debug.Log(transform.rotation);
        } else
        {
            Quaternion target = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5.0f);
            Debug.Log(transform.rotation);
        }

        if 
        (
            camera.IsPointInBounds(transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime + offset, 0, verticalMove * Time.fixedDeltaTime + offset)) &&
            camera.IsPointInBounds(transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime - offset, 0, verticalMove * Time.fixedDeltaTime - offset))
        )
        {
            transform.position = transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentHitPosition, shootRadius);
    }
}
