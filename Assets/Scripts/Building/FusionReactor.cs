using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FusionReactor : Building {

    [SerializeField] protected int energyGenerationAmount;

    [SerializeField] protected int explosionRadius;
    [SerializeField] protected int explosionDamage;
    protected override void ConstructionTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        if (constructionTimeRemaining >= 0) {
            constructionTimeRemaining -= TimeTickSystem.TICK_TIMER_MAX;
            ModifyHealth((TimeTickSystem.TICK_TIMER_MAX / GetConstructionTime()) * GetMaxHealth());
        }
        else {
            this.GetComponentInChildren<Text>().enabled = true;
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
            ResourceManager.ModifyEnergy(energyGenerationAmount);
        }
    }

    protected override void OnDestroy() {
        if (constructionTimeRemaining > 0) {
            TimeTickSystem.OnTick -= ConstructionTickUpdate;
        }
        else if (targetable) {
            ResourceManager.ModifyEnergy(-1 * energyGenerationAmount);

            // Explosion of the reactor
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (Collider2D collider2D in colliderArray) {
                if (collider2D.TryGetComponent<Enemy>(out Enemy enemy)) {
                    enemy.ModifyHealth(-1 * explosionDamage);
                    // Debug.Log("explosion: " + enemy.GetEnemySO().NAME + " " + explosionDamage);
                }
                else if (collider2D.TryGetComponent<Building>(out Building building)) {
                    building.ModifyHealth(-1 * explosionDamage);
                    // Debug.Log("explosion: " + building.GetBuildingSO().NAME + " " + explosionDamage);
                }
                else if (collider2D.TryGetComponent<Drone>(out Drone drone)) {
                    drone.ModifyHealth(-1 * explosionDamage);
                    // Debug.Log("explosion: " + drone.GetDroneSO().NAME + " " + explosionDamage);
                }
            }
        }

        MessageSystem.OnMissionOver -= EndOfMission;

        ResourceManager.RemoveFromBuildingList(this);
    }
}
