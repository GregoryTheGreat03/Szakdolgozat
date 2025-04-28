using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NextClickLocator : MonoBehaviour{
    [SerializeField] private Transform attackRadiusDrawing;
    [SerializeField] private Transform crosshair;

    private bool active;
    private Transform radius;
    private Action<Vector3> setableFunction;

    public void GetAndSetNextClickLocation(Action<Vector3> setableFunction, float crosshairRange) {
        this.setableFunction = setableFunction;
        active = true;
        Cursor.visible = false;
        radius.GetComponent<AttackRadiusDrawing>().SetRadius(crosshairRange);
        crosshair.gameObject.SetActive(true);
        radius.gameObject.SetActive(true);
    }

    private void Start() {
        radius = Instantiate(attackRadiusDrawing);
        radius.GetComponent<AttackRadiusDrawing>().SetAssistTransform(crosshair);
        crosshair.gameObject.SetActive(false);
        radius.gameObject.SetActive(false);
    }

    private void Update() {
        if (active) {
            crosshair.position = UtilsClass.GetMouseWorldPosition();
            if (Input.GetMouseButtonDown(0) && !Utils.IsUI_Cilck()) {
                active = false;
                setableFunction(UtilsClass.GetMouseWorldPosition());
                Cursor.visible = true;
                crosshair.gameObject.SetActive(false);
                radius.gameObject.SetActive(false);
            }
        }
    }
}
