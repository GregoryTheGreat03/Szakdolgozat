using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thumper : Building{

    private bool active = false;
    private int thumperStartTick = 0;
    private float activeSeconds = 0;

    public override SaveData.BuildingSaveData SaveBuildingData() {
        return new SaveData.BuildingSaveData {
            buildingSO = buildingSO,
            position = transform.localPosition,
            health = health,
            targetable = targetable,
            constructionTimeRemaining = constructionTimeRemaining,
            active = active,
            thumperStartTick = thumperStartTick,
            activeSeconds = activeSeconds
        };
    }

    public override void LoadBuildingData(SaveData.BuildingSaveData buildingSaveData) {
        health = buildingSaveData.health;
        targetable = buildingSaveData.targetable;
        constructionTimeRemaining = buildingSaveData.constructionTimeRemaining;
        active = buildingSaveData.active;
        thumperStartTick = buildingSaveData.thumperStartTick;
        activeSeconds = buildingSaveData.activeSeconds;
        if (constructionTimeRemaining > 0) {
            this.GetComponentInChildren<Text>().enabled = false;
            TimeTickSystem.OnTick += ConstructionTickUpdate;
        }
    }

    protected override void Awake() {
        health = GetMaxHealth();
        targetable = false;
        ResourceManager.AddToBuildingList(this);

        TimeTickSystem.OnTick += CheckThumperTimeTickUpdate;

        MessageSystem.OnMissionOver += EndOfMission;
    }

    public bool IsActivated() {
        return active;
    }

    public void Activate() {
        if (!active) {
            ReferenceList.EnemySpawning.ThumperActivated();
        }
        active = true;
    }

    public float GetActiveSeconds() {
        return activeSeconds;
    }

    protected void CheckThumperTimeTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (active && thumperStartTick == 0) {
            thumperStartTick = args.tick;
            Debug.Log("Thumper start tick set: " + thumperStartTick);
        }
        else if (active) {
            ReferenceList.CampaignHandler.FourthMissionCompletedCheck(args.tick - thumperStartTick);
            activeSeconds = (args.tick - thumperStartTick) * TimeTickSystem.TICK_TIMER_MAX;
        }
    }

    protected override void OnDestroy() {
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
        if (targetable) {
            ResourceManager.ModifyEnergy(buildingSO.ENERGY_COST);
        }

        ReferenceList.EnemySpawning.ThumperDestroyed();

        TimeTickSystem.OnTick -= CheckThumperTimeTickUpdate;
        MessageSystem.OnMissionOver -= EndOfMission;

        ResourceManager.RemoveFromBuildingList(this);
    }
}
