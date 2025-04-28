using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hivemind : Enemy{
    
    [SerializeField] private ResourceSO alienSampleSO;

    [SerializeField] private Transform vulnerable;

    [SerializeField] private float minSpawningDistance;
    [SerializeField] private float maxSpawningDistance;

    private static bool invulnerable = true;

    public static void Save(SaveData saveData) {
        saveData.hivemindInvulnerable = invulnerable;
    }

    public static void Load(SaveData saveData) {
        invulnerable = saveData.hivemindInvulnerable;
    }

    public override SaveData.EnemySaveData SaveEnemyData() {
        return new SaveData.EnemySaveData {
            enemySO = enemySO,
            position = transform.localPosition,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
            invulnerable = invulnerable,
        };
    }

    public override void LoadEnemyData(SaveData.EnemySaveData enemySaveData) {
        health = enemySaveData.health;
        cooldown = enemySaveData.cooldown;
        invulnerable = enemySaveData.invulnerable;
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

        UndergroundExplosionDevice.OnFire += UED_FireUpdate;

        if (invulnerable) {
            vulnerable.gameObject.SetActive(false);
        }
        else {
            vulnerable.gameObject.SetActive(true);
        }
    }

    public override void ModifyHealth(float amount) {
        if (amount < 0 && invulnerable) {
            return;
        }
        if (health + amount <= 0) {
            health = 0;
            cooldown = 0;
            Destroy(gameObject);
        }
        else if (health + amount >= enemySO.HEALTH) {
            health = enemySO.HEALTH;
        }
        else {
            health += amount;
        }
    }

    private void OnDestroy() {
        // check if destroyed because of death
        if (health <= 0) {
            StatisticsTracker.EnemyKillCountIncrease();

            for (int i = 0; i < 10; i++) {
                Vector3 spawningPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minSpawningDistance, maxSpawningDistance), transform.position);
                Instantiate(alienSampleSO.PREFAB, spawningPosition, transform.rotation);
            }

            ReferenceList.CampaignHandler.FifthMissionCompleted();
        }

        EnemySpawning.RemoveFromEnemyList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;

        UndergroundExplosionDevice.OnFire -= UED_FireUpdate;
    }

    private void UED_FireUpdate(object sender, UndergroundExplosionDevice.OnFireEventArgs args) {
        invulnerable = false;
        vulnerable.gameObject.SetActive(true);

        UI_Assistant.CreateNewGameMessage("Explosion succesful. The " + enemySO.NAME + " became vulnerable. You are now able to destroy it.", 10);
    }
}
