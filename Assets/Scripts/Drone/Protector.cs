using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : Drone{
    public override void Awake() {
        health = GetMaxHealth();
        cooldown = 0;
        ResourceManager.AddToDroneList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
        TimeTickSystem.OnTick += AttackTickUpdate;
    }

    // Update is called once per frame
    public void Update() {
        HandleMovement();
    }

    private void OnDestroy() {
        ResourceManager.RemoveFromDroneList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;
        ResourceManager.ModifyEnergy(droneSO.ENERGY_COST);
    }
}
