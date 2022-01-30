using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAirshipScript : MonoBehaviour
{
    private float speed = 0.2f;
    private float maxMove = 1f;
    private float orientation = 1f;
    private float amount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (amount > maxMove)
        {
            amount = 0;
            orientation = orientation * (-1);
        }


        amount += speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + (orientation * speed * Time.deltaTime), transform.position.z);
    }
}
