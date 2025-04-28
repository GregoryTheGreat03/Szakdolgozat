using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineGunTurret : Turret{
    protected override void Awake() {
        constructionTimeRemaining = GetConstructionTime();
        health = GetMaxHealth();
        targetable = false;
        cooldown = 0;
        ResourceManager.AddToBuildingList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
        TimeTickSystem.OnTick += AttackTickUpdate;
    }

    protected override void OnDestroy() {
        ResourceManager.RemoveFromBuildingList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;
        if (targetable) {
            ResourceManager.ModifyEnergy(buildingSO.ENERGY_COST);
        }
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
    }
}
