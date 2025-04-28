using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Melter : Enemy{
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

    protected override void AttackTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target != null && cooldown == enemySO.COOLDOWN) {

            if (target.TryGetComponent<Building>(out Building building) && building.GetHealth() > 0 && Physics2D.Distance(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()).distance <= enemySO.RANGE) {
                Debug.Log("melter attacked building");
                building.ModifyHealth(-1 * enemySO.DAMAGE);
                ModifyHealth(-1 * enemySO.HEALTH);
            }

            else if (target.TryGetComponent<Drone>(out Drone drone) && drone.GetHealth() > 0 && Physics2D.Distance(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()).distance <= enemySO.RANGE) {
                Debug.Log("melter attacked drone");
                drone.ModifyHealth(-1 * enemySO.DAMAGE);
                ModifyHealth(-1 * enemySO.HEALTH);
            }
        }
    }
}
