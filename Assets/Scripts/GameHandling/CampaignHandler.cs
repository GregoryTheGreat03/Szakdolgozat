using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CampaignHandler : MonoBehaviour {

    [SerializeField] GameHandler gameHandler;
    [SerializeField] StoryWriter storyWriter;
    [SerializeField] ResourceManager resourceManager;
    [SerializeField] HQ_AndDroneSpawning hQ_AndDroneSpawning;
    [SerializeField] EnemySpawning enemySpawning;
    [SerializeField] ResourceSpawning resourceSpawning;
    [SerializeField] CameraHandler cameraHandler;
    [SerializeField] UI_Assistant uI_Assistant;
    [SerializeField] ConfirmActionHandler confirmActionHandler;
    [SerializeField] TutorialHandler tutorialHandler;

    [SerializeField] OnShipAssemblySO hQ_SO;
    [SerializeField] OnShipAssemblySO workerSO;
    [SerializeField] OnShipAssemblySO patrollerSO;

    [SerializeField] BuildingSO thumperSO;
    [SerializeField] BuildingSO undergroundExplosionDeviceSO;

    [SerializeField] int minutesToComplete4thMission;

    public enum Missions {
        planetfall,
        xenohunt,
        farsight,
        hellgate,
        excalibur
    }

    public bool enemyDatabankUnlocked { get; set; }
    public bool techTreeUnlocked { get; set; }

    private Missions chosenMission;

    private HashSet<string> buildingsOnShip;
    private HashSet<Missions> unlockedMissions = new HashSet<Missions>();
    private HashSet<string> unlockedBuildings = new HashSet<string>();
    private Dictionary<Missions, List<string>> missionSettingDatabase;

    private readonly CultureInfo culture = new CultureInfo("hu-HU");

    void Start() {
        ReferenceList.CampaignHandler = this;
    }

    public void Save(SaveData save) {
        if (GameHandler.IsSandboxMission()) {
            return;
        }
        save.enemyDatabankUnlocked = enemyDatabankUnlocked;
        save.techTreeUnlocked = techTreeUnlocked;
        save.buildingsOnShip = new List<string>(buildingsOnShip);
        save.unlockedBuildings = new List<string>(unlockedBuildings);
        save.unlockedMissions = new List<Missions>(unlockedMissions);
    }

    public void Load(SaveData save) {
        if (save.sandboxMission) {
            return;
        }
        enemyDatabankUnlocked = save.enemyDatabankUnlocked;
        techTreeUnlocked = save.techTreeUnlocked;
        buildingsOnShip = new HashSet<string>(save.buildingsOnShip);
        unlockedBuildings = new HashSet<string>(save.unlockedBuildings);
        unlockedMissions = new HashSet<Missions>(save.unlockedMissions);
        IitializeMissionDificultySettings();
    }

    public void StartCampaign() {
        enemyDatabankUnlocked = false;
        techTreeUnlocked = false;

        buildingsOnShip = new HashSet<string>();

        // string list contains the parameters of the mission: starting hive amount, seconds between waves, min wave strength, max wave strength,
        // min spawning distance, max spawning distance, enemy unit list, enemy rush time, corium field amount, antonium field amount, min resource distance, max resource distance, mission building
        IitializeMissionDificultySettings();
        unlockedMissions.Add(Missions.planetfall);
        
        AddUnlockedBuilding(Texts.HQ_ConstructionName);
        AddUnlockedBuilding(Texts.DroneFactoryName);
        AddBuildingOnShip(Texts.HQ_ConstructionName);

        TechSystem.ClearTechs();

        resourceManager.ClearDronesOnShip();
        resourceManager.SetShipResources(0, 0, 0);
        resourceManager.AddDronesOnShip(hQ_SO.NAME, 7);
        resourceManager.AddDronesOnShip(workerSO.NAME, 14);
        resourceManager.AddDronesOnShip(patrollerSO.NAME, 7);

        gameHandler.Story();
        StoryWriter.ClearLists();
        storyWriter.WriteStory(
            Texts.storyPrologue1Title,
            Texts.storyPrologue1,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyPrologue2,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.player,
            Texts.storyPrologue3,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyPrologue4,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.player,
            Texts.storyPrologue5,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyPrologue6,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyPrologue7,
            0.015f
            );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyPrologue8,
            0.015f
            );

        MessageSystem.OnBetweenMissionsDisplayUpdate += TutorialQuestion;
    }

    public HashSet<Missions> GetUnlockedMissions() {
        return unlockedMissions;
    }

    public HashSet<string> GetUnlockedBuildings() {
        return unlockedBuildings;
    }

    public void AddUnlockedBuilding(string buildingName) {
        unlockedBuildings.Add(buildingName);
    }

    public void AddBuildingOnShip(string name) {
        buildingsOnShip.Add(name);

        if (name == Texts.DroneFactoryName) {
            AddUnlockedBuilding(Texts.HeavyFactoryName);
        }
    }

    public HashSet<string> GetBuildingsOnShip() {
        return buildingsOnShip;
    }

    public void SetChosenMission(Missions mission) {
        chosenMission = mission;
    }

    public void SetupChosenMission(int startingWorker, int startingPatroller, int startingProtector, int startingGuardian, int startingEngineer, int startingIndustrialMiner) {
        List<string> missionParams = missionSettingDatabase.GetValueOrDefault(chosenMission);

        ResourceManager.SetEnergy(0);
        ResourceManager.SetCorium(0);
        ResourceManager.SetAntonium(0);

        hQ_AndDroneSpawning.SetStartingWorkerAmount(startingWorker);
        hQ_AndDroneSpawning.SetStartingPatrollerAmount(startingPatroller);
        hQ_AndDroneSpawning.SetStartingProtectorAmount(startingProtector);
        hQ_AndDroneSpawning.SetStartingGuardianAmount(startingGuardian);
        hQ_AndDroneSpawning.SetStartingEngineerAmount(startingEngineer);
        hQ_AndDroneSpawning.SetStartingIndustrialMinerAmount(startingIndustrialMiner);

        enemySpawning.SetStartingHiveAmount(int.Parse(missionParams[0]));
        enemySpawning.SetSecondsBetweenWaves(float.Parse(missionParams[1], culture.NumberFormat));
        enemySpawning.SetMinWaveStrength(float.Parse(missionParams[2], culture.NumberFormat));
        enemySpawning.SetMaxWaveStrength(float.Parse(missionParams[3], culture.NumberFormat));
        enemySpawning.SetMinSpawningDistance(float.Parse(missionParams[4], culture.NumberFormat));
        enemySpawning.SetMaxSpawningDistance(float.Parse(missionParams[5], culture.NumberFormat));
        enemySpawning.SetEnemySpawningGeneratorList(missionParams[6].Trim().ToLower().Split(' ').ToList());
        enemySpawning.SetEnemyRushTime(float.Parse(missionParams[7], culture.NumberFormat));

        resourceSpawning.SetCoriumAmount(int.Parse(missionParams[8]));
        resourceSpawning.SetAntoniumAmount(int.Parse(missionParams[9]));
        resourceSpawning.SetMinDistance(float.Parse(missionParams[10], culture.NumberFormat));
        resourceSpawning.SetMaxDistance(float.Parse(missionParams[11], culture.NumberFormat));

        GameHandler.SetTimeScale(1);

        if (chosenMission == Missions.excalibur) {
            enemySpawning.SpawnHivemind();
        }

        uI_Assistant.SetMissionBuilding(missionParams[12] == thumperSO.NAME ? thumperSO : (missionParams[12] == undergroundExplosionDeviceSO.NAME ? undergroundExplosionDeviceSO : null));
        cameraHandler.FocusOn(hQ_AndDroneSpawning.GetHQPosition());
        enemySpawning.InitializeMission();
        resourceSpawning.InitializeMission();
        hQ_AndDroneSpawning.InitializeMission();
        TimeTickSystem.ResetTimer();
        gameHandler.StartMission();
    }

    public void FirstMissionCompletedCheck() {
        Debug.Log("1st mission check");
        if (!unlockedMissions.Contains(Missions.xenohunt) && ResourceManager.GetCoriumOnShip() > 500 && ResourceManager.GetAntoniumOnShip() > 100) {
            gameHandler.Story();
            StoryWriter.ClearLists();
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked1,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.player,
                Texts.storyMission2Unlocked2,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked3,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.player,
                Texts.storyMission2Unlocked4,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked5,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.player,
                Texts.storyMission2Unlocked6,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked7,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.player,
                Texts.storyMission2Unlocked8,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked9,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.player,
                Texts.storyMission2Unlocked10,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked11,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission2Unlocked12,
                0.015f
            );

            resourceManager.SetShipResources(ResourceManager.GetCoriumOnShip() - 500, ResourceManager.GetAntoniumOnShip() - 100, ResourceManager.GetSampleOnShip());

            unlockedBuildings.Add(Texts.ScienceLabName);
            unlockedMissions.Add(Missions.xenohunt);

            Debug.Log("1st mission complete");
        }
    }

    public void SecondMissionCompletedCheck() {
        if (!unlockedMissions.Contains(Missions.farsight) && buildingsOnShip.Contains(Texts.ScienceLabName)) {
            gameHandler.Story();
            StoryWriter.ClearLists();
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission3Unlocked1,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission3Unlocked2,
                0.015f
            );

            enemyDatabankUnlocked = true;
            unlockedBuildings.Add(Texts.NetworkExpansionName);
            unlockedMissions.Add(Missions.farsight);
        }
    }

    public void ThirdMissionCompletedCheck() {
        if (!unlockedMissions.Contains(Missions.hellgate) && buildingsOnShip.Contains(Texts.NetworkExpansionName)) {
            gameHandler.Story();
            StoryWriter.ClearLists();
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission4Unlocked1,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission4Unlocked2,
                0.015f
            );
            storyWriter.WriteStory(
                Texts.aida,
                Texts.storyMission4Unlocked3,
                0.015f
            );

            techTreeUnlocked = true;
            unlockedMissions.Add(Missions.hellgate);
        }
    }

    public void FourthMissionCompletedCheck(int ticksWhileThumperActive) {
        if (!unlockedMissions.Contains(Missions.excalibur) && ticksWhileThumperActive * TimeTickSystem.TICK_TIMER_MAX / 60 >= minutesToComplete4thMission) {
            UI_Assistant.CreateNewGameMessage(Texts.mission4Completed, 15);

            MessageSystem.OnBetweenMissionsDisplayUpdate += WriteFourthMissionStory;

            unlockedMissions.Add(Missions.excalibur);
        }
    }

    private void WriteFourthMissionStory(object sender, MessageSystem.BetweenMissionsDisplayUpdateEventArgs args) {
        gameHandler.Story();
        StoryWriter.ClearLists();
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyMission5Unlocked1,
            0.015f
        );
        storyWriter.WriteStory(
            Texts.player,
            Texts.storyMission5Unlocked2,
            0.015f
        );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyMission5Unlocked3,
            0.015f
        );
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyMission5Unlocked4,
            0.015f
        );
        MessageSystem.OnBetweenMissionsDisplayUpdate -= WriteFourthMissionStory;
    }

    public void FifthMissionCompleted() {
        MessageSystem.OnBetweenMissionsDisplayUpdate += WriteFifthMissionStory;
    }

    private void WriteFifthMissionStory(object sender, MessageSystem.BetweenMissionsDisplayUpdateEventArgs args) {
        gameHandler.Story();
        StoryWriter.ClearLists();
        storyWriter.WriteStory(
            Texts.aida,
            Texts.storyEndOfGame1,
            0.015f
        );
        storyWriter.WriteStory(
            Texts.titleEndOfGame2,
            Texts.storyEndOfGame2,
            0.015f
        );
        storyWriter.WriteStory(
            Texts.titleEndOfGame2,
            Texts.storyEndOfGame3,
            0.015f
        );
        MessageSystem.OnBetweenMissionsDisplayUpdate -= WriteFifthMissionStory;
    }

    private void TutorialQuestion(object sender, MessageSystem.BetweenMissionsDisplayUpdateEventArgs args) {
        confirmActionHandler.ConfirmAction(() => {
            tutorialHandler.PlayTutorial();
        }, "We recommend the tutorial simulation, to learn operating the system.", "Play tutorial?", "no", "yes");

        MessageSystem.OnBetweenMissionsDisplayUpdate -= TutorialQuestion;
    }

    private void IitializeMissionDificultySettings() {
        if (missionSettingDatabase == null) {
            missionSettingDatabase = new Dictionary<Missions, List<string>>();
        }
        missionSettingDatabase.Clear();
        missionSettingDatabase.Add(Missions.planetfall, new List<string> { "0", "90", "0,5", "0,8", "50", "120", "s s s s s s s w w w w w m m m b", "30", "40", "10", "10", "100", "" });
        missionSettingDatabase.Add(Missions.xenohunt, new List<string> { "3", "60", "0,7", "1,5", "50", "120", "s s s s s s s w w w w w m m m m b b b h", "30", "65", "20", "10", "110", "" });
        missionSettingDatabase.Add(Missions.farsight, new List<string> { "5", "45", "0,9", "1,7", "40", "105", "s s s s s s s w w w w w w m m m m m b b b b h h", "30", "120", "40", "10", "120", "" });
        missionSettingDatabase.Add(Missions.hellgate, new List<string> { "1", "70", "0,4", "0,8", "60", "120", "s s s s s s s w w w m m b", "60", "50", "15", "20", "100", thumperSO.NAME });
        missionSettingDatabase.Add(Missions.excalibur, new List<string> { "8", "60", "2", "4", "60", "100", "s s s s s s s w w w w w w m m m m m b b b b h h", "60", "70", "25", "10", "120", undergroundExplosionDeviceSO.NAME });
    }
}
