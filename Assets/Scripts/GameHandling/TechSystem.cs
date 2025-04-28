using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSystem{
    public enum Tech{
        BetterCircuits,
        DroneSpeed2,
        DroneSpeed3,
        Damage1,
        Damage2,
        WorkerEfficiency,
        DroneHealth2,
        DroneHealth3,
        WorkerAI,
        WorkerStorage1,
        WorkerStorage2,
        AttackSpeed,
        BuildingHealth1,
        BuildingHealth2,
        BuildingHealth3,
        HQ_Energy1,
        HQ_Energy2,
        HQ_Energy3,
        IndustrialMiner,
        IndustrialMinerResourceMultiply1,
        IndustrialMinerResourceMultiply2,
        IndustrialMinerEfficiency,
        FusionReactor,
        ConstructionSpeed1,
        ConstructionSpeed2,
        ConstructionSpeed3,
        Engineer,
        EngineerRepairSpeed1,
        EngineerRepairSpeed2,
        EngineerRange1
    }

    private static HashSet<Tech> unlockedTechs = new HashSet<Tech>();

    public static void Save(SaveData saveData) {
        saveData.unlockedTechs = new List<Tech>(unlockedTechs);
    }

    public static void Load(SaveData saveData) {
        unlockedTechs = new HashSet<Tech>(saveData.unlockedTechs);
    }

    public static void ClearTechs() {
        unlockedTechs.Clear();
    }

    public static void UnlockTech(Tech tech) {
        if (IsTechUnlockable(tech) && !IsTechUnlocked(tech)) {
            unlockedTechs.Add(tech);
        }
    }

    public static bool IsTechUnlockable(Tech tech) {
        switch (tech) {
            case Tech.BetterCircuits:
                return true;
            case Tech.BuildingHealth1:
                return true;
            case Tech.ConstructionSpeed1:
                return true;
            case Tech.DroneSpeed2:
                return unlockedTechs.Contains(Tech.BetterCircuits);
            case Tech.Damage1:
                return unlockedTechs.Contains(Tech.BetterCircuits);
            case Tech.WorkerEfficiency:
                return unlockedTechs.Contains(Tech.BetterCircuits);
            case Tech.DroneHealth2:
                return unlockedTechs.Contains(Tech.BetterCircuits);
            case Tech.DroneSpeed3:
                return unlockedTechs.Contains(Tech.DroneSpeed2);
            case Tech.Damage2:
                return unlockedTechs.Contains(Tech.Damage1);
            case Tech.WorkerAI:
                return unlockedTechs.Contains(Tech.WorkerEfficiency);
            case Tech.DroneHealth3:
                return unlockedTechs.Contains(Tech.DroneHealth2);
            case Tech.WorkerStorage1:
                return unlockedTechs.Contains(Tech.DroneHealth2);
            case Tech.AttackSpeed:
                return unlockedTechs.Contains(Tech.Damage2);
            case Tech.WorkerStorage2:
                return unlockedTechs.Contains(Tech.WorkerStorage1);
            case Tech.IndustrialMiner:
                return unlockedTechs.Contains(Tech.BuildingHealth1) && unlockedTechs.Contains(Tech.WorkerEfficiency);
            case Tech.BuildingHealth2:
                return unlockedTechs.Contains(Tech.BuildingHealth1);
            case Tech.HQ_Energy1:
                return unlockedTechs.Contains(Tech.BuildingHealth2);
            case Tech.HQ_Energy2:
                return unlockedTechs.Contains(Tech.HQ_Energy1);
            case Tech.HQ_Energy3:
                return unlockedTechs.Contains(Tech.HQ_Energy2);
            case Tech.BuildingHealth3:
                return unlockedTechs.Contains(Tech.BuildingHealth2);
            case Tech.FusionReactor:
                return unlockedTechs.Contains(Tech.BuildingHealth3);
            case Tech.IndustrialMinerEfficiency:
                return unlockedTechs.Contains(Tech.IndustrialMiner);
            case Tech.IndustrialMinerResourceMultiply1:
                return unlockedTechs.Contains(Tech.IndustrialMiner);
            case Tech.IndustrialMinerResourceMultiply2:
                return unlockedTechs.Contains(Tech.IndustrialMinerResourceMultiply1);
            case Tech.ConstructionSpeed2:
                return unlockedTechs.Contains(Tech.ConstructionSpeed1);
            case Tech.ConstructionSpeed3:
                return unlockedTechs.Contains(Tech.ConstructionSpeed2);
            case Tech.Engineer:
                return unlockedTechs.Contains(Tech.ConstructionSpeed2);
            case Tech.EngineerRange1:
                return unlockedTechs.Contains(Tech.Engineer);
            case Tech.EngineerRepairSpeed1:
                return unlockedTechs.Contains(Tech.Engineer);
            case Tech.EngineerRepairSpeed2:
                return unlockedTechs.Contains(Tech.EngineerRepairSpeed1);
            default:
                return false;
        }
    }

    public static bool IsTechUnlocked(Tech tech) {
        return unlockedTechs.Contains(tech);
    }

    public static float GetSpeedModifyer() {
        if (IsTechUnlocked(Tech.DroneSpeed3)) {
            return 1.3f;
        } 
        else if (IsTechUnlocked(Tech.DroneSpeed2)) {
            return 1.2f;
        }
        else if (IsTechUnlocked(Tech.BetterCircuits)) {
            return 1.1f;
        }
        return 1f;
    }

    public static float GetDamageModifyer() {
        if (IsTechUnlocked(Tech.Damage2)) {
            return 1.25f;
        }
        else if (IsTechUnlocked(Tech.Damage1)) {
            return 1.15f;
        }
        return 1f;
    }

    public static float GetAttackSpeedModifyer() {
        if (IsTechUnlocked(Tech.AttackSpeed)) {
            return 0.8f;
        }
        return 1f;
    }

    public static float GetDroneHealthModifyer() {
        if (IsTechUnlocked(Tech.DroneHealth3)) {
            return 1.3f;
        }
        else if (IsTechUnlocked(Tech.DroneHealth2)) {
            return 1.2f;
        }
        else if (IsTechUnlocked(Tech.BetterCircuits)) {
            return 1.1f;
        }
        return 1f;
    }

    public static float GetBuildingHealthModifyer() {
        if (IsTechUnlocked(Tech.BuildingHealth3)) {
            return 1.5f;
        }
        else if (IsTechUnlocked(Tech.BuildingHealth2)) {
            return 1.25f;
        }
        else if (IsTechUnlocked(Tech.BuildingHealth1)) {
            return 1.1f;
        }
        return 1f;
    }

    public static float GetWorkerEfficiencyModifyer() {
        if (IsTechUnlocked(Tech.WorkerEfficiency)) {
            return 1.5f;
        }
        return 1f;
    }

    public static int GetAdditionalWorkerStorage() {
        if (IsTechUnlocked(Tech.WorkerStorage2)) {
            return 10;
        }
        else if (IsTechUnlocked(Tech.WorkerStorage1)) {
            return 5;
        }
        return 0;
    }

    public static float GetConstructionSpeedModifyer() {
        if (IsTechUnlocked(Tech.ConstructionSpeed3)) {
            return 0.85f;
        }
        else if (IsTechUnlocked(Tech.ConstructionSpeed2)) {
            return 0.9f;
        }
        else if (IsTechUnlocked(Tech.ConstructionSpeed1)) {
            return 0.95f;
        }
        return 1f;
    }

    public static float GetIndustrialMinerEfficiencyModifyer() {
        if (IsTechUnlocked(Tech.IndustrialMinerEfficiency)) {
            return 1.5f;
        }
        return 1f;
    }

    public static int GetIndustrialMinerMultiplyModifyer() {
        if (IsTechUnlocked(Tech.IndustrialMinerResourceMultiply2)) {
            return 4;
        }
        else if (IsTechUnlocked(Tech.IndustrialMinerResourceMultiply1)) {
            return 3;
        }
        return 2;
    }

    public static float GetHQ_EnergyModifyer() {
        if (IsTechUnlocked(Tech.HQ_Energy3)) {
            return 4f;
        }
        else if (IsTechUnlocked(Tech.HQ_Energy2)) {
            return 3f;
        }
        else if (IsTechUnlocked(Tech.HQ_Energy1)) {
            return 2f;
        }
        return 1f;
    }

    public static float GetEngineerRangeModifyer() {
        if (IsTechUnlocked(Tech.EngineerRange1)) {
            return 1.5f;
        }
        return 1f;
    }

    public static float GetEngineerHealModifyer() {
        if (IsTechUnlocked(Tech.EngineerRepairSpeed2)) {
            return 3f;
        }
        else if (IsTechUnlocked(Tech.EngineerRepairSpeed1)) {
            return 2f;
        }
        return 1f;
    }
}
