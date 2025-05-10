using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour{

    [SerializeField] CameraHandler cameraHandler;

    private Func<Vector3> GetCameraFollowPositionFunc;
    private Camera myCamera;
    private Func<float> GetCameraZoomFunc;

    private float cameraMoveSpeed = 5f;
    private float cameraZoomSpeed = 5f;

    public void Save(SaveData saveData) {
        saveData.cameraPosition = transform.position;
    }

    public void Load(SaveData saveData) {
        transform.position = saveData.cameraPosition;
        cameraHandler.FocusOn(transform.position);
    }

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc, Func<float> GetCameraZoomFunc) {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }

    private void Start() {
        myCamera = transform.GetComponent<Camera>();
        ReferenceList.cameraControl = this;
    }

    public void SetGetCameraFollowPositionFunc(Func<Vector3> GetCameraFollowPositionFunc){
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

    void Update(){
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement(){
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);

        //setting the camera movement on the frame
        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                //overshot the target
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }

    private void HandleZoom(){
        float cameraZoom = GetCameraZoomFunc();
        
        float cameraZoomDifference = cameraZoom - myCamera.orthographicSize;
        myCamera.orthographicSize += cameraZoomDifference * cameraZoomSpeed * Time.deltaTime;

        //overshooting
        if (cameraZoomDifference > 0) {
            if (myCamera.orthographicSize > cameraZoom) {
                myCamera.orthographicSize = cameraZoom;
            }
        } else { 
            if (myCamera.orthographicSize < cameraZoom) {
                myCamera.orthographicSize = cameraZoom;
            }
        }
    }
}
