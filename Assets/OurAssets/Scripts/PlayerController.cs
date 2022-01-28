using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed = 10f;
    public Camera camera;

    float horizontalMove = 0f;
    float verticalMove = 0f;

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
                Debug.Log("Raycast hecho");
            }

        }
    }

    void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(horizontalMove * Time.fixedDeltaTime, 0, verticalMove * Time.fixedDeltaTime);
    }
}
