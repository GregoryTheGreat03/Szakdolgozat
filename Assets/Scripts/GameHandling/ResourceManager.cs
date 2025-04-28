using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SaveData;

public class ResourceManager : MonoBehaviour{
    private static float energy = 0;
    private static float corium = 0;
    private static float antonium = 0;
    private static int sample = 0;

    private static float coriumOnShip = 0;
    private static float antoniumOnShip = 0;
    private static int sampleOnShip = 0;

    private static Dictionary<string, int> dronesOnShip = new Dictionary<string, int>();

    private static List<Drone> droneList = new List<Drone>();
    private static List<Building> buildingList = new List<Building>(); 

    public void Save(SaveData save) {
        save.energy = energy;
        save.corium = corium;
        save.antonium = antonium;
        save.sample = sample;
        save.coriumOnShip = coriumOnShip;
        save.antoniumOnShip = antoniumOnShip;
        save.sampleOnShip = sampleOnShip;
        save.dronesOnShipKeys = new List<string>(dronesOnShip.Keys);
        save.dronesOnShipValues = new List<int>(dronesOnShip.Values);

        save.dronesOnMap = new List<SaveData.DroneSaveData>();
        foreach (Drone drone in droneList) {
            save.dronesOnMap.Add(drone.SaveDroneData());
        }
        save.buildingsOnMap = new List<BuildingSaveData>();
        foreach (Building building in buildingList) {
            save.buildingsOnMap.Add(building.SaveBuildingData());
        }
    }

    public void Load(SaveData save) {
        energy = save.energy;
        corium = save.corium;
        antonium = save.antonium;
        sample = save.sample;

        coriumOnShip = save.coriumOnShip;
        antoniumOnShip = save.antoniumOnShip;
        sampleOnShip = save.sampleOnShip;
        dronesOnShip = new Dictionary<string, int>();
        int i = 0;
        foreach (string key in save.dronesOnShipKeys) {
            dronesOnShip.Add(key, save.dronesOnShipValues[i++]);
        }

        droneList = new List<Drone>();
        buildingList = new List<Building>();
        foreach (SaveData.DroneSaveData droneSaveData in save.dronesOnMap) {
            Transform droneTransform = Instantiate(droneSaveData.droneSO.prefab, droneSaveData.position, transform.rotation);
            droneTransform.GetComponent<Drone>().LoadDroneData(droneSaveData);
        }
        foreach (BuildingSaveData buildingSaveData in save.buildingsOnMap) {
            Transform buildingTransform = Instantiate(buildingSaveData.buildingSO.prefab, buildingSaveData.position, transform.rotation);
            buildingTransform.GetComponent<Building>().LoadBuildingData(buildingSaveData);
        }
    }

    public void TransferResourcesToShip() {
        coriumOnShip += corium;
        antoniumOnShip += antonium;
        sampleOnShip += sample;

        energy = 0;
        corium = 0;
        antonium = 0;
        sample = 0;
        droneList.Clear();
        buildingList.Clear();
    }

    public static List<Drone>GetDroneList() { 
        return droneList; 
    }

    public static void AddToDroneList(Drone drone) {
        droneList.Add(drone);
    }

    public static void RemoveFromDroneList(Drone drone) {
        droneList.Remove(drone);
    }

    public static List<Building> GetBuildingList() {
        return buildingList;
    }

    public static void AddToBuildingList(Building building) {
        buildingList.Add(building);
    }

    public static void RemoveFromBuildingList(Building building) {
        buildingList.Remove(building);
    }

    public static float GetEnergy() {
        return energy;
    }
    public static void SetEnergy(float amount) {
        energy = amount;
    }

    public static void ModifyEnergy(float amount) {
        if (energy + amount < 0) {
            int freedEnergy = 0;
            while (energy + amount + freedEnergy < 0) {
                if (droneList.Count != 0) {
                    int random = UnityEngine.Random.Range(0, droneList.Count);
                    freedEnergy += droneList[random].GetDroneSO().ENERGY_COST;
                    Destroy(droneList[random].gameObject);
                    droneList.RemoveAt(random);
                }
                else if (buildingList.Count != 0) {
                    // TODO
                    break;
                }
                break;
            }
            energy += amount;

        }
        else {
            energy += amount;
        }
    }

    public static float GetCorium() {
        return corium;
    }
    public static void SetCorium(float amount) {
        corium = amount;
    }

    public static float ModifyCorium(float amount) {
        if (corium + amount < 0) {
            float remaining = corium;
            corium = 0;
            return remaining;
        }
        else {
            corium += amount;
            return amount;
        }
    }

    public static float GetAntonium() {
        return antonium;
    }

    public static void SetAntonium(float amount) {
        antonium = amount;
    }

    public static float ModifyAntonium(float amount) {
        if (antonium + amount < 0) {
            float remaining = antonium;
            antonium = 0;
            return remaining;
        }
        else {
            antonium += amount;
            return amount;
        }
    }

    public static int GetSample() {
        return sample;
    }

    public static void ModifySample(int amount) {
        if (sample + amount < 0) {
            sample = 0;
        }
        else {
            sample += amount;
        }
    }

    public static bool IsEnougResources(DroneSO so) {
        if (so.ENERGY_COST > energy) {
            UI_Assistant.CreateNewGameMessage("Not enough energy.", 5);
            return false;
        }
        if (so.CORIUM_COST > corium) {
            UI_Assistant.CreateNewGameMessage("Not enough corum.", 5);
            return false;
        }
        if (so.ANTONIUM_COST > antonium) {
            UI_Assistant.CreateNewGameMessage("Not enough antonium.", 5);
            return false;
        }
        return true;
    }

    public static bool IsEnougResources(BuildingSO so) {
        if (so.ENERGY_COST > energy) {
            UI_Assistant.CreateNewGameMessage("Not enough energy.", 5);
            return false;
        }
        if (so.CORIUM_COST > corium) {
            UI_Assistant.CreateNewGameMessage("Not enough corum.", 5);
            return false;
        }
        if (so.ANTONIUM_COST > antonium) {
            UI_Assistant.CreateNewGameMessage("Not enough antonium.", 5);
            return false;
        }
        return true;
    }

    public static int GetCoriumOnShip() {
        return (int)coriumOnShip;
    }

    public static int GetAntoniumOnShip() {
        return (int)antoniumOnShip;
    }

    public static int GetSampleOnShip() {
        return sampleOnShip;
    }

    public void SetShipResources(int corium, int antonium, int sample) {
        coriumOnShip = corium;
        antoniumOnShip = antonium;
        sampleOnShip = sample;
    }

    public static bool IsEnougResourcesOnShip(int corium, int antonium, int sample) {
        if (corium > coriumOnShip) {
            return false;
        }
        if (antonium > antoniumOnShip) {
            return false;
        }
        if (sample > sampleOnShip) {
            return false;
        }
        return true;
    }

    public static Dictionary<string, int> GetDronesOnShip() {
        return dronesOnShip;
    }

    public void ClearDronesOnShip() {
        dronesOnShip.Clear();
    }

    public void AddDronesOnShip(string name, int amount) {
        if (dronesOnShip.ContainsKey(name)) {
            dronesOnShip[name] += amount;
        }
        else {
            dronesOnShip.Add(name, amount);
        }

        List<string> removable = new List<string>();

        foreach (string drone in dronesOnShip.Keys) {
            if (dronesOnShip[drone] <= 0) {
                removable.Add(drone);
            }
        }

        foreach (string drone in removable) {
            dronesOnShip.Remove(drone);
        }
    }

    private void Start() {
        ReferenceList.ResourceManager = this;
    }
}
