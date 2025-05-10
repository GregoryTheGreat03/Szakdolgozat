using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Building{

    [SerializeField] protected TurretSO turretSO;

    protected float cooldown;
    protected Enemy target;


    public override SaveData.BuildingSaveData SaveBuildingData() {
        return new SaveData.BuildingSaveData {
            buildingSO = buildingSO,
            position = transform.localPosition,
            health = health,
            targetable = targetable,
            constructionTimeRemaining = constructionTimeRemaining,
            cooldown = cooldown
        };
    }

    public override void LoadBuildingData(SaveData.BuildingSaveData buildingSaveData) {
        health = buildingSaveData.health;
        targetable = buildingSaveData.targetable;
        constructionTimeRemaining = buildingSaveData.constructionTimeRemaining;
        cooldown = buildingSaveData.cooldown;
        if (constructionTimeRemaining > 0) {
            this.GetComponentInChildren<Text>().enabled = false;
            TimeTickSystem.OnTick += ConstructionTickUpdate;
        }
    }

    public TurretSO GetTurretSO() { return turretSO; }

    public float GetDamage() {
        return turretSO.DAMAGE * TechSystem.GetDamageModifyer();
    }

    public float GetRange() {
        return turretSO.RANGE;
    }

    public float GetCooldown() { return cooldown; }

    public float GetCooldownMax() {
        return turretSO.COOLDOWN * TechSystem.GetAttackSpeedModifyer();
    }

    protected void CooldownTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown < GetCooldownMax() && constructionTimeRemaining <= 0) {
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

    protected void AttackTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (target != null && target.transform.position != Vector3.zero) {
            target.ModifyHealth(-1 * GetDamage());

            cooldown = 0;
            target = null;
        }
    }
}
