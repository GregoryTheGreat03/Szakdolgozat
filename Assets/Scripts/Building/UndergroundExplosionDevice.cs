using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndergroundExplosionDevice : Building{
    [SerializeField] private float COOLDOWN;
        
    private float cooldown;
    private bool active = false;

    public class OnFireEventArgs : EventArgs {
    }

    public static event EventHandler<OnFireEventArgs> OnFire;

    public override SaveData.BuildingSaveData SaveBuildingData() {
        return new SaveData.BuildingSaveData {
            buildingSO = buildingSO,
            position = transform.localPosition,
            health = health,
            targetable = targetable,
            constructionTimeRemaining = constructionTimeRemaining,
            cooldown = cooldown,
            active = active,
        };
    }

    public override void LoadBuildingData(SaveData.BuildingSaveData buildingSaveData) {
        health = buildingSaveData.health;
        targetable = buildingSaveData.targetable;
        constructionTimeRemaining = buildingSaveData.constructionTimeRemaining;
        cooldown = buildingSaveData.cooldown;
        active = buildingSaveData.active;
        if (constructionTimeRemaining > 0) {
            this.GetComponentInChildren<Text>().enabled = false;
            TimeTickSystem.OnTick += ConstructionTickUpdate;
        }
    }

    protected override void Awake() {
        health = GetMaxHealth();
        targetable = false;
        ResourceManager.AddToBuildingList(this);

        TimeTickSystem.OnTick += CooldownTickUpdate;
        MessageSystem.OnMissionOver += EndOfMission;
    }

    public bool IsActivated() {
        return active;
    }

    public void Activate() {
        if (!active) {
            cooldown = COOLDOWN;
        }
        active = true;
    }

    public float GetCooldown() {
        return cooldown;
    }

    protected void CooldownTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (active) {
            cooldown -= TimeTickSystem.TICK_TIMER_MAX;
            if (cooldown <= 0) {
                Fire();
            }
        }
    }

    protected override void OnDestroy() {
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
        if (targetable) {
            ResourceManager.ModifyEnergy(buildingSO.ENERGY_COST);
        }

        MessageSystem.OnMissionOver -= EndOfMission;
        ResourceManager.RemoveFromBuildingList(this);
    }

    private void Fire() {
        if (OnFire != null) OnFire(this, new OnFireEventArgs { });
    }
}
