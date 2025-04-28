using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ_AndDroneSpawning : MonoBehaviour{
    [SerializeField] private GameHandler gameHandler;

    [SerializeField] private BuildingSO HQ_SO;

    [SerializeField] private Vector3 hQ_Position;

    [SerializeField] private DroneSO workerSO;
    [SerializeField] private DroneSO patrollerSO;
    [SerializeField] private DroneSO protectorSO;
    [SerializeField] private DroneSO guardianSO;
    [SerializeField] private DroneSO engineerSO;
    [SerializeField] private DroneSO industrialMinerSO;

    [SerializeField] private int startingWorkerAmount;
    [SerializeField] private int startingPatrollerAmount;
    [SerializeField] private int startingProtectorAmount;
    [SerializeField] private int startingGuardianAmount;
    [SerializeField] private int startingEngineerAmount;
    [SerializeField] private int startingIndustrialMinerAmount;

    public bool CheckStartingDroneEnergyConsumption(int worker, int patroller, int protector, int guardian, int engineer, int industrialMiner) {
        return HQ_SO.prefab.GetComponent<HQ>().GetEnergyGeneration() >= worker * workerSO.ENERGY_COST + patroller * patrollerSO.ENERGY_COST + protector * protectorSO.ENERGY_COST + guardian * guardianSO.ENERGY_COST + engineer * engineerSO.ENERGY_COST + industrialMiner * industrialMinerSO.ENERGY_COST; 
    }

    public Vector3 GetHQPosition() {
        return hQ_Position;
    }

    public void SetStartingWorkerAmount(int amount) {
        startingWorkerAmount = amount;
    }

    public void SetStartingPatrollerAmount(int amount) {
        startingPatrollerAmount = amount;
    }
    public void SetStartingProtectorAmount(int amount) {
        startingProtectorAmount = amount;
    }
    public void SetStartingGuardianAmount(int amount) {
        startingGuardianAmount = amount;
    }
    public void SetStartingEngineerAmount(int amount) {
        startingEngineerAmount = amount;
    }
    public void SetStartingIndustrialMinerAmount(int amount) {
        startingIndustrialMinerAmount = amount;
    }

    public void InitializeMission() {

        Transform HQ_Transform = Instantiate(HQ_SO.prefab);
        HQ_Transform.localPosition = hQ_Position;

        SpawnSmallDroneAmount(startingWorkerAmount, workerSO);
        SpawnSmallDroneAmount(startingPatrollerAmount, patrollerSO);
        SpawnSmallDroneAmount(startingProtectorAmount, protectorSO);
        SpawnSmallDroneAmount(startingEngineerAmount, engineerSO);

        SpawnLargeDroneAmount(startingGuardianAmount, guardianSO);
        SpawnLargeDroneAmount(startingIndustrialMinerAmount, industrialMinerSO);
    }

    private void SpawnSmallDroneAmount(int amount, DroneSO droneSO) {
        for (int i = 0; i < amount; i++) {
            Transform droneTransform = Instantiate(droneSO.prefab);

            ResourceManager.ModifyEnergy(-1 * droneSO.ENERGY_COST);
            droneTransform.localPosition = transform.position;
            Drone drone = droneTransform.GetComponent<Drone>();
            Vector3 position = Utils.GenerateRandomPosition(4, transform.position);
            drone.SetGetFollowPositionFunc(() => position);
        }
    }

    private void SpawnLargeDroneAmount(int amount, DroneSO droneSO) {
        for (int i = 0; i < amount; i++) {
            Transform droneTransform = Instantiate(droneSO.prefab);

            ResourceManager.ModifyEnergy(-1 * droneSO.ENERGY_COST);
            droneTransform.localPosition = transform.position;
            Drone drone = droneTransform.GetComponent<Drone>();
            drone.SetGetFollowPositionFunc(() => transform.position);
        }
    }
}
