using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Entity
{
    public float playerSpeed = 5.0f;

    PlayerController controller;
    GunController gunController;
    Camera cam;

    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        cam = Camera.main;
    }

    void Update()
    {
        // Movement
        Vector3 horizInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = horizInput.normalized * playerSpeed;
        controller.Move(velocity);

        // Looking at mouse pos
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        // Shooting on left click
        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }
}
