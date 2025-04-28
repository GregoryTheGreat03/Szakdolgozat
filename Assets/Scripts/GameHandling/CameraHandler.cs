using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour{

    [SerializeField] private CameraControl cameraControl;

    [SerializeField] private bool cameraMovementWithMouse = false;

    private Vector3 cameraFollowPosition;
    [SerializeField] private float cameraMoveAmount = 10f;

    [SerializeField] private float zoom = 10f;
    [SerializeField] private float zoomAmount = 4f;
    [SerializeField] private float zoomMin = 5f;
    [SerializeField] private float zoomMax = 20f;
    [SerializeField] private float startingZoom = 20f;

    public void FocusOn(Vector3 position) {
        cameraFollowPosition = position;
        zoom = startingZoom;
    }

    void Start(){
        cameraControl.Setup(() => cameraFollowPosition, () => zoom);
    }

    void Update()
    {
        HandleManualMovement();
        HandleZoom();
    }

    //camera movement with keys and mouse
    private void HandleManualMovement() {
        float edgeSize = 20f;
        if (Input.GetKey(KeyCode.W) || (cameraMovementWithMouse && Input.mousePosition.y > Screen.height - edgeSize)) {
            cameraFollowPosition.y += cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || (cameraMovementWithMouse && Input.mousePosition.y < edgeSize)) {
            cameraFollowPosition.y -= cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || (cameraMovementWithMouse && Input.mousePosition.x < edgeSize)) {
            cameraFollowPosition.x -= cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || (cameraMovementWithMouse && Input.mousePosition.x > Screen.width - edgeSize)) {
            cameraFollowPosition.x += cameraMoveAmount * Time.deltaTime;
        }
    }

    //camera zoom with keys and mouse
    private void HandleZoom() {
        if (Input.GetKey(KeyCode.KeypadPlus)) {
            zoom -= zoomAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.KeypadMinus)) {
            zoom += zoomAmount * Time.deltaTime;
        }

        zoom -= Input.mouseScrollDelta.y;

        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
    }
}
