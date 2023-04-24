using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Automatically attaches required components to the game object.
[RequireComponent(typeof(GunController))] 
[RequireComponent(typeof(PlayerController))] 

public class Player : MonoBehaviour
{
    public float playerSpeed = 5.0f;

    PlayerController controller;
    Camera cam;
    GunController gunController;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 horizInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = horizInput.normalized * playerSpeed;
        controller.Move(velocity);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }
}
