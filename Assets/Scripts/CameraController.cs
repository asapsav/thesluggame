using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10f; // Speed of camera movement
    public float zoomSpeed = 10f; // Speed of camera zoom
    public float minZoom = 5f; // Minimum zoom distance
    public float maxZoom = 20f; // Maximum zoom distance

    void Update()
    {
        // Camera movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += movement * movementSpeed * Time.deltaTime;

        // Camera zoom
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * zoom * zoomSpeed;

        // Clamp zoom distance
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, minZoom, maxZoom);
        transform.position = cameraPosition;
    }
}

