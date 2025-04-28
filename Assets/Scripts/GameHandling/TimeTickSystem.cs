using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour{
    
    public class OnTickEventArgs : EventArgs {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;
    
    public const float TICK_TIMER_MAX = .2f;

    private static int tick;
    private float tickTimer;

    public static void ResetTimer() {
        tick = 0;
    }

    public static void Save(SaveData save) {
        save.tick = tick;
    }

    public static void Load(SaveData save) {
        tick = save.tick;
    }

    private void Update() {
        if (GameHandler.IsInMission()) {
            tickTimer += Time.deltaTime;
            while (tickTimer >= TICK_TIMER_MAX) {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
            }
        }
    }
}
