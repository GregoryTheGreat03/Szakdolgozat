using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Factory : Building{
    [SerializeField] protected DroneSO engineer;
    [SerializeField] protected DroneSO industrialMiner;

    [SerializeField] protected DroneSO assemblyLine1;
    [SerializeField] protected DroneSO assemblyLine2;
    [SerializeField] protected float productionSpeed;

    protected List<(float timeRemaining, DroneSO droneSO)> productionQueue = new List<(float, DroneSO)>();

    public override SaveData.BuildingSaveData SaveBuildingData() {
        List<float> productionQueueTimes = new List<float>();
        List<DroneSO> productionQueueSOs = new List<DroneSO>();
        foreach ((float timeRemaining, DroneSO droneSO) in productionQueue) {
            productionQueueTimes.Add(timeRemaining);
            productionQueueSOs.Add(droneSO);
        }
        return new SaveData.BuildingSaveData {
            buildingSO = buildingSO,
            position = transform.localPosition,
            health = health,
            targetable = targetable,
            constructionTimeRemaining = constructionTimeRemaining,
            productionQueueTimes = productionQueueTimes,
            productionQueueSOs = productionQueueSOs
        };
    }

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
    }

    public float GetProductionSpeed() {  return productionSpeed; }

    public List<(float, DroneSO)> GetProductionQueue() {
        return productionQueue;
    }

    public DroneSO GetAssemblyLine1() {
        if (assemblyLine1.NAME == industrialMiner.NAME && !TechSystem.IsTechUnlocked(TechSystem.Tech.IndustrialMiner)) {
            return null;
        }
        if (assemblyLine1.NAME == engineer.NAME && !TechSystem.IsTechUnlocked(TechSystem.Tech.Engineer)) {
            return null;
        }
        return assemblyLine1;
    }

    public DroneSO GetAssemblyLine2() {
        if (assemblyLine2.NAME == industrialMiner.NAME && !TechSystem.IsTechUnlocked(TechSystem.Tech.IndustrialMiner)) {
            return null;
        }
        if (assemblyLine2.NAME == engineer.NAME && !TechSystem.IsTechUnlocked(TechSystem.Tech.Engineer)) {
            return null;
        }
        return assemblyLine2;
    }

    public void AddToProductionQueue(DroneSO droneSO) {
        if (ResourceManager.IsEnougResources(droneSO)) {
            ResourceManager.ModifyEnergy(-1 * droneSO.ENERGY_COST);
            ResourceManager.ModifyCorium(-1 * droneSO.CORIUM_COST);
            ResourceManager.ModifyAntonium(-1 * droneSO.ANTONIUM_COST);

            productionQueue.Add((droneSO.prefab.GetComponent<Drone>().GetConstructionTime(), droneSO));
        }
    }

    protected override void Awake() {
        health = GetMaxHealth();
        targetable = false;
        ResourceManager.AddToBuildingList(this);

        TimeTickSystem.OnTick += AssembleTickUpdate;

        MessageSystem.OnMissionOver += EndOfMission;
    }

    protected void AssembleTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (productionQueue.Count > 0) {
            if (productionQueue[0].timeRemaining <= 0) {
                UI_Assistant.CreateNewGameMessage(productionQueue[0].droneSO.NAME + " drone ready.", 5);

                Transform droneTransform = Instantiate(productionQueue[0].droneSO.prefab);

                droneTransform.position = transform.position;
                droneTransform.GetComponent<Drone>().SetGetFollowPositionFunc(()=> transform.position);

                productionQueue.RemoveAt(0);

            } else {
                productionQueue[0] = (productionQueue[0].timeRemaining - productionSpeed * TimeTickSystem.TICK_TIMER_MAX, productionQueue[0].droneSO);
            }
        }
    }

    protected override void OnDestroy() {
        Debug.Log(ResourceManager.GetEnergy() + " destroy start");
        if (targetable) {
            ResourceManager.ModifyEnergy(buildingSO.ENERGY_COST);
        }
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }

        ResourceManager.RemoveFromBuildingList(this);

        TimeTickSystem.OnTick -= AssembleTickUpdate;
        MessageSystem.OnMissionOver -= EndOfMission;
        Debug.Log(ResourceManager.GetEnergy() + " destroy end");
    }
}
