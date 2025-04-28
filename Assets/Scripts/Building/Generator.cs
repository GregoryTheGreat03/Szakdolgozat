using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : Building{

    [SerializeField] protected int energyGenerationAmount;
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
        } else if (targetable){
            ResourceManager.ModifyEnergy(-1 * energyGenerationAmount);
        }

        ResourceManager.AddToBuildingList(this);

        MessageSystem.OnMissionOver -= EndOfMission;
    }
}
