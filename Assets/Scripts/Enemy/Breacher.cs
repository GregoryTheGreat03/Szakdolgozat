using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Breacher : Enemy {

    [SerializeField] protected float regenerationPerSec;
    public override void Awake() {
        wanderingPosition = transform.position;
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

        MessageSystem.OnMissionOver -= EndOfMission;

        EnemySpawning.RemoveFromEnemyList(this);

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= TargetSelectTickUpdate;
        TimeTickSystem.OnTick -= SelectTargetAsFollowPositionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;
        TimeTickSystem.OnTick -= WanderTickUpdate;
    }

    protected virtual void RegenerationTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        ModifyHealth(regenerationPerSec * TimeTickSystem.TICK_TIMER_MAX);
    }
}
