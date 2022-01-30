using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _turretTransform;

    public float runSpeed = 10f;
    
    private Camera _camera;
    private ObjectPool<Bullet> _bulletPool;

    Vector2 moveDirection = Vector2.zero;

    float shootRadius = 2f;

    Vector3 currentHitPosition;

    float offset = 1f;

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
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalMove, verticalMove).normalized * runSpeed;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * 25, Color.yellow, 5);
        RaycastHit rayCast;
        if (Physics.Raycast(ray, out rayCast, 25, _layerMask))
        {
            // Turn the turret
            if (_turretTransform)
            {
                Vector3 lookPoint = rayCast.point;
                lookPoint.y = _turretTransform.position.y;
                _turretTransform.LookAt(lookPoint, Vector3.up);
            }

            // Shoot on player input
            if (Input.GetButtonDown("Fire1") && rayCast.collider.CompareTag("RayCastPlane"))
            {
                _bulletPool.RequestObject(transform.position).SetupBullet(rayCast.point, shootRadius);
                GameManager.Instance.OnPlayerBulletSpawned();
            }
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.x != 0.0f)
        {
            Quaternion target = Quaternion.Euler(0, 0, (-1) * Mathf.Sign(moveDirection.x) * 40);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5.0f);
        } else
        {
            Quaternion target = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5.0f);
        }

        if 
        (
            _camera.IsPointInBounds(transform.position + new Vector3(moveDirection.x * Time.fixedDeltaTime + offset, 0, 0 )) &&
            _camera.IsPointInBounds(transform.position + new Vector3(moveDirection.x * Time.fixedDeltaTime - offset, 0, 0 ))
        )
        {
            transform.position = transform.position + new Vector3(moveDirection.x * Time.fixedDeltaTime, 0, 0);
        }

        if
        (
            _camera.IsPointInBounds(transform.position + new Vector3(0, 0, moveDirection.y * Time.fixedDeltaTime + offset)) &&
            _camera.IsPointInBounds(transform.position + new Vector3(0, 0, moveDirection.y * Time.fixedDeltaTime - offset))
        )
        {
            transform.position = transform.position + new Vector3(0, 0, moveDirection.y * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentHitPosition, shootRadius);
    }
}
