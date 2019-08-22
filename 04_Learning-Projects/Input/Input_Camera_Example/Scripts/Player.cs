using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 100f;
    public float angularSpeed = 40f;
    private Rigidbody _rigidbody = null;
    private bool jumpable = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKey("left"))
            transform.RotateAround(transform.position, Vector3.up, -angularSpeed * Time.deltaTime );
        else if( Input.GetKey("right"))
            transform.RotateAround(transform.position, Vector3.up, angularSpeed * Time.deltaTime );
        else if( Input.GetKey("up"))
            transform.position += transform.forward * ( speed * Time.deltaTime);
        else if( Input.GetKey("down"))
            transform.position += transform.forward * ( -speed * Time.deltaTime);
        else if (Input.GetKey("space") && _rigidbody != null && jumpable)
        {
            jumpable = false;
            _rigidbody.AddForce(0, jumpForce, 0);
        }    
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.name == "Floor")
            jumpable = true;
    }
}
