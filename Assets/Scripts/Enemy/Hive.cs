using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Enemy {
    [SerializeField] private List<string> enemySpawningGeneratorList;

    [SerializeField] private EnemySO swarmerSO; //"s"
    [SerializeField] private EnemySO warriorSO; //"w"
    [SerializeField] private EnemySO melterSO; //"m"
    [SerializeField] private EnemySO breacherSO; //"b"

    [SerializeField] private ResourceSO alienSampleSO;

    [SerializeField] private float minSpawningDistance;
    [SerializeField] private float maxSpawningDistance;

    private const int MINUTE_IN_SECS = 60;
    private int timesSpawned = 0;

    public override SaveData.EnemySaveData SaveEnemyData() {
        return new SaveData.EnemySaveData {
            enemySO = enemySO,
            position = transform.localPosition,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
            timesSpawned = timesSpawned,
        };
    }

    public override void LoadEnemyData(SaveData.EnemySaveData enemySaveData) {
        health = enemySaveData.health;
        cooldown = enemySaveData.cooldown;
        timesSpawned = enemySaveData.timesSpawned;
        SetGetFollowPositionFunc(() => enemySaveData.nextFollowPosition);
    }

    public override void Awake() {
        health = enemySO.HEALTH;
        cooldown = 0;
        target = null;
        SetGetFollowPositionFunc(() => transform.position);

        EnemySpawning.AddToEnemyList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += SpawnTickUpdate;
    }


    private void OnDestroy() {
        // check if destroyed because of death
        if (health <= 0) {
            StatisticsTracker.EnemyKillCountIncrease();

            for (int i = 0; i < 3; i++) {
                Vector3 spawningPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minSpawningDistance, maxSpawningDistance), transform.position);
                Instantiate(alienSampleSO.PREFAB, spawningPosition, transform.rotation);
            }
        }

        EnemySpawning.RemoveFromEnemyList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= SpawnTickUpdate;
    }

    protected void SpawnTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == enemySO.COOLDOWN) {
            Vector3 spawningPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minSpawningDistance, maxSpawningDistance), transform.position);

            switch (enemySpawningGeneratorList[UnityEngine.Random.Range(0, enemySpawningGeneratorList.Count)]) {
                case "b":
                    if (breacherSO.FIRST_SPAWNING_WAVE < timesSpawned) {
                        Instantiate(breacherSO.prefab, spawningPosition, transform.rotation);
                        break;
                    }
                    goto case "m";
                case "m":
                    if (melterSO.FIRST_SPAWNING_WAVE < timesSpawned) {
                        Instantiate(melterSO.prefab, spawningPosition, transform.rotation);
                        break;
                    }
                    goto case "w";
                case "w":
                    if (warriorSO.FIRST_SPAWNING_WAVE < timesSpawned) {
                        Instantiate(warriorSO.prefab, spawningPosition, transform.rotation);
                        break;
                    }
                    goto case "s";
                case "s":
                    if (swarmerSO.FIRST_SPAWNING_WAVE < timesSpawned) {
                        Instantiate(swarmerSO.prefab, spawningPosition, transform.rotation);
                    }
                    break;
            }
            timesSpawned++;
            cooldown = 0;
        }
    }
}
