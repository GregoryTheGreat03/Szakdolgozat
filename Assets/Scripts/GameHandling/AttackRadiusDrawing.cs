using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackRadiusDrawing : MonoBehaviour{
    [SerializeField] private LineRenderer circleRender;

    private const int ATTACK_RADIUS_DRAWING_STEPS = 100;

    private Transform assistTransform; 
    private float radius;

    public void SetRadius(float radius) {
        this.radius = radius;
    }

    public void SetAssistTransform(Transform assistTransform) {
        this.assistTransform = assistTransform;
        if (assistTransform.gameObject.TryGetComponent<Drone>(out Drone drone)) {
            radius = drone.GetRange();

        } else if (assistTransform.gameObject.TryGetComponent<Turret>(out Turret turret)) {
            radius = turret.GetRange();
        }
    }

    public void Update() {
        if (assistTransform.IsDestroyed()) {
            gameObject.SetActive(false);
            return;
        }
        DrawArea(assistTransform.position, radius);
    }

    public void DrawArea(Vector3 position, float radius) {
        for (int currentStep = 0; currentStep < ATTACK_RADIUS_DRAWING_STEPS; currentStep++) {
            float circumferenceProgress = (float)currentStep / ATTACK_RADIUS_DRAWING_STEPS;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xNormalized = Mathf.Cos(currentRadian);
            float yNormalized = Mathf.Sin(currentRadian);

            Vector3 currentPosition = new Vector3(xNormalized * radius, yNormalized * radius, 0) + position;

            circleRender.SetPosition(currentStep, currentPosition);
        }
    }
}
