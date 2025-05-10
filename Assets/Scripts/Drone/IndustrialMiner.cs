using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialMiner : Drone {
    [SerializeField] private GameObject loadedSprite;

    [SerializeField] private List<GameObject> legSprites;

    [SerializeField] private int CAPACITY;
    [SerializeField] private float MINING_EFFICIENCY;
    [SerializeField] private float DEPLOYMENT_TIME;
    private float RESOURCE_MULTIPLIER;

    private bool deploying;
    private float timeRemainingOfDeployment;
    private float corium;
    private float antonium;
    private int mass = 1000;

    public override SaveData.DroneSaveData SaveDroneData() {
        return new SaveData.DroneSaveData {
            droneSO = droneSO,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
            position = transform.localPosition,
            corium = corium,
            antonium = antonium,
            deploying = deploying,
            timeRemainingOfDeployment = timeRemainingOfDeployment,
        };
    }

    public override void LoadDroneData(SaveData.DroneSaveData droneSaveData) {
        health = droneSaveData.health;
        cooldown = droneSaveData.cooldown;
        SetGetFollowPositionFunc(() => droneSaveData.nextFollowPosition);
        corium = droneSaveData.corium;
        antonium = droneSaveData.antonium;
        deploying = droneSaveData.deploying;
        timeRemainingOfDeployment = droneSaveData.timeRemainingOfDeployment;
    }

    public override void Awake() {
        MINING_EFFICIENCY *= TechSystem.GetIndustrialMinerEfficiencyModifyer();
        RESOURCE_MULTIPLIER = TechSystem.GetIndustrialMinerMultiplyModifyer();

        loadedSprite.SetActive(false);

        health = GetMaxHealth();
        timeRemainingOfDeployment = DEPLOYMENT_TIME;
        deploying = false;
        corium = 0f;
        antonium = 0f;
        ResourceManager.AddToDroneList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
    }

    public void Update() {
        HandleDeployment();
        HandleMovement();
    }

    public int GetCapacity() {
        return CAPACITY;
    }

    public int GetStorageAmount() {
        return (int)(corium + antonium);
    }
    public bool IsDeployed() {
        return deploying;
    }

    public void DeploymentSwitch() {
        deploying = !deploying;
        if (deploying) {
            gameObject.GetComponent<Rigidbody2D>().mass = int.MaxValue;
        }
        else {
            gameObject.GetComponent<Rigidbody2D>().mass = mass;
        }
    }

    private void OnDestroy() {
        ResourceManager.RemoveFromDroneList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        ResourceManager.ModifyEnergy(droneSO.ENERGY_COST);
    }

    protected void HandleDeployment() {
        if (deploying && timeRemainingOfDeployment > 0) {
            timeRemainingOfDeployment -= Time.deltaTime;
            if (timeRemainingOfDeployment < 0) {
                timeRemainingOfDeployment = 0;
            }
        } else if (!deploying && timeRemainingOfDeployment < DEPLOYMENT_TIME) {
            timeRemainingOfDeployment += Time.deltaTime;
            if (timeRemainingOfDeployment > DEPLOYMENT_TIME) {
                timeRemainingOfDeployment = DEPLOYMENT_TIME;
            }
        }
        foreach (GameObject leg in legSprites) {
            if (leg != null) {
                leg.transform.localPosition = new Vector3(0, 1 + (DEPLOYMENT_TIME - timeRemainingOfDeployment) / DEPLOYMENT_TIME / 2, 0);
            }
        }
    }

    protected override void HandleMovement() {
        waypoint = GetFollowPositionFunc();
        waypoint.z = transform.position.z;

        Vector3 moveDir = (waypoint - transform.position).normalized;
        float distance = Vector3.Distance(waypoint, transform.position);

        //setting the drone movement
        if (distance > 0 && timeRemainingOfDeployment == DEPLOYMENT_TIME) {
            Vector3 newPosition = transform.position + moveDir * Mathf.Clamp(distance, slowestSpeedPercentage, 1) * GetMaxSpeed() * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newPosition, waypoint);

            if (distanceAfterMoving > distance) {
                newPosition = waypoint;
            }

            transform.position = newPosition;
        }
    }

    protected override void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {
            target = null;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {

                if (timeRemainingOfDeployment == 0 && collider2D.TryGetComponent<Resource>(out Resource resource)) {
                    if (corium + antonium < CAPACITY) {
                        if (cooldown == GetCooldownMax()) {

                            float amount = resource.MineResourceAmount(
                                    (corium + antonium + (1 * RESOURCE_MULTIPLIER) <= CAPACITY) ?
                                    1 :
                                    (CAPACITY - corium - antonium) / RESOURCE_MULTIPLIER,
                                    MINING_EFFICIENCY
                                );

                            if (resource.GetResourceSO().ID == 1) {
                                corium += amount * RESOURCE_MULTIPLIER;
                            }
                            else if (resource.GetResourceSO().ID == 2) {
                                antonium += amount * RESOURCE_MULTIPLIER;
                            }

                            if (corium + antonium == CAPACITY) {
                                loadedSprite.SetActive(true);
                            }

                            cooldown = 0f;
                        }
                    }
                }

                // when a worker is in reach
                else if (collider2D.TryGetComponent<Worker>(out Worker worker)) {
                    if (worker.GetCapacity() > worker.GetStoredResourceAmount()) {
                        if (corium > 0) {
                            if (corium >= worker.GetCapacity() - worker.GetStoredResourceAmount()) {
                                corium -= worker.GetCapacity() - worker.GetStoredResourceAmount();
                                worker.LoadCorium(worker.GetCapacity() - worker.GetStoredResourceAmount());
                            }
                            else {
                                worker.LoadCorium((int)corium);
                                corium -= (int)corium;
                            }
                            loadedSprite.SetActive(false);
                        } 
                        else if (antonium > 0) {
                            if (antonium >= worker.GetCapacity() - worker.GetStoredResourceAmount()) {
                                antonium -= worker.GetCapacity() - worker.GetStoredResourceAmount();
                                worker.LoadAntonium(worker.GetCapacity() - worker.GetStoredResourceAmount());
                            }
                            else {
                                worker.LoadAntonium((int)antonium);
                                antonium -= (int)antonium;
                            }
                            loadedSprite.SetActive(false);
                        }
                    }
                }

                // when HQ can be reached
                else if (collider2D.TryGetComponent<HQ>(out HQ hq)) {
                    ResourceManager.ModifyCorium(corium);
                    corium = 0;
                    ResourceManager.ModifyAntonium(antonium);
                    antonium = 0;

                    loadedSprite.SetActive(false);
                }
            }
        }
    }

    public override void ModifyHealth(float amount) {
        if (amount < 0) {
            UI_Assistant.CreateNewGameMessage("One of your industrial miners is under attack!", 5);
        }
        if (health + amount <= 0) {
            health = 0;
            Destroy(gameObject);
        }
        else if (health + amount >= GetMaxHealth()) {
            health = GetMaxHealth();
        }
        else {
            health += amount;
        }
    }

}
