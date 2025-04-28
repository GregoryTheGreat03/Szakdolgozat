using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour{

    [SerializeField] private ResourceSO resourceSO;

    protected float amount;
    protected float miningTimeDone;

    public SaveData.ResourceSaveData SaveResourceData() {
        return new SaveData.ResourceSaveData { 
                position = transform.localPosition,
                resourceSO = resourceSO,
                amount = amount,
                miningTimeDone = miningTimeDone
            };
    }

    public void LoadResourceData(SaveData.ResourceSaveData resourceSaveData) {
        amount = resourceSaveData.amount;
        miningTimeDone = resourceSaveData.miningTimeDone;
    }

    public void Awake(){
        amount = resourceSO.AMOUNT;
        miningTimeDone = resourceSO.MINING_TIME;

        MessageSystem.OnMissionOver += EndOfMission;
    }

    public ResourceSO GetResourceSO() {
        return resourceSO;
    }

    protected void EndOfMission(object sender, MessageSystem.OnMissionOverEventArgs args) {
        Destroy(gameObject);
    }

    public float MineResourceAmount(float amount, float efficiency) {
        if (miningTimeDone > 0) {
            miningTimeDone -= efficiency;
            return 0;
        }
        else {

            if (this.amount - amount <= 0) {
                float remaining = this.amount;
                this.amount = 0;
                UI_Assistant.CreateNewGameMessage(resourceSO.NAME + " resource field depleted.", 5);
                Destroy(gameObject);
                return remaining;
            }
            this.amount -= amount;
            miningTimeDone = resourceSO.MINING_TIME;
            return amount;
        }
    }

    private void OnDestroy() {
        ResourceSpawning.RemoveFromResourceList(this);

        MessageSystem.OnMissionOver -= EndOfMission;
    }
}
