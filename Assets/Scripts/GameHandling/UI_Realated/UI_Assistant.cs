using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Assistant : MonoBehaviour {

    // classes
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private CameraHandler cameraHandler;
    [SerializeField] private UnitSelectionHandler unitSelectionHandler;
    [SerializeField] private BuildingGridSystem buildingGridSystem;
    [SerializeField] private HQ_AndDroneSpawning hQ_AndDroneSpawning;
    [SerializeField] private ResourceSpawning resourceSpawning;
    [SerializeField] private EnemySpawning enemySpawning;
    [SerializeField] private ConfirmActionHandler confirmActionHandler;
    [SerializeField] private PopupMessageHandler popupMessageHandler;
    [SerializeField] private StringInputHandler stringInputHandler;
    [SerializeField] private CampaignHandler campaignHandler;
    [SerializeField] private NextClickLocator nextClickLocator;
    [SerializeField] private MovingDescriptionHandler movingDescriptionHandler;

    // prefabs
    [SerializeField] private Transform buttonPrefab;
    [SerializeField] private Transform fileButtonPrefab;

    // Main menu visuals
    [SerializeField] private GameObject mainMenuDisplay;

    [SerializeField] private Button newSandboxMissionButton;
    [SerializeField] private Button newCampaignButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitGameButton;

    // Pause game visuals
    [SerializeField] private GameObject pauseGameDisplay;

    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private Button pauseSaveButton;
    [SerializeField] private Button pauseLoadButton;
    [SerializeField] private Button pauseEndMissionButton;
    [SerializeField] private Button pauseBackToMainMenuButton;
    [SerializeField] private Button pauseExitGameButton;

    [SerializeField] private GameObject saveLoadScrollView;
    [SerializeField] private GameObject saveLoadScrollViewContent;
    [SerializeField] private Button closeScrollViewButton;


    // Sandbox mission setup visuals
    [SerializeField] private GameObject sandboxMissionSetupDisplay;

    [SerializeField] private InputField startingCoriumInputField;
    [SerializeField] private InputField startingAntoniumInputField;

    [SerializeField] private InputField startingWorkerInputField;
    [SerializeField] private InputField startingPatrollerInputField;
    [SerializeField] private InputField startingProtectorInputField;
    [SerializeField] private InputField startingGuardianInputField;
    [SerializeField] private InputField startingEngineerInputField;
    [SerializeField] private InputField startingIndustrialMinerInputField;

    [SerializeField] private InputField startingHiveAmountInputField;
    [SerializeField] private InputField secondsBetweenWavesInputField;
    [SerializeField] private InputField minWaveStrengthInputField;
    [SerializeField] private InputField maxWaveStrengthInputField;
    [SerializeField] private InputField minSpawnDistanceInputField;
    [SerializeField] private InputField maxSpawnDistanceInputField;
    [SerializeField] private InputField enemyListInputField;

    [SerializeField] private InputField spawnedCoriumAmountInputField;
    [SerializeField] private InputField spawnedAntoniumAmountInputField;
    [SerializeField] private InputField minResourceDistanceInputField;
    [SerializeField] private InputField maxResourceDistanceInputField;

    [SerializeField] private InputField gameSpeedInputField;

    [SerializeField] private GameObject sandboxMissionSetupErrorDisplay;

    [SerializeField] private Button startMissionButton;
    [SerializeField] private Button backToMainMenuButton;

    // story visuals
    [SerializeField] private GameObject storyDisplay;

    // between missions visuals
    [SerializeField] private GameObject betweenMissionsDisplay;

    // in mission visuals
    [SerializeField] private GameObject inMissionDispaly;

    [SerializeField] private GameObject gameMessageDispaly;
    [SerializeField] private GameObject resourceDisplay;
    [SerializeField] private GameObject timeDisplay;

    [SerializeField] private GameObject selectedUnitDisplay;
    [SerializeField] private Button selectedDrone1Button;
    [SerializeField] private Button selectedDrone2Button;

    [SerializeField] private GameObject selectedBuildingNameDisplay;
    [SerializeField] private GameObject selectedBuildingInformationDisplay;
    [SerializeField] private Button assembleButton1;
    [SerializeField] private GameObject assemble1Information;
    [SerializeField] private Button assembleButton2;
    [SerializeField] private GameObject assemble2Information;
    [SerializeField] private GameObject assemblingAndQueueInformation;
    [SerializeField] private Button activateMissionBuildingButton;

    [SerializeField] private Button closeSelectedDisplayButton;

    [SerializeField] private Button baseComponentMenuButton;
    [SerializeField] private GameObject baseComponentMenuText;
    [SerializeField] private Button buildWallButton;
    [SerializeField] private Button buildGeneratorButton;
    [SerializeField] private Button buildFusionReactorButton;
    [SerializeField] private Button buildMissionBuildingButton;

    [SerializeField] private Button factoryMenuButton;
    [SerializeField] private GameObject factoryMenuText;
    [SerializeField] private Button buildFactoryButton;
    [SerializeField] private Button buildHeavyFactoryButton;

    [SerializeField] private Button turretMenuButton;
    [SerializeField] private GameObject turretMenuText;
    [SerializeField] private Button buildMachineGunTurretButton;
    [SerializeField] private Button buildHeavyTurretButton;
    [SerializeField] private Button buildRailCannonButton;

    [SerializeField] private Button backButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameObject missionObjectiveDisplay;

    // mission over visuals
    [SerializeField] private GameObject missionOverDispaly;
    [SerializeField] private GameObject missionOverStatisticsDispaly;
    [SerializeField] private Button missionOverBackToShipButton;

    // Scriptable Objects
    [SerializeField] private BuildingSO wall;
    [SerializeField] private BuildingSO generator;
    [SerializeField] private BuildingSO fusionReactor;
    [SerializeField] private BuildingSO droneFactory;
    [SerializeField] private BuildingSO heavyFactory;
    [SerializeField] private BuildingSO machineGunTurret;
    [SerializeField] private BuildingSO heavyTurret;
    [SerializeField] private BuildingSO railCannon;

    private Text gameMessageText;
    private static Dictionary<string, float> gameMessages = new Dictionary<string, float>();
    
    private List<Transform> createdUI_Elements;
    private Text resourceText;
    private Text timeText;
    private Text selectedUnitsText;
    private bool missionBuilding = false;

    private enum BuildingMenu {
        outerMenu,
        baseComponentMenu,
        factoryMenu,
        turretMenu
    }

    private static BuildingMenu buildingMenu;

    public static void SetBuildingMenuToOuterMenu() {
        buildingMenu = BuildingMenu.outerMenu;
    }

    public static void CreateNewGameMessage(string message, int lifetime) {
        if (gameMessages.ContainsKey(message)) {
            gameMessages[message] = lifetime;
        }
        else {
            gameMessages.Add(message, lifetime);
        }
    }

    public static void ClearGameMessages() {
        gameMessages.Clear();
    }

    public void SetMissionObjectiveText(string objective) {
        if (objective != "") {
            objective = objective.Insert(objective.IndexOf(":") + 1, "\r\n");
        }

        missionObjectiveDisplay.GetComponentInChildren<Text>().text = objective;
        missionObjectiveDisplay.SetActive(false);
    }

    public void SetMissionBuilding(BuildingSO buildingSO = null) {
        buildMissionBuildingButton.onClick.RemoveAllListeners();

        if (buildingSO != null) {
            buildMissionBuildingButton.GetComponentInChildren<Text>().text = "build " + buildingSO.NAME;
            buildMissionBuildingButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(buildingSO));

            EventTrigger trigger = buildMissionBuildingButton.GetComponent<EventTrigger>();
            trigger.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { movingDescriptionHandler.WriteStatistics(buildingSO); });
            trigger.triggers.Add(entry);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((data) => { movingDescriptionHandler.Disappear(); });
            trigger.triggers.Add(entry2);

            missionBuilding = true;
        }
        else {
            missionBuilding = false;
        }
    }

    private void Awake() {
        gameMessages = new Dictionary<string, float>();
        createdUI_Elements = new List<Transform>();

        // main menu buttons
        newSandboxMissionButton.onClick.AddListener(() => { gameHandler.SandboxMissionSetup(); TechSystem.ClearTechs(); });
        newCampaignButton.onClick.AddListener(() => campaignHandler.StartCampaign());
        loadButton.onClick.AddListener(() => { gameHandler.GamePauseSwitch(); LoadLoadDisplay(); });
        exitGameButton.onClick.AddListener(() => Application.Quit());

        // pause game buttons
        pauseResumeButton.onClick.AddListener(() => gameHandler.GamePauseSwitch());
        pauseSaveButton.onClick.AddListener(() => LoadSaveDisplay());
        pauseLoadButton.onClick.AddListener(() => LoadLoadDisplay());
        closeScrollViewButton.onClick.AddListener(() => CloseSaveLoadScrollView());
        pauseEndMissionButton.onClick.AddListener(() => confirmActionHandler.ConfirmAction(() => gameHandler.MissionOver(), "This action will end your mission, destroying your HQ."));
        pauseBackToMainMenuButton.onClick.AddListener(() => confirmActionHandler.ConfirmAction(() => gameHandler.MainMenu()));
        pauseExitGameButton.onClick.AddListener(() => confirmActionHandler.ConfirmAction(() => Application.Quit(), "Are you sure you want to quit from the game?"));
        saveLoadScrollView.SetActive(false);

        // sandbox mission setup buttons
        startMissionButton.onClick.AddListener(() => CheckSandboxMissionInputFields());
        backToMainMenuButton.onClick.AddListener(() => gameHandler.MainMenu());

        // in mission buttons and textbox setup
        gameMessageText = gameMessageDispaly.GetComponent<Text>();
        resourceText = resourceDisplay.GetComponent<Text>();
        timeText = timeDisplay.GetComponent<Text>();
        selectedUnitsText = selectedUnitDisplay.GetComponent<Text>();

        buildingMenu = BuildingMenu.outerMenu;
        baseComponentMenuButton.onClick.AddListener(() => buildingMenu = BuildingMenu.baseComponentMenu);
        factoryMenuButton.onClick.AddListener(() => buildingMenu = BuildingMenu.factoryMenu);
        turretMenuButton.onClick.AddListener(() => buildingMenu = BuildingMenu.turretMenu);

        closeSelectedDisplayButton.onClick.AddListener(() => unitSelectionHandler.ClearSelected());
        backButton.onClick.AddListener(() => buildingMenu = BuildingMenu.outerMenu);
        cancelButton.onClick.AddListener(() => buildingGridSystem.CancelBuilding());

        buildWallButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(wall));
        buildGeneratorButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(generator));
        buildFusionReactorButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(fusionReactor));
        buildFactoryButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(droneFactory));
        buildHeavyFactoryButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(heavyFactory));
        buildMachineGunTurretButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(machineGunTurret));
        buildHeavyTurretButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(heavyTurret));
        buildRailCannonButton.onClick.AddListener(() => buildingGridSystem.BuildBuilding(railCannon));

        // misssion over button setup
        missionOverBackToShipButton.onClick.AddListener(() => { 
            if (GameHandler.IsSandboxMission()) { 
                gameHandler.MainMenu();
            } else {
                gameHandler.BetweenMissions();
            }
        });

        confirmActionHandler.gameObject.SetActive(true);
        popupMessageHandler.gameObject.SetActive(true);
        stringInputHandler.gameObject.SetActive(true);
        missionObjectiveDisplay.SetActive(false);

        TimeTickSystem.OnTick += ResourceDisplayTickUpdate;
        TimeTickSystem.OnTick += GameMessageDisplayTickUpdate;

        HideEverythingOnTheRightDisplayPanel();
    }

    private void Update() {
        timeText.text = StatisticsTracker.GetTimeElapsed();

        if (Input.GetKeyDown(KeyCode.M)) {
            missionObjectiveDisplay.SetActive(missionObjectiveDisplay.GetComponentInChildren<Text>().text != "" ? !missionObjectiveDisplay.activeSelf : false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gameHandler.GetGameState() != GameHandler.GameState.missionOver) {
            gameHandler.GamePauseSwitch();
            CloseSaveLoadScrollView();
        }

        ActivateAppropriateDisplays();

        switch (gameHandler.GetGameState()) {
            case GameHandler.GameState.gamePaused: {
                    pauseGameDisplay.SetActive(true);
                    if (GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.mainMenu || GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.sandboxMissionSetup) {
                        pauseResumeButton.gameObject.SetActive(false);
                        pauseSaveButton.gameObject.SetActive(false);
                        pauseEndMissionButton.gameObject.SetActive(false);
                        pauseSettingsButton.gameObject.SetActive(true);

                        pauseBackToMainMenuButton.gameObject.transform.localPosition = new Vector3(0, -80, 0);
                        pauseExitGameButton.gameObject.transform.localPosition = new Vector3(0, -160, 0);
                    }
                    else if (GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.betweenMissions || GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.story) {
                        pauseSettingsButton.gameObject.SetActive(false);
                        pauseEndMissionButton.gameObject.SetActive(false);
                        pauseResumeButton.gameObject.SetActive(true);
                        pauseSaveButton.gameObject.SetActive(true);

                        pauseBackToMainMenuButton.gameObject.transform.localPosition = new Vector3(0, -80, 0);
                        pauseExitGameButton.gameObject.transform.localPosition = new Vector3(0, -160, 0);
                    }
                    else if (GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.inMission) {
                        pauseSettingsButton.gameObject.SetActive(false);
                        pauseResumeButton.gameObject.SetActive(true);
                        pauseEndMissionButton.gameObject.SetActive(true);
                        pauseSaveButton.gameObject.SetActive(true);

                        pauseBackToMainMenuButton.gameObject.transform.localPosition = new Vector3(0, -160, 0);
                        pauseExitGameButton.gameObject.transform.localPosition = new Vector3(0, -240, 0);
                    }
                    break;
                }
            case GameHandler.GameState.inMission: {
                    HideEverythingOnTheRightDisplayPanel();
                    UpdateRightDisplayPanel();
                    break; 
                }
            case GameHandler.GameState.missionOver: {
                    WriteMissionOverStatistics();
                    break;
                }
        }
    }

    private void ActivateAppropriateDisplays() {
        pauseGameDisplay.SetActive(GameHandler.IsGamePaused());

        if (!GameHandler.IsGamePaused()) {
            betweenMissionsDisplay.SetActive(GameHandler.IsBetweenMissions());
            inMissionDispaly.SetActive(GameHandler.IsInMission());
            missionOverDispaly.SetActive(GameHandler.IsMissionOver());
            mainMenuDisplay.SetActive(GameHandler.IsMainMenu());
            sandboxMissionSetupDisplay.SetActive(GameHandler.IsSandboxMissionSetup());
            storyDisplay.SetActive(GameHandler.IsStory());

            if (GameHandler.IsMainMenu()) {
                newSandboxMissionButton.gameObject.SetActive(GameHandler.IsGameCompleted());
            }
        }
    }

    private void CheckSandboxMissionInputFields() {
        string[] enemyList;

        // getting the values from the input fields
        Utils.GetIntFromInputField(startingCoriumInputField, out int startingCorium);
        Utils.GetIntFromInputField(startingAntoniumInputField, out int startingAntonium);

        Utils.GetIntFromInputField(startingWorkerInputField, out int startingWorker);
        Utils.GetIntFromInputField(startingPatrollerInputField, out int startingPatroller);
        Utils.GetIntFromInputField(startingProtectorInputField, out int startingProtector);
        Utils.GetIntFromInputField(startingGuardianInputField, out int startingGuardian);
        Utils.GetIntFromInputField(startingEngineerInputField, out int startingEngineer);
        Utils.GetIntFromInputField(startingIndustrialMinerInputField, out int startingIndustrialMiner);

        Utils.GetIntFromInputField(startingHiveAmountInputField, out int startingHiveAmount);
        Utils.GetFloatFromInputField(secondsBetweenWavesInputField, out float secondsBetweenWaves);
        Utils.GetFloatFromInputField(minWaveStrengthInputField, out float minWaveStrength);
        Utils.GetFloatFromInputField(maxWaveStrengthInputField, out float maxWaveStrength);
        Utils.GetFloatFromInputField(minSpawnDistanceInputField, out float minEnemySpawningDistance);
        Utils.GetFloatFromInputField(maxSpawnDistanceInputField, out float maxEnemySpawningDistance);
        if (enemyListInputField.transform.Find("Text").GetComponent<Text>().text != "") {
            enemyList = enemyListInputField.transform.Find("Text").GetComponent<Text>().text.Trim().ToLower().Split(' ');
        }
        else {
            enemyList = enemyListInputField.transform.Find("Placeholder").GetComponent<Text>().text.Trim().Split(' ');
        }

        Utils.GetIntFromInputField(spawnedCoriumAmountInputField, out int coriumFieldAmount);
        Utils.GetIntFromInputField(spawnedAntoniumAmountInputField, out int antoniumFieldAmount);
        Utils.GetFloatFromInputField(minResourceDistanceInputField, out float minResourceDistance);
        Utils.GetFloatFromInputField(maxResourceDistanceInputField, out float maxResourceDistance);

        Utils.GetFloatFromInputField(gameSpeedInputField, out float gameSpeed);

        // checking if the values are correct
        if (startingIndustrialMiner + startingGuardian > 1) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: You can only start the game with one large drone. (This includes guardians and industruial miners)";
            return;
        }
        if (startingCorium < 0 || startingAntonium < 0 || startingWorker < 0 || startingPatroller < 0 || startingProtector < 0 || startingGuardian < 0 || startingEngineer < 0 || startingIndustrialMiner < 0 || startingHiveAmount < 0 || secondsBetweenWaves < 0 || minWaveStrength < 0 || maxWaveStrength < 0 || minEnemySpawningDistance < 0 || maxEnemySpawningDistance < 0 || coriumFieldAmount < 0 || antoniumFieldAmount < 0 || minResourceDistance < 0 || maxResourceDistance < 0 || gameSpeed < 0) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: One of the input fields contains a negative value.";
            return;
        }
        if (startingHiveAmount > 50) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: You cannot start the game with more than 50 hives.";
            return;
        }
        if (secondsBetweenWaves < 5) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: There must be at lest 5 seconds between enemy waves.";
            return;
        }
        if (secondsBetweenWaves < 5) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: There must be at lest 5 seconds between enemy waves.";
            return;
        }
        if (minWaveStrength > 50 || maxWaveStrength > 50) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: Both min and max wave strengths have to be under 50.";
            return;
        }
        if (minEnemySpawningDistance > 200 || maxEnemySpawningDistance > 200) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: Both min and max enemy spawning distances have to be under 200.";
            return;
        }
        if (coriumFieldAmount + antoniumFieldAmount > 1000) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: There can't be more resource fields spawned than a thousand.";
            return;
        }
        if (minResourceDistance > 200 || maxResourceDistance > 200) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: Both min and max resource spawning distances have to be under 200.";
            return;
        }
        if (!hQ_AndDroneSpawning.CheckStartingDroneEnergyConsumption(startingWorker, startingPatroller, startingProtector, startingGuardian, startingEngineer, startingIndustrialMiner)) {
            sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "Error: The HQ can't provide enough energy for the drones. (choose fewer starting drones)";
            return;
        }

        // clearing the errors
        sandboxMissionSetupErrorDisplay.GetComponent<Text>().text = "";

        // setting the values ingame
        ResourceManager.SetEnergy(0);
        ResourceManager.SetCorium(startingCorium);
        ResourceManager.SetAntonium(startingAntonium);

        // setting up the game
        SetMissionObjectiveText("");

        hQ_AndDroneSpawning.SetStartingWorkerAmount(startingWorker);
        hQ_AndDroneSpawning.SetStartingPatrollerAmount(startingPatroller);
        hQ_AndDroneSpawning.SetStartingProtectorAmount(startingProtector);
        hQ_AndDroneSpawning.SetStartingGuardianAmount(startingGuardian);
        hQ_AndDroneSpawning.SetStartingEngineerAmount(startingEngineer);
        hQ_AndDroneSpawning.SetStartingIndustrialMinerAmount(startingIndustrialMiner);

        enemySpawning.SetStartingHiveAmount(startingHiveAmount);
        enemySpawning.SetSecondsBetweenWaves(secondsBetweenWaves);
        enemySpawning.SetMinWaveStrength(minWaveStrength);
        enemySpawning.SetMaxWaveStrength(maxWaveStrength);
        enemySpawning.SetMinSpawningDistance(minEnemySpawningDistance);
        enemySpawning.SetMaxSpawningDistance(maxEnemySpawningDistance);
        enemySpawning.SetEnemySpawningGeneratorList(enemyList.ToList());

        resourceSpawning.SetCoriumAmount(coriumFieldAmount);
        resourceSpawning.SetAntoniumAmount(antoniumFieldAmount);
        resourceSpawning.SetMinDistance(minResourceDistance);
        resourceSpawning.SetMaxDistance(maxResourceDistance);

        GameHandler.SetTimeScale(gameSpeed);

        // starting the mission
        GameHandler.SetSandboxMission(true);
        cameraHandler.FocusOn(hQ_AndDroneSpawning.GetHQPosition());
        enemySpawning.InitializeMission();
        resourceSpawning.InitializeMission();
        hQ_AndDroneSpawning.InitializeMission();
        TimeTickSystem.ResetTimer();
        gameHandler.StartMission();
        SetMissionBuilding();
    }

    private void HideEverythingOnTheRightDisplayPanel() {
        selectedBuildingNameDisplay.SetActive(false);
        selectedBuildingInformationDisplay.SetActive(false);
        assembleButton1.gameObject.SetActive(false);
        assemble1Information.SetActive(false);
        assembleButton2.gameObject.SetActive(false);
        assemble2Information.SetActive(false);
        assemblingAndQueueInformation.SetActive(false);
        activateMissionBuildingButton.gameObject.SetActive(false);

        selectedUnitDisplay.SetActive(false);
        selectedDrone1Button.gameObject.SetActive(false);
        selectedDrone2Button.gameObject.SetActive(false);

        baseComponentMenuButton.gameObject.SetActive(false);
        baseComponentMenuText.SetActive(false);
        buildWallButton.gameObject.SetActive(false);
        buildGeneratorButton.gameObject.SetActive(false);
        buildFusionReactorButton.gameObject.SetActive(false);
        buildMissionBuildingButton.gameObject.SetActive(false);

        factoryMenuButton.gameObject.SetActive(false);
        factoryMenuText.SetActive(false);
        buildFactoryButton.gameObject.SetActive(false);
        buildHeavyFactoryButton.gameObject.SetActive(false);

        turretMenuButton.gameObject.SetActive(false);
        turretMenuText.SetActive(false);
        buildMachineGunTurretButton.gameObject.SetActive(false);
        buildHeavyTurretButton.gameObject.SetActive(false);
        buildRailCannonButton.gameObject.SetActive(false);

        closeSelectedDisplayButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    private void UpdateRightDisplayPanel() {
        List<GameObject> selected = unitSelectionHandler.GetSelected();
        List<Building> buildings = ResourceManager.GetBuildingList();

        if (buildingGridSystem.buildingWithMouse) {
            cancelButton.gameObject.SetActive(true);
            movingDescriptionHandler.Disappear();
        }

        else if (selected.Count == 0) {
            switch (buildingMenu) {
                case BuildingMenu.outerMenu:
                    baseComponentMenuButton.gameObject.SetActive(true);
                    factoryMenuButton.gameObject.SetActive(true);
                    turretMenuButton.gameObject.SetActive(true);
                    break;
                case BuildingMenu.baseComponentMenu:
                    baseComponentMenuText.SetActive(true);
                    buildWallButton.gameObject.SetActive(true);
                    buildGeneratorButton.gameObject.SetActive(true);
                    if (TechSystem.IsTechUnlocked(TechSystem.Tech.FusionReactor) && buildings.Select(x => x.GetBuildingSO().NAME == heavyFactory.NAME && x.GetConstructionTimeRemaining() <= 0).Where(x => x).Any()) {
                        buildFusionReactorButton.gameObject.SetActive(true);
                    }
                    if (missionBuilding && buildings.Select(x => x.GetBuildingSO().NAME == heavyFactory.NAME && x.GetConstructionTimeRemaining() <= 0).Where(x => x).Any()) {
                        buildMissionBuildingButton.gameObject.SetActive(true);
                    }
                    backButton.gameObject.SetActive(true);
                    break;
                case BuildingMenu.factoryMenu:
                    factoryMenuText.SetActive(true);
                    buildFactoryButton.gameObject.SetActive(true);
                    if (buildings.Select(x => x.GetBuildingSO().NAME == droneFactory.NAME && x.GetConstructionTimeRemaining() <= 0).Where(x => x).Any()) {
                        buildHeavyFactoryButton.gameObject.SetActive(true);
                    }
                    backButton.gameObject.SetActive(true);
                    break;
                case BuildingMenu.turretMenu:
                    turretMenuText.SetActive(true);
                    buildMachineGunTurretButton.gameObject.SetActive(true);
                    if (buildings.Select(x => x.GetBuildingSO().NAME == droneFactory.NAME && x.GetConstructionTimeRemaining() <= 0).Where(x => x).Any()) {
                        buildHeavyTurretButton.gameObject.SetActive(true);
                    }
                    if (buildings.Select(x => x.GetBuildingSO().NAME == heavyFactory.NAME && x.GetConstructionTimeRemaining() <= 0).Where(x => x).Any()) {
                        buildRailCannonButton.gameObject.SetActive(true);
                    }
                    backButton.gameObject.SetActive(true);
                    break;
            }
        }

        // display selected building
        else if (selected[0].TryGetComponent<Building>(out Building building)) {
            closeSelectedDisplayButton.gameObject.SetActive(true);
            selectedBuildingNameDisplay.SetActive(true);
            selectedBuildingNameDisplay.GetComponent<Text>().text = building.GetBuildingSO().NAME;
            selectedBuildingInformationDisplay.SetActive(true);
            selectedBuildingInformationDisplay.GetComponent<Text>().text = "Integrity: " + (int)Math.Ceiling(building.GetHealth()) + "/" + (int)Math.Ceiling(building.GetMaxHealth()) + "\n";

            if (building.GetConstructionTimeRemaining() > 0) {
                selectedBuildingInformationDisplay.GetComponent<Text>().text = "---- Under construction ----\n\nIntegrity: " + (int)Math.Ceiling(building.GetHealth()) + "/" + (int)Math.Ceiling(building.GetMaxHealth())
                    + "\nTime remaining: " + (int)building.GetConstructionTimeRemaining() + " sec\nCompleted: " + Math.Round((building.GetConstructionTime() - building.GetConstructionTimeRemaining()) / (building.GetConstructionTime()) * 100) + "%\n\n---- Under construction ----";
            }

            else if (selected[0].TryGetComponent<Turret>(out Turret turret)) {
                selectedBuildingInformationDisplay.GetComponent<Text>().text += "Cooldown: " + (turret.GetTurretSO().COOLDOWN - turret.GetCooldown() == 0 ? "ready" : Math.Ceiling(turret.GetTurretSO().COOLDOWN - turret.GetCooldown()) + " sec") + "\n";
            }
            // Displaying the selected building
            else if (selected[0].TryGetComponent<Factory>(out Factory factory)) {
                DroneSO assemblyLine1 = factory.GetAssemblyLine1();
                if (assemblyLine1 != null) {
                    assembleButton1.gameObject.SetActive(true);
                    assemble1Information.SetActive(true);

                    assembleButton1.gameObject.GetComponentInChildren<Text>().text = "Assemble " + assemblyLine1.NAME;
                    assembleButton1.onClick.RemoveAllListeners();
                    assembleButton1.onClick.AddListener(() => factory.AddToProductionQueue(assemblyLine1));

                    assemble1Information.GetComponent<Text>().text = "Cost:\n\tEnergy: " + assemblyLine1.ENERGY_COST + "\n\tCorium: " + assemblyLine1.CORIUM_COST + "\n\tAntonium: " + assemblyLine1.ANTONIUM_COST + "\n\tTime: " + assemblyLine1.prefab.GetComponent<Drone>().GetConstructionTime() / factory.GetProductionSpeed() + " sec";
                }

                DroneSO assemblyLine2 = factory.GetAssemblyLine2();
                if (assemblyLine2 != null) {
                    assembleButton2.gameObject.SetActive(true);
                    assemble2Information.SetActive(true);

                    assembleButton2.GetComponentInChildren<Text>().text = "Assemble " + assemblyLine2.NAME;
                    assembleButton2.onClick.RemoveAllListeners();
                    assembleButton2.onClick.AddListener(() => factory.AddToProductionQueue(assemblyLine2));

                    assemble2Information.GetComponent<Text>().text = "Cost:\n\tEnergy: " + assemblyLine2.ENERGY_COST + "\n\tCorium: " + assemblyLine2.CORIUM_COST + "\n\tAntonium: " + assemblyLine2.ANTONIUM_COST + "\n\tTime: " + assemblyLine2.prefab.GetComponent<Drone>().GetConstructionTime() / factory.GetProductionSpeed() + " sec";
                }

                List<(float timeRemaining, DroneSO droneSO)> productionQueue = factory.GetProductionQueue();
                if (productionQueue.Count > 0) {
                    assemblingAndQueueInformation.SetActive(true);
                    Text assemblingAndQueueInformationText = assemblingAndQueueInformation.GetComponent<Text>();
                    assemblingAndQueueInformationText.text = "Assembling: " + productionQueue[0].droneSO.NAME;
                    assemblingAndQueueInformationText.text += "\tReady in: " + Math.Ceiling(productionQueue[0].timeRemaining / factory.GetProductionSpeed()) + " sec\n";

                    if (productionQueue.Count > 1) {
                        assemblingAndQueueInformationText.text += "\nIn Queue:";
                        bool first = true;
                        foreach (var drone in productionQueue) {
                            if (drone.timeRemaining == drone.droneSO.CONSTRUCTION_TIME) {
                                assemblingAndQueueInformationText.text += (first ? "" : ",") + " " + drone.droneSO.NAME;
                                first = false;
                            }
                        }
                    }
                }
            }
            else if (selected[0].TryGetComponent<Thumper>(out Thumper thumper)) {
                activateMissionBuildingButton.gameObject.SetActive(true);
                selectedBuildingInformationDisplay.GetComponent<Text>().text += "Active for: " + (int)thumper.GetActiveSeconds() + " seconds.";

                if (thumper.IsActivated()) {
                    activateMissionBuildingButton.GetComponentInChildren<Text>().color = new Color32(0, 100, 0, 255);
                    activateMissionBuildingButton.GetComponentInChildren<Text>().text = "activated";
                }
                else {
                    activateMissionBuildingButton.GetComponentInChildren<Text>().color = new Color32(0, 255, 0, 255);
                    activateMissionBuildingButton.GetComponentInChildren<Text>().text = "activate";
                    activateMissionBuildingButton.onClick.RemoveAllListeners();
                    activateMissionBuildingButton.onClick.AddListener(() => thumper.Activate());
                }
            }
            else if (selected[0].TryGetComponent<UndergroundExplosionDevice>(out UndergroundExplosionDevice undergroundExplosionDevice)) {
                activateMissionBuildingButton.gameObject.SetActive(true);
                selectedBuildingInformationDisplay.GetComponent<Text>().text += "Time remaining before fired: " + (int)undergroundExplosionDevice.GetCooldown() + " seconds.";

                if (undergroundExplosionDevice.IsActivated()) {
                    activateMissionBuildingButton.GetComponentInChildren<Text>().color = new Color32(0, 100, 0, 255);
                    activateMissionBuildingButton.GetComponentInChildren<Text>().text = "activated";
                }
                else {
                    activateMissionBuildingButton.GetComponentInChildren<Text>().color = new Color32(0, 255, 0, 255);
                    activateMissionBuildingButton.GetComponentInChildren<Text>().text = "activate";
                    activateMissionBuildingButton.onClick.RemoveAllListeners();
                    activateMissionBuildingButton.onClick.AddListener(() => undergroundExplosionDevice.Activate());
                }
            }
        }

        // Listing the selected drones
        else if (selected[0].TryGetComponent<Drone>(out Drone d)) {
            closeSelectedDisplayButton.gameObject.SetActive(true);
            selectedUnitDisplay.SetActive(true);
            selectedUnitsText.text = "";

            foreach (GameObject droneGameObject in selected) {
                selectedDrone1Button.onClick.RemoveAllListeners();
                selectedDrone2Button.onClick.RemoveAllListeners();

                Drone drone = droneGameObject.GetComponent<Drone>();

                selectedUnitsText.text += drone.GetDroneSO().NAME + ":\n";
                selectedUnitsText.text += "\tintegrity: " + Math.Ceiling(drone.GetHealth()) + "/" + Math.Ceiling(drone.GetMaxHealth()) + "\n";
                selectedUnitsText.text += "\tcooldown: " + (drone.GetCooldownMax() - drone.GetCooldown() == 0 ? "ready" : Math.Ceiling(drone.GetCooldownMax() - drone.GetCooldown()) + " sec") + "\n";

                if (drone.TryGetComponent<Worker>(out Worker worker)) {
                    selectedUnitsText.text += "\tstorage: " + (worker.HasSample() ? "carrying sample" : (worker.GetCapacity() - worker.GetStoredResourceAmount() == 0 ? "full" : worker.GetStoredResourceAmount() + "/" + worker.GetCapacity())) + "\n";
                    if (selected.Count == 1 && TechSystem.IsTechUnlocked(TechSystem.Tech.WorkerAI)) {
                        if (worker.IsAI_Active()) {
                            selectedDrone1Button.gameObject.SetActive(true);
                            selectedDrone2Button.gameObject.SetActive(true);
                            selectedDrone1Button.GetComponentInChildren<Text>().text = "select location";
                            selectedDrone2Button.GetComponentInChildren<Text>().text = "deactivate AI";
                            selectedDrone1Button.onClick.AddListener(() => {
                                nextClickLocator.GetAndSetNextClickLocation(worker.SetMiningLocation, worker.GetRange());
                            });
                            selectedDrone2Button.onClick.AddListener(() => worker.SetAI_Active(false));
                        }
                        else {
                            selectedDrone1Button.gameObject.SetActive(true);
                            selectedDrone1Button.GetComponentInChildren<Text>().text = "activate AI";
                            selectedDrone1Button.onClick.AddListener(() => worker.SetAI_Active(true));
                        }
                    }
                }

                else if (drone.TryGetComponent<IndustrialMiner>(out IndustrialMiner industrialMiner)) {
                    selectedUnitsText.text += "\tstorage: " + (industrialMiner.GetCapacity() - industrialMiner.GetStorageAmount() == 0 ? "full" : industrialMiner.GetStorageAmount() + "/" + industrialMiner.GetCapacity()) + "\n";
                    selectedUnitsText.text += "\tdeployed: " + (industrialMiner.IsDeployed() ? "true" : "false") + "\n";
                    if (selected.Count == 1) {
                        selectedDrone1Button.gameObject.SetActive(true);
                        if (industrialMiner.IsDeployed()) {
                            selectedDrone1Button.GetComponentInChildren<Text>().text = "undeploy miner";
                        } else {
                            selectedDrone1Button.GetComponentInChildren<Text>().text = "deploy miner";
                        }
                        selectedDrone1Button.onClick.AddListener(() => industrialMiner.DeploymentSwitch());
                    }
                }

                selectedUnitsText.text += "\n";
            }
        }
    }

    private void ResourceDisplayTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        resourceText.text = "energy: " + Math.Floor(ResourceManager.GetEnergy()) + "\n";
        resourceText.text += "corium: " + Math.Floor(ResourceManager.GetCorium()) + "\n";
        resourceText.text += "antonium: " + Math.Floor(ResourceManager.GetAntonium())+ "\n";
        resourceText.text += "alien sample: " + ResourceManager.GetSample();
    }

    private void GameMessageDisplayTickUpdate(object sender, TimeTickSystem.OnTickEventArgs args) {
        gameMessageText.text = "";
        List<string> dictionaryIterator = new List<string>();
        foreach (string key in gameMessages.Keys) {
            dictionaryIterator.Add(key);
        }

        dictionaryIterator.Reverse();

        foreach (string message in dictionaryIterator) {
            if (gameMessages[message] > 0) {
                gameMessageText.text += message + "\n";
                gameMessages[message] -= TimeTickSystem.TICK_TIMER_MAX;
            } else {
                gameMessages.Remove(message);
            }
        }
    }

    private void WriteMissionOverStatistics() {
        missionOverStatisticsDispaly.GetComponent<Text>().text = "Time elapsed: " + StatisticsTracker.GetTimeElapsed() + "\n\nSaved Materials\n\tCorium: " + ResourceManager.GetCorium() + "\n\tAntonium: " + ResourceManager.GetAntonium() + "\n\tSample: " + ResourceManager.GetSample() + "\n\nEnemies destroyed: " + StatisticsTracker.GetEnemyKillCountInMission();
    }

    private void LoadSaveDisplay() {
        saveLoadScrollView.SetActive(true);

        Transform newSaveButton = Instantiate(buttonPrefab);
        createdUI_Elements.Add(newSaveButton);
        newSaveButton.SetParent(saveLoadScrollViewContent.transform, false);
        newSaveButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100, 0);
        newSaveButton.GetComponentInChildren<Text>().text = "create new save file";
        newSaveButton.GetComponent<Button>().onClick.AddListener(() => {
            stringInputHandler.CreateGetStringInput(() => {
                SaveSystem.Save(stringInputHandler.GetResult() + ".txt");
            }, "Enter the name of your save file");

            CloseSaveLoadScrollView();
        });

        List<FileInfo> fileInfos = SaveSystem.GetSaveFileInfos();

        int i = -200;
        saveLoadScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -i);
        foreach (FileInfo fileInfo in fileInfos) {
            Transform olderSaveButton = Instantiate(fileButtonPrefab);
            createdUI_Elements.Add(olderSaveButton);
            olderSaveButton.SetParent(saveLoadScrollViewContent.transform, false);
            olderSaveButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, i, 0);
            i -= 100;
            saveLoadScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -i);
            olderSaveButton.Find("Inner/Name Text").GetComponent<Text>().text = fileInfo.Name[..^4];
            olderSaveButton.Find("Inner/Date Text").GetComponent<Text>().text = fileInfo.LastWriteTime.ToString();
            olderSaveButton.GetComponent<Button>().onClick.AddListener(() => {
                SaveSystem.Save(fileInfo.Name);
                CloseSaveLoadScrollView();
            });
        }
    }

    private void LoadLoadDisplay() {
        saveLoadScrollView.SetActive(true);

        List<FileInfo> fileInfos = SaveSystem.GetSaveFileInfos();

        int i = -100;
        saveLoadScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -i);
        foreach (FileInfo fileInfo in fileInfos) {
            Transform loadSaveButton = Instantiate(fileButtonPrefab);
            createdUI_Elements.Add(loadSaveButton);
            loadSaveButton.SetParent(saveLoadScrollViewContent.transform, false);
            loadSaveButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, i, 0);
            i -= 100;
            saveLoadScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -i);
            loadSaveButton.Find("Inner/Name Text").GetComponent<Text>().text = fileInfo.Name[..^4];
            loadSaveButton.Find("Inner/Date Text").GetComponent<Text>().text = fileInfo.LastWriteTime.ToString();
            loadSaveButton.GetComponent<Button>().onClick.AddListener(() => {
                StartCoroutine(SaveSystem.Load(fileInfo.FullName));
                CloseSaveLoadScrollView();
            });
        }
    }


    private void DeleteCreatedUI_Elements() {
        foreach (Transform t in createdUI_Elements) {
            Destroy(t.gameObject);
        }
        createdUI_Elements.Clear();
    }

    private void CloseSaveLoadScrollView() {
        DeleteCreatedUI_Elements();
        saveLoadScrollView.SetActive(false);
        if (GameHandler.GetGameStateBeforePaused() == GameHandler.GameState.mainMenu) {
            gameHandler.GamePauseSwitch();
        }
    }
}
