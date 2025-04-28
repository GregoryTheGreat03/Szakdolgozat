using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionHandler : MonoBehaviour{

    [SerializeField] private BuildingGridSystem buildingGridSystem;
    [SerializeField] private Transform selectionAreaTransform;
    [SerializeField] private Transform attackRadiusDrawing;

    private Vector3 selectStartPosition;
    private bool selectionStarted = false;
    private const float REQUIRED_MINIMAL_AREA_SELECT_DISTANCE = 0.3f;
    private HashSet<Drone> selectedDrones = new HashSet<Drone>();
    private Building selectedBuilding;
    private List<Transform> drawnRadius = new List<Transform>();
    private List<World_Bar> drawnHealthBars = new List<World_Bar>();

    public List<GameObject> GetSelected() {
        List<GameObject> selected = new List<GameObject>();
        if (selectedBuilding != null) {
            selected.Add(selectedBuilding.gameObject);
        }
        if (selectedDrones.Count > 0) {
            foreach (Drone drone in selectedDrones) {
                if (!drone.IsDestroyed()) {
                    selected.Add(drone.gameObject);
                }
            }
        }
        return selected;
    }

    public void ClearSelected() {
        selectedBuilding = null;
        selectedDrones.Clear();
    }

    void Update(){
        HandleSelectUnits();
        HandleSelectedHealthBarDrawing();
        HandleSelectedRangeDrawing();
        HandleMovementOrder();
    }

    private void HandleSelectUnits() {
        if (buildingGridSystem.buildingWithMouse) {
            selectedDrones.Clear();
            return;
        }

        if (Input.GetMouseButtonDown(0)) {

            if (Utils.IsUI_Cilck()) {
                Debug.Log("UI click");
                return;
            }
            
            // Mouse Pressed
            selectStartPosition = UtilsClass.GetMouseWorldPosition();
            selectionAreaTransform.position = selectStartPosition;
            selectionAreaTransform.gameObject.SetActive(true);
            selectionStarted = true;

            Debug.Log(selectStartPosition);
        }
        if (Input.GetMouseButton(0) && selectionStarted) {
            // Mouse Held Down
            Vector3 selectionAreaSize = UtilsClass.GetMouseWorldPosition() - selectStartPosition;
            selectionAreaTransform.localScale = selectionAreaSize;
        }

        if (Input.GetMouseButtonUp(0) && selectionStarted) {
            selectionStarted = false;
            selectionAreaTransform.gameObject.SetActive(false);
            Vector3 selectEndPosition = UtilsClass.GetMouseWorldPosition();
            Debug.Log(selectEndPosition);

            Vector2 lowerLeftPosition = new Vector2(Mathf.Min(selectStartPosition.x, selectEndPosition.x), Mathf.Min(selectStartPosition.y, selectEndPosition.y));
            Vector2 upperRightPosition = new Vector2(Mathf.Max(selectStartPosition.x, selectEndPosition.x), Mathf.Max(selectStartPosition.y, selectEndPosition.y));

            if (!Input.GetKey(KeyCode.LeftControl)) {
                selectedDrones.Clear();
            }
            selectedBuilding = null;


            // Single or area selection
            if (Utils.PositionDistance(new Vector3(lowerLeftPosition.x, lowerLeftPosition.y, 0), new Vector3(upperRightPosition.x, upperRightPosition.y, 0)) < REQUIRED_MINIMAL_AREA_SELECT_DISTANCE) {

                Debug.Log("Single selection");
                Collider2D[] colliderArray = Physics2D.OverlapPointAll(new Vector2(selectStartPosition.x, selectStartPosition.y));
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Drone>(out Drone drone)) {
                        selectedDrones.Add(drone);
                        selectedBuilding = null;

                        if (Input.GetKey(KeyCode.LeftShift)) {
                            UpdateSelectedRangeDrawing();
                            UpdateSelectedHealthBarDrawing();
                        }
                        return;
                    }
                }

                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        selectedBuilding = building;
                        selectedDrones.Clear();

                        if (Input.GetKey(KeyCode.LeftShift)) {
                            UpdateSelectedRangeDrawing();
                            UpdateSelectedHealthBarDrawing();
                        }
                        return;
                    }
                }

                if (Input.GetKey(KeyCode.LeftShift)) {
                    UpdateSelectedRangeDrawing();
                    UpdateSelectedHealthBarDrawing();
                }

            }
            else {

                Collider2D[] colliderArray = Physics2D.OverlapAreaAll(lowerLeftPosition, upperRightPosition);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Drone>(out Drone drone)) {
                        selectedDrones.Add(drone);
                    }
                }

                if (Input.GetKey(KeyCode.LeftShift)) {
                    UpdateSelectedRangeDrawing();
                    UpdateSelectedHealthBarDrawing();
                }
            }
        }
    }

    private void HandleSelectedRangeDrawing() {

        if (Input.GetKeyDown(KeyCode.LeftShift)) {

            if (selectedDrones.Count != 0) {
                foreach (Drone drone in selectedDrones) {
                    Transform attackRadius = Instantiate(attackRadiusDrawing);
                    attackRadius.GetComponent<AttackRadiusDrawing>().SetAssistTransform(drone.transform);
                    drawnRadius.Add(attackRadius);

                }
            }

            if (selectedBuilding != null && selectedBuilding.TryGetComponent<Turret>(out Turret turret)) {
                Debug.Log("Drawing turret range");
                Transform attackRadius = Instantiate(attackRadiusDrawing);
                attackRadius.GetComponent<AttackRadiusDrawing>().SetAssistTransform(turret.transform);
                drawnRadius.Add(attackRadius);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            foreach (Transform radius in drawnRadius) {
                Destroy(radius.gameObject);
            }
            drawnRadius.Clear();
        }
    }

    private void HandleSelectedHealthBarDrawing() {

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (selectedDrones.Count != 0) {
                foreach (Drone drone in selectedDrones) {
                    World_Bar healthBar = World_Bar.Create(drone.transform, drone.GetHealthBarPosition(), drone.GetHealthBarSize(), new Color32(0, 100, 0, 255), new Color32(0, 255, 0, 255), drone.GetHealth() / drone.GetMaxHealth(), 5);
                    drawnHealthBars.Add(healthBar);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            foreach (World_Bar healthBar in drawnHealthBars) {
                if (!healthBar.GetTransform().IsDestroyed()) {
                    Drone drone = healthBar.GetTransform().GetComponentInParent<Drone>();
                    healthBar.SetSize(drone.GetHealth() / drone.GetMaxHealth());
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            foreach (World_Bar healthBar in drawnHealthBars) {
                if (!healthBar.GetTransform().IsDestroyed()) {
                    healthBar.DestroySelf();
                }
            }
            drawnHealthBars.Clear();
        }
    }

    private void UpdateSelectedRangeDrawing() {
        foreach (Transform radius in drawnRadius) {
            Destroy(radius.gameObject);
        }
        drawnRadius.Clear();

        if (selectedDrones.Count != 0) {
            foreach (Drone drone in selectedDrones) {
                Transform attackRadius = Instantiate(attackRadiusDrawing);
                attackRadius.GetComponent<AttackRadiusDrawing>().SetAssistTransform(drone.transform);
                drawnRadius.Add(attackRadius);
            }
        }

        if (selectedBuilding != null && selectedBuilding.TryGetComponent<Turret>(out Turret turret)) {
            Debug.Log("Drawing turret range");
            Transform attackRadius = Instantiate(attackRadiusDrawing);
            attackRadius.GetComponent<AttackRadiusDrawing>().SetAssistTransform(turret.transform);
            drawnRadius.Add(attackRadius);
        }
    }

    private void UpdateSelectedHealthBarDrawing() {
        foreach (World_Bar healthBar in drawnHealthBars) {
            healthBar.DestroySelf();
        }
        drawnHealthBars.Clear();

        if (selectedDrones.Count != 0) {
            foreach (Drone drone in selectedDrones) {
                World_Bar healthBar = World_Bar.Create(drone.transform, drone.GetHealthBarPosition(), drone.GetHealthBarSize(), new Color32(0, 100, 0, 255), new Color32(0, 255, 0, 255), drone.GetHealth() / drone.GetMaxHealth(), 5);
                drawnHealthBars.Add(healthBar);
            }
        }
    }

    private void HandleMovementOrder() {
        Vector3 waypoint = UtilsClass.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(1)) {
            foreach (Drone drone in selectedDrones) {
                drone.SetGetFollowPositionFunc(() => waypoint);
            }
        }
    }
}
