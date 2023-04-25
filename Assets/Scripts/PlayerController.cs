using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    public void Move(Vector3 m_velocity)
    {
        velocity = m_velocity;
    }

    public void LookAt(Vector3 point)
    {
        Vector3 correctLookAtPoint = new Vector3 (point.x, transform.position.y, point.z); // Correct the height of the point the player is looking at
        transform.LookAt(correctLookAtPoint);
    }
}