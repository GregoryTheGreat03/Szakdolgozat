using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQ : Factory{

    [SerializeField] protected int energyGenerationAmount;

    public override void LoadBuildingData(SaveData.BuildingSaveData buildingSaveData) {
        health = buildingSaveData.health;
        targetable = buildingSaveData.targetable;
        constructionTimeRemaining = buildingSaveData.constructionTimeRemaining;
        productionQueue = new List<(float timeRemaining, DroneSO droneSO)>();
        if (constructionTimeRemaining > 0) {
            this.GetComponentInChildren<Text>().enabled = false;
            TimeTickSystem.OnTick += ConstructionTickUpdate;
        }
        int i = 0;
        foreach (float timeRemaining in buildingSaveData.productionQueueTimes) {
            productionQueue.Add((timeRemaining, buildingSaveData.productionQueueSOs[i++]));
        }
        ResourceManager.ModifyEnergy(-1*energyGenerationAmount * TechSystem.GetHQ_EnergyModifyer());
    }

    protected override void Awake(){
        health = GetMaxHealth();
        targetable = true;
        ResourceManager.AddToBuildingList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += AssembleTickUpdate;

        ResourceManager.ModifyEnergy(energyGenerationAmount * TechSystem.GetHQ_EnergyModifyer());
    }

    public int GetEnergyGeneration() {
        return (int)(energyGenerationAmount * TechSystem.GetHQ_EnergyModifyer());
    }


    protected override void OnDestroy() {
        if (health <= 0) {
            ReferenceList.GameHandler.MissionOver();
        }

        ResourceManager.RemoveFromBuildingList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= AssembleTickUpdate;
    }
}
