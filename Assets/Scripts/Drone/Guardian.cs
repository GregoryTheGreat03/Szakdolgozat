using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Guardian : Drone{
    [SerializeField] private int maximumTargetAmount;

    private List<Enemy> targetList = new List<Enemy>();

    public override void Awake() {
        health = GetMaxHealth();
        cooldown = 0;
        ResourceManager.AddToDroneList(this);

        MessageSystem.OnMissionOver += EndOfMission;

        TimeTickSystem.OnTick += CooldownTickUpdate;
        TimeTickSystem.OnTick += DetectionTickUpdate;
        TimeTickSystem.OnTick += AttackTickUpdate;
    }

    public void Update() {
        HandleMovement();
    }

    private void OnDestroy() {
        ResourceManager.RemoveFromDroneList(this);

        MessageSystem.OnMissionOver -= EndOfMission;

        TimeTickSystem.OnTick -= CooldownTickUpdate;
        TimeTickSystem.OnTick -= DetectionTickUpdate;
        TimeTickSystem.OnTick -= AttackTickUpdate;

        ResourceManager.ModifyEnergy(droneSO.ENERGY_COST);
    }


    protected override void DetectionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (cooldown == GetCooldownMax()) {

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, GetRange());

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Enemy>(out Enemy enemy) && enemy.transform.position != Vector3.zero) {

                    if (targetList.Count < maximumTargetAmount) {
                        targetList.Add(enemy);
                    } else {
                        targetList.Sort((target1, target2) => {
                            return Utils.PositionDistance(target1.transform.position, transform.position).CompareTo(Utils.PositionDistance(target2.transform.position, transform.position)); 
                            });

                        if (Utils.PositionDistance(transform.position, targetList[maximumTargetAmount-1].transform.position) > Utils.PositionDistance(transform.position, enemy.transform.position)) {
                            targetList.RemoveAt(maximumTargetAmount-1);
                            targetList.Add(enemy);
                        }
                        
                    }
                }
            }
        }
    }

    protected override void AttackTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (targetList.Count > 0) {
            foreach (Enemy target in targetList) {
                target.ModifyHealth(-1 * GetDamage());
            }
            targetList.Clear();
            cooldown = 0;
        }
    }
}
