using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Enemy{
    public override void Awake() {
        health = enemySO.HEALTH;
        cooldown = 0;
        target = null;
        SetGetFollowPositionFunc(() => transform.position);

        EnemySpawning.AddToEnemyList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += TargetSelectTickUpdate;
        TimeTickSystem.OnTick += SelectTargetAsFollowPositionTickUpdate;
        TimeTickSystem.OnTick += AttackTickUpdate;
        TimeTickSystem.OnTick += WanderTickUpdate;
    }

    public void Update() {
        HandleMovement();
    }

    private void OnDestroy() {
        if (health <= 0) {
            StatisticsTracker.EnemyKillCountIncrease();
        }

        EnemySpawning.RemoveFromEnemyList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= TargetSelectTickUpdate;
        TimeTickSystem.OnTick -= SelectTargetAsFollowPositionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;
        TimeTickSystem.OnTick -= WanderTickUpdate;
    }
}
