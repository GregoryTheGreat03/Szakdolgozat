using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour{

    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected float agroRange = 100;

    protected float health;
    protected float cooldown;
    protected GameObject target;
    protected float targetDistance;

    protected Vector3 wanderingPosition;
    protected Vector3 targetPosition;
    protected Vector3 waypoint;
    protected Func<Vector3> GetFollowPositionFunc;

    protected const float MAXIMUM_WANDERING_DISTANE = 4f;

    public virtual SaveData.EnemySaveData SaveEnemyData() {
        return new SaveData.EnemySaveData {
            enemySO = enemySO,
            position = transform.localPosition,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
        };
    }

    public virtual void LoadEnemyData(SaveData.EnemySaveData enemySaveData) {
        health = enemySaveData.health;
        cooldown = enemySaveData.cooldown;
        SetGetFollowPositionFunc(() => enemySaveData.nextFollowPosition);
    }

    public EnemySO GetEnemySO() {
        return enemySO;
    }

    public virtual void Awake() {
        wanderingPosition = transform.position;
        targetPosition = transform.position;
        health = enemySO.HEALTH;
        cooldown = 0;
        target = null;
        SetGetFollowPositionFunc(() => transform.position);
    }

    public void SetGetFollowPositionFunc(Func<Vector3> SetGetFollowPositionFunc) {
        GetFollowPositionFunc = SetGetFollowPositionFunc;
    }

    protected void HandleMovement() {
        waypoint = GetFollowPositionFunc();
        waypoint.z = transform.position.z;

        Vector3 moveDir = (waypoint - transform.position).normalized;
        float distance = Vector3.Distance(waypoint, transform.position);

        //setting the enemy movement
        if (distance > 0) {
            if (!(target is null) && !target.IsDestroyed() && Physics2D.Distance(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()).distance <= 0) {
                return;
            }

            Vector3 newPosition = transform.position + moveDir * enemySO.SPEED * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newPosition, waypoint);

            if (distanceAfterMoving > distance) {
                //overshot the target
                newPosition = waypoint;
            }

            transform.position = newPosition;
        }
    }

    protected virtual void WanderTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target == null) {
            if (Utils.PositionDistance(wanderingPosition, transform.position) == 0) {
                wanderingPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(0f, MAXIMUM_WANDERING_DISTANE), transform.position);
                //Debug.Log("Wandering position set. Distance: "+ Utils.PositionDistance(wanderingTargetPosition, transform.position));
            }

            SetGetFollowPositionFunc(() => wanderingPosition);
        }
    }

    protected void CooldownTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown < enemySO.COOLDOWN) {
            //Debug.Log("on cd:" + cooldown + "/" + enemySO.COOLDOWN);
            cooldown += TimeTickSystem.TICK_TIMER_MAX;
            if (cooldown > enemySO.COOLDOWN) {
                cooldown = enemySO.COOLDOWN;
            }
        }
    }

    protected void TargetSelectTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {

        if (target.IsDestroyed()) {
            target = null;
        }

        foreach (Drone drone in ResourceManager.GetDroneList()) {
            if (drone.IsDestroyed()) {
                continue;
            }
            if (target == null && (EnemySpawning.IsEnemyRush() || Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance <= agroRange)) {
                target = drone.gameObject;
                targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance;
            }
            if (targetDistance > Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance) {
                target = drone.gameObject;
                targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance;
            }
        }

        if (EnemySpawning.IsEnemyRush()) {
            targetDistance *= 10;
        }

        foreach (Building building in ResourceManager.GetBuildingList()) {
            if (building.IsDestroyed()) {
                continue;    
            }
            if (target == null && building.IsTargetable() && (EnemySpawning.IsEnemyRush() || Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance <= agroRange)) {
                target = building.gameObject;
                targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance;
            }
            if (building.IsTargetable() && targetDistance > Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance) {
                target = building.gameObject;
                targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance;
            }
        }

        /*
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, enemySO.AGRO_RANGE);

        foreach (Collider2D collider2D in colliderArray) {
            if (collider2D.TryGetComponent<Drone>(out Drone drone)) {
                if (target is null) { 
                    target = drone.gameObject;
                    targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance;
                }
                if (targetDistance > Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance) {
                    target = drone.gameObject;
                    targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), drone.GetComponent<Collider2D>()).distance;
                }
            }

            if (collider2D.TryGetComponent<Building>(out Building building)) {
                if (target is null && building.IsTargetable()) {
                    target = building.gameObject;
                    targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance;
                }
                if (building.IsTargetable() && targetDistance > Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance) {
                    target = building.gameObject;
                    targetDistance = Physics2D.Distance(gameObject.GetComponent<Collider2D>(), building.GetComponent<Collider2D>()).distance;
                }
            }
        }*/
    }

    protected void SelectTargetAsFollowPositionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target != null) {
            targetPosition = target.transform.position;
            SetGetFollowPositionFunc(() => targetPosition);
        }
    }

    protected virtual void AttackTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target != null && cooldown == enemySO.COOLDOWN) {
            
            if (target.TryGetComponent<Building>(out Building building) && building.GetHealth() > 0 && Physics2D.Distance(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()).distance <= 0) {
                building.ModifyHealth(-1 * enemySO.DAMAGE);

                //Debug.Log("damaged building " + enemySO.DAMAGE);

                cooldown = 0;
                if (target.IsDestroyed()) {
                    target = null;
                }
            } 

            else if (target.TryGetComponent<Drone>(out Drone drone) && drone.GetHealth() > 0 && Physics2D.Distance(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()).distance <= 0) {
                drone.ModifyHealth(-1 * enemySO.DAMAGE);

                //Debug.Log("damaged drone " + enemySO.DAMAGE);

                cooldown = 0;
                if (target.IsDestroyed()) {
                    target = null;
                }
            }
        }
    }

    protected void EndOfMission(object sender, MessageSystem.OnMissionOverEventArgs args) {
        Destroy(gameObject);
    }

    public virtual void ModifyHealth(float amount) {
        if (health + amount <= 0) {
            health = 0;
            cooldown = 0;
            Destroy(gameObject);
        } else if (health + amount >= enemySO.HEALTH) {
            health = enemySO.HEALTH;
        } else {
            health += amount;
        }
    }
}
