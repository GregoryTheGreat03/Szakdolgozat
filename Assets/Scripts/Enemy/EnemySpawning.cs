using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawning : MonoBehaviour {

    [SerializeField] private CampaignHandler campaignHandler;

    [SerializeField] private EnemySO swarmerSO; //"s"
    [SerializeField] private EnemySO warriorSO; //"w"
    [SerializeField] private EnemySO melterSO; //"m"
    [SerializeField] private EnemySO breacherSO; //"b"
    [SerializeField] private EnemySO hiveSO; //"h"

    [SerializeField] private EnemySO hivemindSO;

    [SerializeField] private Vector3 hivemindPosition;

    [SerializeField] private int startingHiveAmount;

    [SerializeField] private List<string> enemySpawningGeneratorList;
    [SerializeField] private float minSpawningDistance;
    [SerializeField] private float maxSpawningDistance;
    [SerializeField] private float secondsBewtweenWaves;
    [SerializeField] private float minWaveStrength;
    [SerializeField] private float maxWaveStrength;
    [SerializeField] private float enemyRushTime;

    private const int MINUTE_IN_SECS = 60;
    private int nthWave = 1;
    private bool thumperActivated = false;
    private static bool enemyRush;
    private static List<Enemy> enemyList;

    public void Save(SaveData save) {
        save.secondsBewtweenWaves = secondsBewtweenWaves;
        save.enemyRushTime = enemyRushTime;
        save.thumperActivated = thumperActivated;
        save.enemyRush = enemyRush;

        save.enemiesOnMap = new List<SaveData.EnemySaveData>();
        foreach (Enemy enemy in enemyList) {
            save.enemiesOnMap.Add(enemy.SaveEnemyData());
        }
    }

    public void Load(SaveData save) {
        secondsBewtweenWaves = save.secondsBewtweenWaves;
        enemyRushTime = save.enemyRushTime;
        thumperActivated = save.thumperActivated;
        enemyRush = save.enemyRush;

        enemyList = new List<Enemy>();
        foreach (SaveData.EnemySaveData enemySaveData in save.enemiesOnMap) {
            Transform enemyTransform = Instantiate(enemySaveData.enemySO.prefab, enemySaveData.position, transform.rotation);
            enemyTransform.GetComponent<Enemy>().LoadEnemyData(enemySaveData);
        }
    }

    public static List<Enemy> GetEnemyList() {
        return enemyList;
    }

    public static void AddToEnemyList(Enemy enemy) {
        enemyList.Add(enemy);
    }

    public static void RemoveFromEnemyList(Enemy enemy) {
        enemyList.Remove(enemy);
    }

    public void SetStartingHiveAmount(int amount) {
        startingHiveAmount = amount;
    }

    public void SetEnemySpawningGeneratorList(List<string> enemyList) {
        enemySpawningGeneratorList = enemyList;
    }

    public void SetMinSpawningDistance(float distance) {
        minSpawningDistance = distance;
    }

    public void SetMaxSpawningDistance(float distance) {
        maxSpawningDistance = distance;
    }

    public void SetSecondsBetweenWaves(float seconds) {
        secondsBewtweenWaves = seconds;
    }

    public void SetMinWaveStrength(float strength) {
        minWaveStrength = strength;
    }

    public void SetMaxWaveStrength(float strength) {
        maxWaveStrength = strength;
    }

    public void SetEnemyRushTime(float time) {
        enemyRushTime = time;
    }

    public void ThumperActivated() {
        thumperActivated = true;
        UI_Assistant.CreateNewGameMessage("Thumper activated. Expect strengthening tetranid waves.", 15);
    }

    public void ThumperDestroyed() {
        thumperActivated = false;
        UI_Assistant.CreateNewGameMessage("Thumper destroyed. Expect stagnation in tetranid waves.", 15);
    }

    public void SpawnHivemind() {
        Instantiate(hivemindSO.prefab, hivemindPosition, transform.rotation);
    }

    public static bool IsEnemyRush() {
        return enemyRush;
    }

    void Start(){
        enemyList = new List<Enemy>();
        ReferenceList.EnemySpawning = this;
        TimeTickSystem.OnTick += EnemyWaveOnTickUpdate;
    }

    public void InitializeMission() {
        nthWave = 1;
        enemyRush = false;
        thumperActivated = false;
        for (int i = 0; i < startingHiveAmount; i++) {
            Vector3 spawningPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minSpawningDistance, maxSpawningDistance), transform.position);
            Transform enemyTransform = Instantiate(hiveSO.prefab, spawningPosition, transform.rotation);
        }
    }

    protected void EnemyWaveOnTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (args.tick * TimeTickSystem.TICK_TIMER_MAX >= secondsBewtweenWaves * nthWave) {

            Debug.Log("Spawning enemy wave");

            if (enemySpawningGeneratorList.Count == 0) {
                return;
            }

            float minutes = args.tick * TimeTickSystem.TICK_TIMER_MAX / MINUTE_IN_SECS;

            if (thumperActivated) {
                minWaveStrength *= 1.1f;
                maxWaveStrength *= 1.1f;
            }

            if (minutes >= enemyRushTime && !enemyRush) {
                minWaveStrength *= 2;
                maxWaveStrength *= 2;
                secondsBewtweenWaves /= 2;
                nthWave *= 2;
                enemyRush = true;
                UI_Assistant.CreateNewGameMessage("Tetranid rush imminent! Expected strength: UNKNOWN", 15);
            }

            float waveStrength = nthWave * UnityEngine.Random.Range(minWaveStrength, maxWaveStrength);

            if (!enemyRush) {
                UI_Assistant.CreateNewGameMessage("Enemy wave incoming! Expected strength: " + (int)Math.Ceiling(waveStrength), 10);
            }

            while (waveStrength > 0) {
                Vector3 spawningPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minSpawningDistance, maxSpawningDistance), transform.position);

                switch (enemySpawningGeneratorList[UnityEngine.Random.Range(0, enemySpawningGeneratorList.Count)]) {
                    case "h":
                        if (hiveSO.FIRST_SPAWNING_WAVE < nthWave) {
                            Instantiate(hiveSO.prefab, spawningPosition, transform.rotation);
                            waveStrength -= hiveSO.WAVE_WEIGHT;
                            //Debug.Log("Hive spawned");
                            break;
                        }
                        goto case "b";
                    case "b":
                        if (breacherSO.FIRST_SPAWNING_WAVE < nthWave && enemySpawningGeneratorList.Contains("b")) {
                            Instantiate(breacherSO.prefab, spawningPosition, transform.rotation);
                            waveStrength -= breacherSO.WAVE_WEIGHT;
                            //Debug.Log("Breacher spawned");
                            break;
                        }
                        goto case "m";
                    case "m":
                        if (melterSO.FIRST_SPAWNING_WAVE < nthWave && enemySpawningGeneratorList.Contains("m")) {
                            Instantiate(melterSO.prefab, spawningPosition, transform.rotation);
                            waveStrength -= melterSO.WAVE_WEIGHT;
                            //Debug.Log("Melter spawned");
                            break;
                        }
                        goto case "w";
                    case "w":
                        if (warriorSO.FIRST_SPAWNING_WAVE < nthWave && enemySpawningGeneratorList.Contains("w")) {
                            Instantiate(warriorSO.prefab, spawningPosition, transform.rotation);
                            waveStrength -= warriorSO.WAVE_WEIGHT;
                            //Debug.Log("Warrior spawned");
                            break;
                        }
                        goto case "s";
                    case "s":
                        if (swarmerSO.FIRST_SPAWNING_WAVE < nthWave && enemySpawningGeneratorList.Contains("s")) {
                            Instantiate(swarmerSO.prefab, spawningPosition, transform.rotation);
                            waveStrength -= swarmerSO.WAVE_WEIGHT;
                            //Debug.Log("Swarmer spawned");
                            break;
                        }
                        goto default;
                    default:
                        Debug.Log("No enemy Spawned");
                        waveStrength -= 1;
                        break;
                }
            }
            nthWave++;
        }
    }
}
