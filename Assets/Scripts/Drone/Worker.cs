using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Drone{
    [SerializeField] private GameObject loadedSprite;
    [SerializeField] private GameObject hasSampleSprite;

    [SerializeField] private int CAPACITY;
    [SerializeField] private float MINING_EFFICIENCY;
    [SerializeField] private float AI_ENEMY_DETECTION_RANGE;
    private float corium;
    private float antonium;
    private int sample;
    private bool aI_Active;
    private bool beingChased;
    private Vector3 miningLocation;

    public override SaveData.DroneSaveData SaveDroneData() {
        return new SaveData.DroneSaveData {
            droneSO = droneSO,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
            position = transform.localPosition,
            corium = corium,
            antonium = antonium,
            sample = sample
        };
    }

    public override void LoadDroneData(SaveData.DroneSaveData droneSaveData) {
        health = droneSaveData.health;
        cooldown = droneSaveData.cooldown;
        SetGetFollowPositionFunc(() => droneSaveData.nextFollowPosition);
        corium = droneSaveData.corium;
        antonium = droneSaveData.antonium;
        sample = droneSaveData.sample;
    }

    public override void Awake() {
        CAPACITY += TechSystem.GetAdditionalWorkerStorage();
        MINING_EFFICIENCY *= TechSystem.GetWorkerEfficiencyModifyer();

        loadedSprite.SetActive(false);
        hasSampleSprite.SetActive(false);

        health = GetMaxHealth();
        corium = 0f;
        antonium = 0f;
        ResourceManager.AddToDroneList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
        TimeTickSystem.OnTick += AttackTickUpdate;
    }

    public void Update(){
        HandleMovement();
    }

    public int GetCapacity() {
        return CAPACITY;
    }

    public bool HasSample() {
        return sample > 0;
    }

    public bool IsAI_Active() {
        return aI_Active;
    }

    public int GetStoredResourceAmount() {
        return (int)(corium + antonium);
    }

    public void LoadCorium(int amount) {
        corium += amount;
        CheckIfFull();
    }
    public void LoadAntonium(int amount) {
        antonium += amount;
        CheckIfFull();
    }
    public void SetCapacity(int amount) {
        CAPACITY = amount;
    }

    public void SetMiningLocation(Vector3 position) {
        miningLocation = position;
    }

    public void SetAI_Active(bool aI_Active) {
        this.aI_Active = aI_Active;
        if (aI_Active) {
            miningLocation = Vector3.zero;
        }
    }

    private void CheckIfFull() {
        if (corium + antonium == CAPACITY) {
            loadedSprite.SetActive(true);
        }
    }

    protected override void HandleMovement() {
        if (aI_Active) {
            if (miningLocation != Vector3.zero && (corium + antonium == CAPACITY || beingChased || HasSample())) {
                SetGetFollowPositionFunc(() => new Vector3(0, 0, 0));
            } else {
                SetGetFollowPositionFunc(() => miningLocation);
            }
        }

        waypoint = GetFollowPositionFunc();
        waypoint.z = transform.position.z;

        Vector3 moveDir = (waypoint - transform.position).normalized;
        float distance = Vector3.Distance(waypoint, transform.position);

        //setting the drone movement
        if (distance > 0) {
            Vector3 newPosition = transform.position + moveDir * Mathf.Clamp(distance, slowestSpeedPercentage, 1) * GetMaxSpeed() * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newPosition, waypoint);

            if (distanceAfterMoving > distance) {
                newPosition = waypoint;
            }

            transform.position = newPosition;
        }
    }


    private void OnDestroy() {
        ResourceManager.RemoveFromDroneList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;
        ResourceManager.ModifyEnergy(droneSO.ENERGY_COST);
    }

    protected void AI_OnTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {

        if (aI_Active) {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, AI_ENEMY_DETECTION_RANGE);
            beingChased = false;
            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Enemy>(out Enemy enemy)) {
                    beingChased = true;
                    break;
                }
            }
        }
    }

    protected override void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {
            target = null;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Enemy>(out Enemy enemy)) {
                    if (target is null) target = enemy;
                    if (Utils.PositionDistance(transform.position, target.transform.position) > Utils.PositionDistance(transform.position, enemy.transform.position)) {
                        target = enemy;
                    }
                } 
                
                // when HQ can be reached
                else if (collider2D.TryGetComponent<HQ>(out HQ hq)) {
                    ResourceManager.ModifyCorium(corium);
                    corium = 0;
                    ResourceManager.ModifyAntonium(antonium);
                    antonium = 0;
                    ResourceManager.ModifySample(sample);
                    sample = 0;

                    loadedSprite.SetActive(false);
                    hasSampleSprite.SetActive(false);
                } 
                
                // resource detection
                else if (collider2D.TryGetComponent<Resource>(out Resource resource)) {
                    if (cooldown == GetCooldownMax()) {
                        if (corium + antonium < CAPACITY && (resource.GetResourceSO().ID == 1 || resource.GetResourceSO().ID == 2)) {

                            float amount = resource.MineResourceAmount(
                                    (corium + antonium + 1 <= CAPACITY) ?
                                    1 :
                                    CAPACITY - corium - antonium,
                                    MINING_EFFICIENCY
                                );

                            if (resource.GetResourceSO().ID == 1) {
                                corium += amount;
                            }
                            else if (resource.GetResourceSO().ID == 2) {
                                antonium += amount;
                            }

                            CheckIfFull();
                            cooldown = 0f;
                        }
                        else if (resource.GetResourceSO().ID == 3 && sample == 0) {

                            float amount = resource.MineResourceAmount(
                                    (corium + antonium + 1 <= CAPACITY) ?
                                    1 :
                                    CAPACITY - corium - antonium,
                                    MINING_EFFICIENCY
                                );

                            if (amount > 0) {
                                sample++;
                                hasSampleSprite.SetActive(true);
                            }

                            cooldown = 0f;
                        }
                    }
                }
            }
        }
    }

    public override void ModifyHealth(float amount) {
        if (amount < 0) {
            UI_Assistant.CreateNewGameMessage("One of your workers is under attack!", 5);
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
