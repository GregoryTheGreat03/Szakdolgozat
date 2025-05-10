using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGridSystem : MonoBehaviour {

    [SerializeField] private int TILE_SIZE;

    public bool buildingWithMouse = false;

    private Transform testTransform;

    public Vector3 SnapToGrid(Vector3 position, float size) {
        float x = Mathf.Round(position.x + (size % 2 == 1 ? TILE_SIZE / 2f : 0)) - (size % 2 == 1 ? TILE_SIZE / 2f : 0);
        float y = Mathf.Round(position.y + (size % 2 == 1 ? TILE_SIZE / 2f : 0)) - (size % 2 == 1 ? TILE_SIZE / 2f : 0);
        return new Vector3(x, y, 0);
    }

    public void CancelBuilding() {
        if (testTransform != null) {
            Destroy(testTransform.gameObject);
            testTransform = null;
        }
        buildingWithMouse = false;
        UI_Assistant.SetBuildingMenuToOuterMenu();
    }
    public void BuildBuilding(BuildingSO buildingSO) {
        if (testTransform != null) {
            Destroy(testTransform.gameObject);
            testTransform = null;
        }
        if (ResourceManager.IsEnougResources(buildingSO)) {
            testTransform = Instantiate(buildingSO.prefab);
            buildingWithMouse = true;
        }
    }

    private void Start() {
        ReferenceList.BuildingGridSystem = this;
    }

    private void Update() {
        if (testTransform != null) {
            testTransform.position = SnapToGrid(UtilsClass.GetMouseWorldPosition(), testTransform.gameObject.GetComponent<Building>().GetBuildingSO().SIZE);
            
            if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < Screen.width - 300 * Screen.height / 720) {
                Debug.Log("Placing Building");
                bool occupied = false;

                // checking if space is occupied by resources
                float size = testTransform.gameObject.GetComponent<Building>().GetBuildingSO().SIZE;
                Vector2 vectorizedSize = new Vector2(size, size);
                Collider2D[] colliderArray = Physics2D.OverlapBoxAll(testTransform.position, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Resource>(out Resource resource)) {
                        UI_Assistant.CreateNewGameMessage("You cannot build here. A resource field is in the way.", 5);
                        occupied = true;
                    }
                }

                // checking if space is occupied by a building
                int buildingCounter = 0;
                size = testTransform.gameObject.GetComponent<Building>().GetBuildingSO().SIZE - 0.1f;
                vectorizedSize = new Vector2(size, size);
                colliderArray = Physics2D.OverlapBoxAll(testTransform.position, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        buildingCounter++;
                    }
                }
                if (buildingCounter > 1) {
                    UI_Assistant.CreateNewGameMessage("You cannot build here. A building is in the way.", 5);
                    occupied = true;
                }

                // checking if there are connecting buildings
                buildingCounter = 0;
                size = testTransform.gameObject.GetComponent<Building>().GetBuildingSO().SIZE-0.1f;
                vectorizedSize = new Vector2(size, size);
                Vector2 vectorizedPosition = new Vector2(testTransform.position.x + 0.1f, testTransform.position.y);
                colliderArray = Physics2D.OverlapBoxAll(vectorizedPosition, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        buildingCounter++;
                    }
                }

                vectorizedPosition = new Vector2(testTransform.position.x - 0.1f, testTransform.position.y);
                colliderArray = Physics2D.OverlapBoxAll(vectorizedPosition, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        buildingCounter++;
                    }
                }

                vectorizedPosition = new Vector2(testTransform.position.x, testTransform.position.y + 0.1f);
                colliderArray = Physics2D.OverlapBoxAll(vectorizedPosition, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        buildingCounter++;
                    }
                }

                vectorizedPosition = new Vector2(testTransform.position.x, testTransform.position.y - 0.1f);
                colliderArray = Physics2D.OverlapBoxAll(vectorizedPosition, vectorizedSize, 0f);
                foreach (Collider2D collider2D in colliderArray) {
                    if (collider2D.TryGetComponent<Building>(out Building building)) {
                        buildingCounter++;
                    }
                }
                if (buildingCounter <= 4) {
                    UI_Assistant.CreateNewGameMessage("Buildings must be connected to each other", 5);
                    occupied = true;
                }

                // if building can be placed
                if (!occupied) {
                    Building buildableBuilding = testTransform.gameObject.GetComponent<Building>();
                    if (ResourceManager.IsEnougResources(buildableBuilding.GetBuildingSO())){
                        buildableBuilding.StartConstuction();
                        UI_Assistant.CreateNewGameMessage("A " + buildableBuilding.GetBuildingSO().NAME + " will be built in " + buildableBuilding.GetConstructionTime() + " seconds.", 5);

                        ResourceManager.ModifyEnergy(-1 * buildableBuilding.GetBuildingSO().ENERGY_COST);
                        ResourceManager.ModifyCorium(-1 * buildableBuilding.GetBuildingSO().CORIUM_COST);
                        ResourceManager.ModifyAntonium(-1 * buildableBuilding.GetBuildingSO().ANTONIUM_COST);

                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                            testTransform = Instantiate(buildableBuilding.GetBuildingSO().prefab);
                        }
                        else {
                            testTransform = null;
                            buildingWithMouse = false;
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            CancelBuilding();
        }
    }
}
