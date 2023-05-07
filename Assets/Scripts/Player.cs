using UnityEngine;

public class Player : Entity
{
    public float playerSpeed = 5.0f;
    public bool isWalking;

    TileSpawner tileSpawner;
    PlayerController controller;
    GunController gunController;
    Camera cam;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        cam = Camera.main;
        FindObjectOfType<EnemySpawner>().OnNewWave += OnNewWave;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnNewWave(int currentWaveNum)
    {
        m_health = startingHealth;
        gunController.Equip(currentWaveNum - 1);
    }

    void Update()
    {
        // Movement
        Vector3 horizInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = horizInput.normalized * playerSpeed;
        controller.Move(velocity);

        isWalking = horizInput != Vector3.zero;

        // Looking at mouse pos
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        if (transform.position.y < -5f)
        {
            Die();
        }

        // Shooting on left click
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            gunController.OnTriggerRelease();
        }
        // Reload on pressing R
        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
