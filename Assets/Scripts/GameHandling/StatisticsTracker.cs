using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatisticsTracker : MonoBehaviour {

    private static int enemyKillCountAltogether = 0;
    private static int enemyKillCountInMission = 0;
    private static float timeElapsed = 0;

    public static void Save(SaveData save) {
        save.enemyKillCountInMission = enemyKillCountInMission;
        save.enemyKillCountAltogether = enemyKillCountAltogether;
        save.timeElapsed = timeElapsed;
    }

    public static void Load(SaveData save) {
        enemyKillCountInMission = save.enemyKillCountInMission;
        enemyKillCountAltogether = save.enemyKillCountAltogether;
        timeElapsed = save.timeElapsed;
    }

    private void Start() {
        TimeTickSystem.OnTick += TimeTickUpdate;
    }

    private void TimeTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        timeElapsed = TimeTickSystem.TICK_TIMER_MAX * args.tick;
    }

    public static void EnemyKillCountIncrease() {
        enemyKillCountInMission++;
    }

    public static int GetEnemyKillCountInMission() {
        return enemyKillCountInMission;
    }

    public static void SaveStatistics() {
        enemyKillCountAltogether += enemyKillCountInMission;
        enemyKillCountInMission = 0;
    }

    public static string GetTimeElapsed() {
        int hours = (int)(timeElapsed / 3600);
        int minutes = (int)(timeElapsed % 3600 / 60);
        int seconds = (int)(timeElapsed % 60);
        return (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes) + ":" + (seconds < 10 ? "0" + seconds : seconds);
    }
}
