using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Building : MonoBehaviour{

    [SerializeField] protected BuildingSO buildingSO;

    protected float health;
    protected bool targetable;
    protected float constructionTimeRemaining;

    public virtual SaveData.BuildingSaveData SaveBuildingData() {
        return new SaveData.BuildingSaveData {
            buildingSO = buildingSO,
            position = transform.localPosition,
            health = health,
            targetable = targetable,
            constructionTimeRemaining = constructionTimeRemaining,
        };
    }

    public virtual void LoadBuildingData(SaveData.BuildingSaveData buildingSaveData) {
        health = buildingSaveData.health;
        targetable = buildingSaveData.targetable;
        constructionTimeRemaining = buildingSaveData.constructionTimeRemaining;
        if (constructionTimeRemaining > 0) {
            this.GetComponentInChildren<Text>().enabled = false;
            TimeTickSystem.OnTick += ConstructionTickUpdate;
        }
    }

    protected virtual void Awake(){
        health = GetMaxHealth();
        targetable = false;
        ResourceManager.AddToBuildingList(this);

        MessageSystem.OnMissionOver += EndOfMission;
    }

    public BuildingSO GetBuildingSO() {
        return buildingSO;
    }
    public float GetHealth() {
        return health;
    }
    public float GetMaxHealth() {
        return buildingSO.HEALTH * TechSystem.GetBuildingHealthModifyer();
    }

    public float GetConstructionTimeRemaining() {
        return constructionTimeRemaining;
    }

    public float GetConstructionTime() {
        return buildingSO.CONSTRUCTION_TIME * TechSystem.GetConstructionSpeedModifyer();
    }

    public bool IsTargetable() {
        return targetable;
    }

    public void SetTargetable(bool targetable) {
        this.targetable = targetable;
    }

    public void StartConstuction() {
        constructionTimeRemaining = GetConstructionTime();
        health = 0;
        SetTargetable(true);
        this.GetComponentInChildren<Text>().enabled = false;
        TimeTickSystem.OnTick += ConstructionTickUpdate;
    }

    protected virtual void ConstructionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (constructionTimeRemaining > 0) {
            constructionTimeRemaining -= TimeTickSystem.TICK_TIMER_MAX;
            ModifyHealth((TimeTickSystem.TICK_TIMER_MAX / GetConstructionTime()) * GetMaxHealth());
        } else {
            this.GetComponentInChildren<Text>().enabled = true;
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
    }

    protected void EndOfMission(object sender, MessageSystem.OnMissionOverEventArgs args) {
        Destroy(gameObject);
    }

    public void ModifyHealth(float amount) {
        if (amount < 0) {
            UI_Assistant.CreateNewGameMessage("Your base is under attack!", 5);
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

    protected virtual void OnDestroy() {
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
        if (targetable) {
            ResourceManager.ModifyEnergy(buildingSO.ENERGY_COST);
        }

        ResourceManager.RemoveFromBuildingList(this);

        MessageSystem.OnMissionOver -= EndOfMission;
    }
}
