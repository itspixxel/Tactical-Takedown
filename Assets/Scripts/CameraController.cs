using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TileSpawner spawner;
    public float orthoScaleLerpSpeed = 2f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        // Calculate the new orthographic size based on the level size
        float targetOrthoSize = spawner.currentLevel.levelSize.x + 2f;

        // Lerp between the current orthographic size and the target size
        float newOrthoSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, Time.deltaTime * orthoScaleLerpSpeed);

        // Set the new orthographic size of the camera
        cam.orthographicSize = newOrthoSize;
    }
}
