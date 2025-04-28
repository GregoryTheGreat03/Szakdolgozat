using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TimeTickSystem;

public class MessageSystem : MonoBehaviour{

    public class OnMissionOverEventArgs : EventArgs {
    }

    public static event EventHandler<OnMissionOverEventArgs> OnMissionOver;

    public void MissionOver() {
        if (OnMissionOver != null) OnMissionOver(this, new OnMissionOverEventArgs { });
    }

    public class BetweenMissionsDisplayUpdateEventArgs : EventArgs {
    }

    public static event EventHandler<BetweenMissionsDisplayUpdateEventArgs> OnBetweenMissionsDisplayUpdate;

    public void BetweenMissionsDisplayUpdate() {
        if (OnBetweenMissionsDisplayUpdate != null) OnBetweenMissionsDisplayUpdate(this, new BetweenMissionsDisplayUpdateEventArgs { });
    }


    private void Start() {
        ReferenceList.MessageSystem = this;
    }
}
