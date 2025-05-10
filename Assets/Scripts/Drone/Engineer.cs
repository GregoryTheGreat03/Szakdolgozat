using UnityEngine;

public class Engineer : Drone {

    [SerializeField] private float REPAIR_EFFICIENCY;

    private GameObject repairTarget;

    public override float GetRange() {
        return droneSO.RANGE * TechSystem.GetEngineerRangeModifyer();
    }

    public override void Awake() {
        REPAIR_EFFICIENCY *= TechSystem.GetEngineerHealModifyer();
        health = GetMaxHealth();
        cooldown = 0;
        ResourceManager.AddToDroneList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
        TimeTickSystem.OnTick += RepairTickUpdate;
    }

    public void Update() {
        HandleMovement();
    }

    private void OnDestroy() {
        ResourceManager.RemoveFromDroneList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        TimeTickSystem.OnTick -= RepairTickUpdate;
        ResourceManager.ModifyEnergy(droneSO.ENERGY_COST);
    }

    protected override void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {
            repairTarget = null;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Drone>(out Drone drone)) {
                    if (repairTarget is null && drone.GetHealth() != drone.GetMaxHealth()) repairTarget = drone.gameObject;
                    if (repairTarget is not null && drone.GetHealth() != drone.GetMaxHealth() && Utils.PositionDistance(transform.position, repairTarget.transform.position) > Utils.PositionDistance(transform.position, drone.transform.position)) {
                        repairTarget = drone.gameObject;
                    }
                }
            }

            if (repairTarget is not null) { return; }

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Building>(out Building building)) {
                    if (repairTarget is null && building.GetHealth() != building.GetMaxHealth()) repairTarget = building.gameObject;
                    if (repairTarget is not null && building.GetHealth() != building.GetMaxHealth() && Utils.PositionDistance(transform.position, repairTarget.transform.position) > Utils.PositionDistance(transform.position, building.transform.position)) {
                        repairTarget = building.gameObject;
                    }
                }
            }
        }
    }

    protected void RepairTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (repairTarget is not null) {
            if (repairTarget.TryGetComponent<Drone>(out Drone drone)) {
                drone.ModifyHealth(REPAIR_EFFICIENCY);
            }
            else if (repairTarget.TryGetComponent<Building>(out Building building)) {
                building.ModifyHealth(REPAIR_EFFICIENCY);
            }

            cooldown = 0;
            repairTarget = null;
        }
    }
}
