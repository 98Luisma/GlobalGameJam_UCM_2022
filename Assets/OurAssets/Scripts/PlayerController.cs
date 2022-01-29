using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private LayerMask _layerMask;

    public float runSpeed = 10f;
    
    private Camera _camera;
    private ObjectPool<Bullet> _bulletPool;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    float shootRadius = 2f;

    Vector3 currentHitPosition;

    float offset = 3f;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(_bulletPrefab, 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameManager.Instance.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 25, Color.yellow, 5);
            RaycastHit rayCast;
            if (Physics.Raycast(ray, out rayCast, 25, _layerMask))
            {
                if (rayCast.collider.CompareTag("RayCastPlane"))
                {
                _bulletPool.RequestObject(transform.position).SetupBullet(rayCast.point, shootRadius);
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
        } else
        {
            Quaternion target = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5.0f);
        }

        if 
        (
            _camera.IsPointInBounds(transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime + offset, 0, 0 )) &&
            _camera.IsPointInBounds(transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime - offset, 0, 0 ))
        )
        {
            transform.position = transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime, 0, 0);
        }

        if
        (
            _camera.IsPointInBounds(transform.position + new Vector3(0, 0, verticalMove * Time.fixedDeltaTime + offset)) &&
            _camera.IsPointInBounds(transform.position + new Vector3(0, 0, verticalMove * Time.fixedDeltaTime - offset))
        )
        {
            transform.position = transform.position + new Vector3(0, 0, verticalMove * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentHitPosition, shootRadius);
    }
}
