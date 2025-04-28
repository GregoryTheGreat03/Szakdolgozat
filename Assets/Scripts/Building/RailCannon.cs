using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RailCannon : Turret{
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

    protected override void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {
            target = null;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Breacher>(out Breacher breacher)) {
                    if (target is null) target = breacher;
                    if (Utils.PositionDistance(transform.position, target.transform.position) > Utils.PositionDistance(transform.position, breacher.transform.position)) {
                        target = breacher;
                    }
                }
                if (collider2D.TryGetComponent<Hive>(out Hive hive)) {
                    if (target is null) target = hive;
                    if (Utils.PositionDistance(transform.position, target.transform.position) > Utils.PositionDistance(transform.position, hive.transform.position)) {
                        target = hive;
                    }
                }
            }
        }
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
