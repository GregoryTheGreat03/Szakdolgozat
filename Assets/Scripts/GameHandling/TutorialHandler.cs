using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static CampaignHandler;

public class TutorialHandler : MonoBehaviour {

    [SerializeField] GameHandler gameHandler;
    [SerializeField] HQ_AndDroneSpawning hQ_AndDroneSpawning;
    [SerializeField] EnemySpawning enemySpawning;
    [SerializeField] ResourceSpawning resourceSpawning;
    [SerializeField] UI_Assistant uI_Assistant;
    [SerializeField] CameraHandler cameraHandler;
    [SerializeField] UnitSelectionHandler unitSelectionHandler;
    [SerializeField] ResourceManager resourceManager;

    [SerializeField] private DroneSO workerSO;
    [SerializeField] private DroneSO patrollerSO;
    [SerializeField] private DroneSO protectorSO;

    [SerializeField] private BuildingSO generatorSO;
    [SerializeField] private BuildingSO droneFactorySO;

    [SerializeField] private EnemySO swarmerSO;

    [SerializeField] private ResourceSO coriumSO;

    private static int stepsCompleted = 0;
    private Worker tutorialWorker = null;
    private float timer;

    public void PlayTutorial() {
        stepsCompleted = 1;

        ResourceManager.SetEnergy(0);
        ResourceManager.SetCorium(0);
        ResourceManager.SetAntonium(0);

        hQ_AndDroneSpawning.SetStartingWorkerAmount(0);
        hQ_AndDroneSpawning.SetStartingPatrollerAmount(0);
        hQ_AndDroneSpawning.SetStartingProtectorAmount(0);
        hQ_AndDroneSpawning.SetStartingGuardianAmount(0);
        hQ_AndDroneSpawning.SetStartingEngineerAmount(0);
        hQ_AndDroneSpawning.SetStartingIndustrialMinerAmount(0);

        enemySpawning.SetStartingHiveAmount(0);
        enemySpawning.SetSecondsBetweenWaves(int.MaxValue);
        enemySpawning.SetMinWaveStrength(0);
        enemySpawning.SetMaxWaveStrength(0);
        enemySpawning.SetMinSpawningDistance(0);
        enemySpawning.SetMaxSpawningDistance(0);
        enemySpawning.SetEnemySpawningGeneratorList(new List<string> { "" });
        enemySpawning.SetEnemyRushTime(int.MaxValue);

        resourceSpawning.SetCoriumAmount(0);
        resourceSpawning.SetAntoniumAmount(0);
        resourceSpawning.SetMinDistance(0);
        resourceSpawning.SetMaxDistance(0);

        GameHandler.SetTimeScale(1);

        uI_Assistant.SetMissionBuilding(null);
        uI_Assistant.SetMissionObjectiveText(Texts.tutorialMissionObjective);
        cameraHandler.FocusOn(hQ_AndDroneSpawning.GetHQPosition());
        enemySpawning.InitializeMission();
        resourceSpawning.InitializeMission();
        hQ_AndDroneSpawning.InitializeMission();
        gameHandler.StartMission();

        UI_Assistant.ClearGameMessages();
        UI_Assistant.CreateNewGameMessage(Texts.tutorial1, int.MaxValue);
    }

    public static int GetStepsCompleted() {
        return stepsCompleted;
    }

    private void Update() {
        switch (stepsCompleted) {
            case 0: break;
            case 3:
            case 2:
            case 1: {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 4: {
                    Transform workerTransform = Instantiate(workerSO.prefab, new Vector3(0, 0, 0), transform.rotation);
                    workerTransform.GetComponent<Worker>().SetGetFollowPositionFunc(() => new Vector3(3, 3, 0));
                    workerTransform.GetComponent<Worker>().SetCapacity(10);
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial2, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 5: {
                    if (unitSelectionHandler.GetSelected().Count > 0 && unitSelectionHandler.GetSelected()[0].TryGetComponent<Worker>(out Worker worker)) {
                        stepsCompleted++;
                        tutorialWorker = worker;
                    }
                    break;
                }
            case 6: {
                    Instantiate(coriumSO.PREFAB, new Vector3(3, 10, 0), transform.rotation);
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial3, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 7: {
                    if (tutorialWorker.GetStoredResourceAmount() > 0) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 8: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial4, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 9: {
                    if (tutorialWorker.GetStoredResourceAmount() == tutorialWorker.GetCapacity()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 10: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial5, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 11: {
                    if (tutorialWorker.GetStoredResourceAmount() == 0) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 12: {
                    ResourceManager.ModifyCorium(60);
                    ResourceManager.ModifyAntonium(10);
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial6, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 13: {
                    if (ResourceManager.GetBuildingList().Select(x => x.GetBuildingSO().NAME == generatorSO.NAME).Where(x => x).Any()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 14: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial7, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 15: {
                    if (ResourceManager.GetBuildingList().Select(x => x.GetBuildingSO().NAME == generatorSO.NAME && x.GetConstructionTimeRemaining() <= 0 && x.IsTargetable()).Where(x => x).Any()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 16: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial8, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 17: {
                    if (ResourceManager.GetDroneList().Select(x => x.GetDroneSO().NAME == patrollerSO.NAME).Where(x => x).Any()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 18: {
                    Instantiate(swarmerSO.prefab, new Vector3(-30, -30, 0), transform.rotation);
                    Transform patrollerTransform = Instantiate(patrollerSO.prefab, new Vector3(1, 1, 0), transform.rotation);
                    patrollerTransform.GetComponent<Patroller>().SetGetFollowPositionFunc(() => new Vector3(1, 1, 0));
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial9, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 19: {
                    if (StatisticsTracker.GetEnemyKillCountInMission() > 0) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 20: {
                    ResourceManager.ModifyCorium(175);
                    ResourceManager.ModifyAntonium(2);
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial10, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 21: {
                    if (ResourceManager.GetBuildingList().Select(x => x.GetBuildingSO().NAME == droneFactorySO.NAME && x.GetConstructionTimeRemaining() <= 0 && x.IsTargetable()).Where(x => x).Any()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 22: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial11, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 23: {
                    if (ResourceManager.GetDroneList().Select(x => x.GetDroneSO().NAME == protectorSO.NAME).Where(x => x).Any()) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 24: {
                    Instantiate(swarmerSO.prefab, new Vector3(-35, -29, 0), transform.rotation);
                    Instantiate(swarmerSO.prefab, new Vector3(30, 32, 0), transform.rotation);
                    Instantiate(swarmerSO.prefab, new Vector3(-28, -33, 0), transform.rotation);
                    Instantiate(swarmerSO.prefab, new Vector3(30, -28, 0), transform.rotation);
                    Instantiate(swarmerSO.prefab, new Vector3(-39, -30, 0), transform.rotation);
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial12, int.MaxValue);
                    stepsCompleted++;
                    break;
                }
            case 25: {
                    if (StatisticsTracker.GetEnemyKillCountInMission() > 5) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 26: {
                    UI_Assistant.ClearGameMessages();
                    UI_Assistant.CreateNewGameMessage(Texts.tutorial13, int.MaxValue);
                    stepsCompleted++;
                    timer = 0;
                    break;
                }
            case 27: {
                    timer += Time.deltaTime;
                    if (timer > 10) {
                        stepsCompleted++;
                    }
                    break;
                }
            case 28: {
                    ResourceManager.ModifyCorium(-1 * ResourceManager.GetCorium());
                    ResourceManager.ModifyAntonium(-1 * ResourceManager.GetAntonium());
                    gameHandler.MissionOver();
                    stepsCompleted = 0;
                    break;
                }
        }
    }
}
