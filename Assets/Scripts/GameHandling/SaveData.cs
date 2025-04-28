using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CampaignHandler;

[Serializable]
public class SaveData{
    // From Game Handler
    public GameHandler.GameState gameState;
    public float timeScale;
    public bool sandboxMission;

    // From Time Tick System
    public int tick;

    // From Camera
    public Vector3 cameraPosition;

    // From Campaign Handler
    public bool enemyDatabankUnlocked;
    public bool techTreeUnlocked;
    public List<string> buildingsOnShip;
    public List<Missions> unlockedMissions;
    public List<string> unlockedBuildings;

    // From Resource Manager
    public float energy;
    public float corium;
    public float antonium;
    public int sample;

    public float coriumOnShip;
    public float antoniumOnShip;
    public int sampleOnShip;

    public List<string> dronesOnShipKeys;
    public List<int> dronesOnShipValues;

    public List<DroneSaveData> dronesOnMap;

    [Serializable]
    public struct DroneSaveData {
        public DroneSO droneSO;
        public Vector3 position;
        public float health;
        public float cooldown;
        public Vector3 nextFollowPosition;

        // Worker and Industrial Miner
        public float corium;
        public float antonium;

        // Only Worker
        public int sample;

        // Only Industrial Miner
        public bool deploying;
        public float timeRemainingOfDeployment;
    }

    public List<BuildingSaveData> buildingsOnMap;

    [Serializable]
    public struct BuildingSaveData {
        public BuildingSO buildingSO;
        public Vector3 position;
        public float health;
        public bool targetable;
        public float constructionTimeRemaining;

        // Only Factorys and HQ
        public List<float> productionQueueTimes;
        public List<DroneSO> productionQueueSOs;

        // Only Turrets and UED
        public float cooldown;

        // only Thumper and UED
        public bool active;

        // Only Thumper
        public int thumperStartTick;
        public float activeSeconds;
    }

    // From Enemy Spawning
    public float secondsBewtweenWaves;
    public float enemyRushTime;
    public bool thumperActivated;
    public bool enemyRush;

    public List<EnemySaveData> enemiesOnMap;

    [Serializable]
    public struct EnemySaveData {
        public EnemySO enemySO;
        public Vector3 position;
        public float health;
        public float cooldown;
        public Vector3 nextFollowPosition;

        // Only Hive
        public int timesSpawned;

        // Only Hivemind
        public bool invulnerable;
    }

    // From Statistics Tracker
    public int enemyKillCountAltogether;
    public int enemyKillCountInMission;
    public float timeElapsed;

    // From Story Writer
    public List<string> titleList;
    public List<string> bodyList;
    public List<float> timePerCharacter;

    // From Resource Spawning
    public List<SaveData.ResourceSaveData> resourceFieldsOnMap;

    [Serializable]
    public struct ResourceSaveData {
        public ResourceSO resourceSO;
        public Vector3 position;
        public float amount;
        public float miningTimeDone;
    }

    // From Tech System
    public List<TechSystem.Tech> unlockedTechs;

    // From Hivemind
    public bool hivemindInvulnerable;
}
