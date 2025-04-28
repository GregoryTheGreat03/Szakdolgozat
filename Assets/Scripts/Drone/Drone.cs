using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : MonoBehaviour {

    [SerializeField] protected DroneSO droneSO;

    protected float health;
    protected float cooldown;

    protected Enemy target;

    protected Func<Vector3> GetFollowPositionFunc;

    protected Vector3 waypoint;
    protected float slowestSpeedPercentage = 0.25f;

    public virtual SaveData.DroneSaveData SaveDroneData() {
        return new SaveData.DroneSaveData { 
            droneSO = droneSO,
            position = transform.localPosition,
            health = health,
            cooldown = cooldown,
            nextFollowPosition = GetFollowPositionFunc(),
            };
    }

    public virtual void LoadDroneData(SaveData.DroneSaveData droneSaveData) {
        health = droneSaveData.health;
        cooldown = droneSaveData.cooldown;
        SetGetFollowPositionFunc(() => droneSaveData.nextFollowPosition);
    }

    public DroneSO GetDroneSO() { 
        return droneSO;
    }

    public float GetHealth() { 
        return health; 
    }

    public float GetDamage() {
        return droneSO.DAMAGE * TechSystem.GetDamageModifyer();
    }

    public virtual float GetRange() {
        return droneSO.RANGE;
    }

    public float GetMaxHealth() {
        return droneSO.HEALTH * TechSystem.GetDroneHealthModifyer();
    }

    public float GetMaxSpeed() {
        return droneSO.SPEED * TechSystem.GetSpeedModifyer();
    }

    public virtual float GetConstructionTime() {
        return droneSO.CONSTRUCTION_TIME * TechSystem.GetConstructionSpeedModifyer();
    }

    public Vector3 GetHealthBarPosition() {
        return droneSO.HEALTH_BAR_POSITION;
    }

    public Vector3 GetHealthBarSize() {
        return droneSO.HEALTH_BAR_SIZE;
    }

    public float GetCooldown() { 
        return cooldown; 
    }

    public float GetCooldownMax() {
        return droneSO.COOLDOWN * TechSystem.GetAttackSpeedModifyer();
    }

    public virtual void Awake() {
        health = GetMaxHealth();
        cooldown = 0;
    }

    public void SetGetFollowPositionFunc(Func<Vector3> SetGetFollowPositionFunc) {
        GetFollowPositionFunc = SetGetFollowPositionFunc;
    }

    protected virtual void HandleMovement() {
        waypoint = GetFollowPositionFunc();
        waypoint.z = transform.position.z;

        Vector3 moveDir = (waypoint - transform.position).normalized;
        float distance = Vector3.Distance(waypoint, transform.position);

        //setting the drone movement
        if (distance > 0) {
            Vector3 newPosition = transform.position + moveDir * Mathf.Clamp(distance, slowestSpeedPercentage, 1) * GetMaxSpeed() * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newPosition, waypoint);

            if (distanceAfterMoving > distance) {
                //overshot the target
                newPosition = waypoint;
            }

            transform.position = newPosition;
        }
    }

    protected void CooldownTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args){
        if (cooldown < GetCooldownMax()){
            cooldown += TimeTickSystem.TICK_TIMER_MAX;
            if (cooldown > GetCooldownMax()) {
                cooldown = GetCooldownMax();
            }
        }
    }

    protected virtual void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {
            target = null;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Enemy>(out Enemy enemy)) {
                    if (target is null) target = enemy;
                    if (Utils.PositionDistance(transform.position, target.transform.position) > Utils.PositionDistance(transform.position, enemy.transform.position)) {
                        target = enemy;
                    }
                }
            }
        }
    }

    protected virtual void AttackTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target != null && target.transform.position != Vector3.zero) {
            target.ModifyHealth(-1 * GetDamage());

            //Debug.Log("attacked target: " + target.GetEnemySO().NAME + "\n at: " + target.transform.position);

            cooldown = 0;
            target = null;
        }
    }

    protected void EndOfMission(object sender, MessageSystem.OnMissionOverEventArgs args) {
        Destroy(gameObject);
    }

    public virtual void ModifyHealth(float amount) {
        if (health + amount <= 0) {
            health = 0;
            Destroy(gameObject);
        } else if (health + amount >= GetMaxHealth()) {
            health = GetMaxHealth();
        } else {
            health += amount;
        }
    }
}
