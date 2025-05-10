using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetweenMissionsUI_Assistant : MonoBehaviour {

    [SerializeField] private MessageSystem messageSystem;
    [SerializeField] private CampaignHandler campaignHandler;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private HQ_AndDroneSpawning hQ_AndDroneSpawning;
    [SerializeField] private ConfirmActionHandler confirmActionHandler;
    [SerializeField] private TechTreeUI_Assistant techTreeUI_Assistant;
    [SerializeField] private UI_Assistant uI_Assistant;

    [SerializeField] private OnShipAssemblySO hQ;
    [SerializeField] private OnShipAssemblySO worker;
    [SerializeField] private OnShipAssemblySO patroller;
    [SerializeField] private OnShipAssemblySO protector;
    [SerializeField] private OnShipAssemblySO guardian;
    [SerializeField] private OnShipAssemblySO engineer;
    [SerializeField] private OnShipAssemblySO industrialMiner;
    [SerializeField] private OnShipAssemblySO railcannonBullet;
    [SerializeField] private OnShipAssemblySO nuke;

    [SerializeField] private EnemySO swarmerSO;
    [SerializeField] private EnemySO warriorSO;
    [SerializeField] private EnemySO melterSO;
    [SerializeField] private EnemySO breacherSO;
    [SerializeField] private EnemySO hiveSO;

    // menu buttons
    [SerializeField] private Button techTreeButton;
    [SerializeField] private Button constructionOnShipButton;
    [SerializeField] private Button missionSetupButton;
    [SerializeField] private Button enemyDatabankButton;
    [SerializeField] private Button aIDA_DisplayButton;

    // tech tree display
    [SerializeField] private GameObject techTreeDisplay;

    // construction on ship display
    [SerializeField] private GameObject constructionOnShipDisplay;
    [SerializeField] private Button constructionOnShipHQ_ConstructionButton;
    [SerializeField] private Button constructionOnShipDroneFactoryButton;
    [SerializeField] private Button constructionOnShipHeavyFactoryButton;
    [SerializeField] private Button constructionOnShipOrbitalRailCannonButton;
    [SerializeField] private Button constructionOnShipOrbitalNukeSiloButton;
    [SerializeField] private Button constructionOnShipScienceLabButton;
    [SerializeField] private Button constructionOnShipNetworkExpansionButton;
    [SerializeField] private GameObject constructionOnShipBuildingNameText;
    [SerializeField] private GameObject constructionOnShipBuildingDescriptionText;
    [SerializeField] private GameObject constructionOnShipBuildingCostText;
    [SerializeField] private GameObject constructionOnShipBuildingOperationalText;
    [SerializeField] private GameObject constructionOnShipConstructionErrorText;
    [SerializeField] private Button constructionOnShipConstructBuildingButton;
    [SerializeField] private GameObject constructionOnShipAssemblyDisplay;
    [SerializeField] private GameObject constructionOnShipAssemblyLine1Display;
    [SerializeField] private GameObject constructionOnShipAssemblyLine1NameText;
    [SerializeField] private GameObject constructionOnShipAssemblyLine1DescriptionText;
    [SerializeField] private InputField constructionOnShipAssemblyLine1InputField;
    [SerializeField] private GameObject constructionOnShipAssemblyLine2Display;
    [SerializeField] private GameObject constructionOnShipAssemblyLine2NameText;
    [SerializeField] private GameObject constructionOnShipAssemblyLine2DescriptionText;
    [SerializeField] private InputField constructionOnShipAssemblyLine2InputField;
    [SerializeField] private GameObject constructionOnShipAssemblyLine3Display;
    [SerializeField] private GameObject constructionOnShipAssemblyLine3NameText;
    [SerializeField] private GameObject constructionOnShipAssemblyLine3DescriptionText;
    [SerializeField] private InputField constructionOnShipAssemblyLine3InputField;
    [SerializeField] private Button constructionOnShipStartConstructionButton;

    // mission setup display
    [SerializeField] private GameObject missionSetupDisplay;
    [SerializeField] private GameObject missionSetupMissionTitleText;
    [SerializeField] private GameObject missionSetupMissionDescriptionText;
    [SerializeField] private GameObject missionSetupRemainingHQsText;
    [SerializeField] private GameObject missionSetupErrorDisplay;
    [SerializeField] private InputField missionSetupStartingWorkersInputField;
    [SerializeField] private GameObject missionSetupWorkersOnShipText;
    [SerializeField] private InputField missionSetupStartingPatrollersInputField;
    [SerializeField] private GameObject missionSetupPatrollersOnShipText;
    [SerializeField] private InputField missionSetupStartingProtectorsInputField;
    [SerializeField] private GameObject missionSetupProtectorsOnShipText;
    [SerializeField] private InputField missionSetupStartingGuardianInputField;
    [SerializeField] private GameObject missionSetupGuardiansOnShipText;
    [SerializeField] private GameObject missionSetupEngineersText;
    [SerializeField] private InputField missionSetupStartingEngineersInputField;
    [SerializeField] private GameObject missionSetupEngineersOnShipText;
    [SerializeField] private GameObject missionSetupIndustrialMinerText;
    [SerializeField] private InputField missionSetupStartingIndustrialMinerInputField;
    [SerializeField] private GameObject missionSetupIndustrialMinersOnShipText;
    [SerializeField] private Button missionSetupStartMissionButton;
    [SerializeField] private Button missionSetup1stMissionButton;
    [SerializeField] private Button missionSetup2ndMissionButton;
    [SerializeField] private Button missionSetup3rdMissionButton;
    [SerializeField] private Button missionSetup4thMissionButton;
    [SerializeField] private Button missionSetup5thMissionButton;

    // enemy database display
    [SerializeField] private GameObject enemyDatabaseDisplay;
    [SerializeField] private GameObject enemyDatabaseEnemyNameDisplay;
    [SerializeField] private GameObject enemyDatabaseEnemyDescriptionDisplay;
    [SerializeField] private GameObject enemyDatabaseEnemyStatisticsDisplay;
    [SerializeField] private GameObject enemyDatabaseEnemyImage;
    [SerializeField] private Button enemyDatabaseSwarmerButton;
    [SerializeField] private Button enemyDatabaseWarriorButton;
    [SerializeField] private Button enemyDatabaseMelterButton;
    [SerializeField] private Button enemyDatabaseBreacherButton;
    [SerializeField] private Button enemyDatabaseHiveButton;

    private enum BetweenMissionsMenu {
        techTree,
        constructionOnShip,
        missionSetup,
        enemyDatabase
    }

    private BetweenMissionsMenu betweenMissionsMenu;
    private bool resourceDisplay;
    private Text aIDA_Text;

    private Text constructionErrorText;
    private Text buildingNameText;
    private Text buildingDescriptionText;
    private Text buildingCostText;
    private Text assemblyLine1NameText;
    private Text assemblyLine1DescriptionText;
    private Text assemblyLine2NameText;
    private Text assemblyLine2DescriptionText;
    private Text assemblyLine3NameText;
    private Text assemblyLine3DescriptionText;

    private Text missionTitleText;
    private Text missionDescriptionText;
    private Text missionSetupErrorText;
    private Text workerOnShipText;
    private Text patrollerOnShipText;
    private Text protectorOnShipText;
    private Text guardianOnShipText;
    private Text engineerOnShipText;
    private Text industrialMinerOnShipText;
    private Text remainingHQsText;

    private Text enemyNameText;
    private Text enemyDescriptionText;
    private Text enemyStatisticsText;

    public void UpdateResourceDisplay() {
        campaignHandler.FirstMissionCompletedCheck();
        if (resourceDisplay) {
            aIDA_Text.text = "Corium: " + ResourceManager.GetCoriumOnShip() + "\r\nAntonium: " + ResourceManager.GetAntoniumOnShip() + "\r\nAlien Sample: " + ResourceManager.GetSampleOnShip();
        }
        else {
            aIDA_Text.text = "AIDA: \r\nWelcome on the ship captain! \r\n(At least virtually)";
            aIDA_Text.text = "AIDA: \r\nClick here to see your resources";
        }
    }

    private void Awake() {
        resourceDisplay = false;

        MessageSystem.OnBetweenMissionsDisplayUpdate += UpdateDisplay;

        betweenMissionsMenu = BetweenMissionsMenu.missionSetup;
        // menu buttons
        techTreeButton.onClick.AddListener(() => { betweenMissionsMenu = BetweenMissionsMenu.techTree; messageSystem.BetweenMissionsDisplayUpdate(); });
        constructionOnShipButton.onClick.AddListener(() => { betweenMissionsMenu = BetweenMissionsMenu.constructionOnShip; messageSystem.BetweenMissionsDisplayUpdate(); });
        missionSetupButton.onClick.AddListener(() => { betweenMissionsMenu = BetweenMissionsMenu.missionSetup; messageSystem.BetweenMissionsDisplayUpdate(); });
        enemyDatabankButton.onClick.AddListener(() => { betweenMissionsMenu = BetweenMissionsMenu.enemyDatabase; messageSystem.BetweenMissionsDisplayUpdate(); });
        aIDA_DisplayButton.onClick.AddListener(() => { resourceDisplay = !resourceDisplay; UpdateResourceDisplay(); });

        // construction on ship buttons
        constructionOnShipHQ_ConstructionButton.onClick.AddListener(() => { WriteOnConstructionOnShipDisplay(Texts.HQ_ConstructionName, Texts.HQ_ConstructionDescription, 0, 0, 0, hQ, worker, patroller); });
        constructionOnShipDroneFactoryButton.onClick.AddListener(() => {
            if (TechSystem.IsTechUnlocked(TechSystem.Tech.Engineer)) {
                WriteOnConstructionOnShipDisplay(Texts.DroneFactoryName, Texts.DroneFactoryDescription, 200, 0, 0, protector, null, engineer);
            } else {
                WriteOnConstructionOnShipDisplay(Texts.DroneFactoryName, Texts.DroneFactoryDescription, 200, 0, 0, null, protector, null);
            }
        });
        constructionOnShipHeavyFactoryButton.onClick.AddListener(() => {
            if (TechSystem.IsTechUnlocked(TechSystem.Tech.IndustrialMiner)) {
                WriteOnConstructionOnShipDisplay(Texts.HeavyFactoryName, Texts.HeavyFactoryDescription, 400, 100, 0, guardian, null, industrialMiner);
            } else {
                WriteOnConstructionOnShipDisplay(Texts.HeavyFactoryName, Texts.HeavyFactoryDescription, 400, 100, 0, null, guardian, null);
            }
        });
        constructionOnShipOrbitalRailCannonButton.onClick.AddListener(() => { WriteOnConstructionOnShipDisplay(Texts.OrbitalRailCannonName, Texts.OrbitalRailCannonDescription, 800, 200, 0, null, railcannonBullet, null); });
        constructionOnShipOrbitalNukeSiloButton.onClick.AddListener(() => { WriteOnConstructionOnShipDisplay(Texts.OrbitalNukeSiloName, Texts.OrbitalNukeSiloDescription, 500, 150, 0, null, nuke, null); });
        constructionOnShipScienceLabButton.onClick.AddListener(() => { WriteOnConstructionOnShipDisplay(Texts.ScienceLabName, Texts.ScienceLabDescription, 240, 170, 3); });
        constructionOnShipNetworkExpansionButton.onClick.AddListener(() => { WriteOnConstructionOnShipDisplay(Texts.NetworkExpansionName, Texts.NetworkExpansionDescription, 100, 500, 7); });

        // mission setup buttons
        missionSetup1stMissionButton.onClick.AddListener(() => { WriteOnMissionSetupDisplay(Texts.mission1Title, Texts.mission1Description, (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.xenohunt)
                    && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.planetfall) ? Texts.mission1Objective : "")); campaignHandler.SetChosenMission(CampaignHandler.Missions.planetfall); });
        missionSetup2ndMissionButton.onClick.AddListener(() => { WriteOnMissionSetupDisplay(Texts.mission2Title, Texts.mission2Description, (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.farsight)
                    && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.xenohunt) ? Texts.mission2Objective : "")); campaignHandler.SetChosenMission(CampaignHandler.Missions.xenohunt); });
        missionSetup3rdMissionButton.onClick.AddListener(() => { WriteOnMissionSetupDisplay(Texts.mission3Title, Texts.mission3Description, (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.hellgate)
                    && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.farsight) ? Texts.mission3Objective : "")); campaignHandler.SetChosenMission(CampaignHandler.Missions.farsight); });
        missionSetup4thMissionButton.onClick.AddListener(() => { WriteOnMissionSetupDisplay(Texts.mission4Title, Texts.mission4Description, (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.excalibur)
                    && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.hellgate) ? Texts.mission4Objective : "")); campaignHandler.SetChosenMission(CampaignHandler.Missions.hellgate); });
        missionSetup5thMissionButton.onClick.AddListener(() => { WriteOnMissionSetupDisplay(Texts.mission5Title, Texts.mission5Description, Texts.mission5Objective, Texts.mission5Additional); campaignHandler.SetChosenMission(CampaignHandler.Missions.excalibur); });
        missionSetupStartMissionButton.onClick.AddListener(() => { StartMission(); });

        // enemy database buttons
        enemyDatabaseSwarmerButton.onClick.AddListener(() => { WriteOnEnemyDatabaseDisplay(swarmerSO, Texts.swarmerDescription); });
        enemyDatabaseWarriorButton.onClick.AddListener(() => { WriteOnEnemyDatabaseDisplay(warriorSO, Texts.warriorDescription); });
        enemyDatabaseMelterButton.onClick.AddListener(() => { WriteOnEnemyDatabaseDisplay(melterSO, Texts.melterDescription); });
        enemyDatabaseBreacherButton.onClick.AddListener(() => { WriteOnEnemyDatabaseDisplay(breacherSO, Texts.breacherDescription); });
        enemyDatabaseHiveButton.onClick.AddListener(() => { WriteOnEnemyDatabaseDisplay(hiveSO, Texts.hiveDescription); });

        aIDA_Text = aIDA_DisplayButton.GetComponentInChildren<Text>();

        // construction on ship texts
        constructionErrorText = constructionOnShipConstructionErrorText.GetComponent<Text>();
        buildingNameText = constructionOnShipBuildingNameText.GetComponent<Text>();
        buildingDescriptionText = constructionOnShipBuildingDescriptionText.GetComponent<Text>();
        buildingCostText = constructionOnShipBuildingCostText.GetComponent<Text>();
        assemblyLine1NameText = constructionOnShipAssemblyLine1NameText.GetComponent<Text>();
        assemblyLine1DescriptionText = constructionOnShipAssemblyLine1DescriptionText.GetComponent<Text>();
        assemblyLine2NameText = constructionOnShipAssemblyLine2NameText.GetComponent<Text>();
        assemblyLine2DescriptionText = constructionOnShipAssemblyLine2DescriptionText.GetComponent<Text>();
        assemblyLine3NameText = constructionOnShipAssemblyLine3NameText.GetComponent<Text>();
        assemblyLine3DescriptionText = constructionOnShipAssemblyLine3DescriptionText.GetComponent<Text>();

        // mission setup texts
        missionTitleText = missionSetupMissionTitleText.GetComponent<Text>();
        missionDescriptionText = missionSetupMissionDescriptionText.GetComponent<Text>();
        missionSetupErrorText = missionSetupErrorDisplay.GetComponent<Text>();
        workerOnShipText = missionSetupWorkersOnShipText.GetComponent<Text>();
        patrollerOnShipText = missionSetupPatrollersOnShipText.GetComponent<Text>();
        protectorOnShipText = missionSetupProtectorsOnShipText.GetComponent<Text>();
        guardianOnShipText = missionSetupGuardiansOnShipText.GetComponent<Text>();
        engineerOnShipText = missionSetupEngineersOnShipText.GetComponent<Text>();
        industrialMinerOnShipText = missionSetupIndustrialMinersOnShipText.GetComponent<Text>();
        remainingHQsText = missionSetupRemainingHQsText.GetComponent<Text>();

        // enemy database texts
        enemyNameText = enemyDatabaseEnemyNameDisplay.GetComponent<Text>();
        enemyDescriptionText = enemyDatabaseEnemyDescriptionDisplay.GetComponent<Text>();
        enemyStatisticsText = enemyDatabaseEnemyStatisticsDisplay.GetComponent<Text>();
    }

    private void UpdateDisplay(object sender, MessageSystem.BetweenMissionsDisplayUpdateEventArgs args) {
        UpdateResourceDisplay();

        // button color
        Debug.Log(campaignHandler.enemyDatabankUnlocked);
        if (campaignHandler.enemyDatabankUnlocked) {
            enemyDatabankButton.GetComponentInChildren<Text>().color = new Color32(255, 0, 0, 255);
        }
        else {
            enemyDatabankButton.GetComponentInChildren<Text>().color = new Color32(100, 0, 0, 255);
        }
        if (campaignHandler.techTreeUnlocked) {
            techTreeButton.GetComponentInChildren<Text>().color = new Color32(0, 255, 0, 255);
        }
        else {
            techTreeButton.GetComponentInChildren<Text>().color = new Color32(0, 100, 0, 255);
        }

        // changeing display
        if (betweenMissionsMenu == BetweenMissionsMenu.techTree && campaignHandler.techTreeUnlocked) {
            missionSetupDisplay.SetActive(false);
            constructionOnShipDisplay.SetActive(false);
            enemyDatabaseDisplay.SetActive(false);
            techTreeDisplay.SetActive(true);

            techTreeUI_Assistant.UpdateDisplay();
        }
        else if (betweenMissionsMenu == BetweenMissionsMenu.constructionOnShip) {
            techTreeDisplay.SetActive(false);
            enemyDatabaseDisplay.SetActive(false);
            missionSetupDisplay.SetActive(false);
            constructionOnShipDisplay.SetActive(true);

            WriteOnConstructionOnShipDisplay(Texts.HQ_ConstructionName, Texts.HQ_ConstructionDescription, 0, 0, 0, hQ, worker, patroller);
        }
        else if (betweenMissionsMenu == BetweenMissionsMenu.missionSetup) {
            techTreeDisplay.SetActive(false);
            enemyDatabaseDisplay.SetActive(false);
            constructionOnShipDisplay.SetActive(false);
            missionSetupDisplay.SetActive(true);

            if (campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.excalibur)) {
                WriteOnMissionSetupDisplay(Texts.mission5Title, Texts.mission5Description, Texts.mission5Objective, Texts.mission5Additional);
                campaignHandler.SetChosenMission(CampaignHandler.Missions.excalibur);
            }
            else if (campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.hellgate)) {
                WriteOnMissionSetupDisplay(Texts.mission4Title, Texts.mission4Description,
                    (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.excalibur) && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.hellgate) ? Texts.mission4Objective : ""));
                campaignHandler.SetChosenMission(CampaignHandler.Missions.hellgate);
            }
            else if (campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.farsight)) {
                WriteOnMissionSetupDisplay(Texts.mission3Title, Texts.mission3Description, 
                    (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.hellgate) && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.farsight) ? Texts.mission3Objective : ""));
                campaignHandler.SetChosenMission(CampaignHandler.Missions.farsight);
            }
            else if (campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.xenohunt)) {
                WriteOnMissionSetupDisplay(Texts.mission2Title, Texts.mission2Description, 
                    (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.farsight) && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.xenohunt) ? Texts.mission2Objective : ""));
                campaignHandler.SetChosenMission(CampaignHandler.Missions.xenohunt);
            }
            else {
                WriteOnMissionSetupDisplay(Texts.mission1Title, Texts.mission1Description, 
                    (!campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.xenohunt) && campaignHandler.GetUnlockedMissions().Contains(CampaignHandler.Missions.planetfall) ? Texts.mission1Objective : ""));
                campaignHandler.SetChosenMission(CampaignHandler.Missions.planetfall);
            }
        }
        else if (betweenMissionsMenu == BetweenMissionsMenu.enemyDatabase && campaignHandler.enemyDatabankUnlocked) {
            techTreeDisplay.SetActive(false);
            missionSetupDisplay.SetActive(false);
            constructionOnShipDisplay.SetActive(false);
            enemyDatabaseDisplay.SetActive(true);

            WriteOnEnemyDatabaseDisplay();
        }
    }

    private void WriteOnEnemyDatabaseDisplay(EnemySO enemySO = null, string description = "") {
        enemyDescriptionText.text = description;
        if (enemySO != null) {
            enemyNameText.text = enemySO.NAME;
            enemyStatisticsText.text = "health: " + enemySO.HEALTH +
                "\r\ndamage: " + enemySO.DAMAGE +
                "\r\nrange: " + (enemySO.DAMAGE == 0 ? "none" : "melee") +
                "\r\nattack cooldown: " + enemySO.COOLDOWN + " sec" +
                "\r\nmovement speed: " + enemySO.SPEED +
                "\r\n\r\nfirst wave when it can spawn: " + (enemySO.FIRST_SPAWNING_WAVE == 0 ? "always" : enemySO.FIRST_SPAWNING_WAVE);
        }
        else {
            enemyNameText.text = "";
            enemyStatisticsText.text = "";
        }
    }

    private void WriteOnMissionSetupDisplay (string title, string description, string objective, string additional = "") {
        HashSet<CampaignHandler.Missions> unlockedMissions = campaignHandler.GetUnlockedMissions();

        // display only accesible buttons
        missionSetup1stMissionButton.gameObject.SetActive(unlockedMissions.Contains(CampaignHandler.Missions.planetfall));
        missionSetup2ndMissionButton.gameObject.SetActive(unlockedMissions.Contains(CampaignHandler.Missions.xenohunt));
        missionSetup3rdMissionButton.gameObject.SetActive(unlockedMissions.Contains(CampaignHandler.Missions.farsight));
        missionSetup4thMissionButton.gameObject.SetActive(unlockedMissions.Contains(CampaignHandler.Missions.hellgate));
        missionSetup5thMissionButton.gameObject.SetActive(unlockedMissions.Contains(CampaignHandler.Missions.excalibur));

        missionTitleText.text = title;
        missionDescriptionText.text = description + objective + additional;

        missionSetupErrorText.text = "";

        Dictionary<string, int> dronesOnShip = ResourceManager.GetDronesOnShip();
        
        workerOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(worker.NAME, 0) + " on ship)";
        patrollerOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(patroller.NAME, 0) + " on ship)";
        protectorOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(protector.NAME, 0) + " on ship)";
        guardianOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(guardian.NAME, 0) + " on ship)";
        engineerOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(engineer.NAME, 0) + " on ship)";
        industrialMinerOnShipText.text = "(" + dronesOnShip.GetValueOrDefault(industrialMiner.NAME, 0) + " on ship)";

        remainingHQsText.text = "hqs remaining: " + dronesOnShip.GetValueOrDefault(hQ.NAME, 0);

        if (!TechSystem.IsTechUnlocked(TechSystem.Tech.Engineer)) {
            missionSetupEngineersText.SetActive(false);
            missionSetupStartingEngineersInputField.transform.parent.gameObject.SetActive(false);
            missionSetupEngineersOnShipText.SetActive(false);
        } else {
            missionSetupEngineersText.SetActive(true);
            missionSetupStartingEngineersInputField.transform.parent.gameObject.SetActive(true);
            missionSetupEngineersOnShipText.SetActive(true);
        }

        if (!TechSystem.IsTechUnlocked(TechSystem.Tech.IndustrialMiner)) {
            missionSetupIndustrialMinerText.SetActive(false);
            missionSetupStartingIndustrialMinerInputField.transform.parent.gameObject.SetActive(false);
            missionSetupIndustrialMinersOnShipText.SetActive(false);
        }
        else {
            missionSetupIndustrialMinerText.SetActive(true);
            missionSetupStartingIndustrialMinerInputField.transform.parent.gameObject.SetActive(true);
            missionSetupIndustrialMinersOnShipText.SetActive(true);
        }

        uI_Assistant.SetMissionObjectiveText(objective);
    }

    private void WriteOnConstructionOnShipDisplay(string name, string description, int coriumCost, int antoniumCost, int sampleCost, OnShipAssemblySO assemblyLine1 = null, OnShipAssemblySO assemblyLine2 = null, OnShipAssemblySO assemblyLine3 = null) {
        HashSet<string> unlockedBuildings = campaignHandler.GetUnlockedBuildings();
        
        // display only accesible buttons
        constructionOnShipHQ_ConstructionButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.HQ_ConstructionName));
        constructionOnShipDroneFactoryButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.DroneFactoryName));
        constructionOnShipHeavyFactoryButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.HeavyFactoryName));
        constructionOnShipOrbitalRailCannonButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.OrbitalRailCannonName));
        constructionOnShipOrbitalNukeSiloButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.OrbitalNukeSiloName));
        constructionOnShipScienceLabButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.ScienceLabName));
        constructionOnShipNetworkExpansionButton.gameObject.SetActive(unlockedBuildings.Contains(Texts.NetworkExpansionName));

        constructionOnShipAssemblyLine1InputField.text = "";
        constructionOnShipAssemblyLine2InputField.text = "";
        constructionOnShipAssemblyLine3InputField.text = "";

        buildingNameText.text = name;
        buildingDescriptionText.text = description;
        constructionErrorText.text = "";

        if (campaignHandler.GetBuildingsOnShip().Contains(name)) {
            constructionOnShipConstructBuildingButton.gameObject.SetActive(false);
            constructionOnShipBuildingCostText.SetActive(false);
            if (assemblyLine1 != null || assemblyLine2 != null || assemblyLine3 != null) {
                if (name == Texts.OrbitalRailCannonName && ResourceManager.GetDronesOnShip().TryGetValue(railcannonBullet.name, out int bullets)) {
                    buildingDescriptionText.text += "\r\nBullets in stock: " + bullets;
                }
                else if (name == Texts.OrbitalNukeSiloName && ResourceManager.GetDronesOnShip().TryGetValue(nuke.name, out int nukes)) {
                    buildingDescriptionText.text += "\r\nNuclear missiles in stock: " + nukes;
                }

                constructionOnShipAssemblyDisplay.SetActive(true);
                constructionOnShipStartConstructionButton.gameObject.SetActive(true);
                constructionOnShipBuildingOperationalText.SetActive(false);

                constructionOnShipAssemblyLine1Display.SetActive(false);
                constructionOnShipAssemblyLine2Display.SetActive(false);
                constructionOnShipAssemblyLine3Display.SetActive(false);

                int assemblyLinesWorking = 0;

                if (assemblyLine1 != null) {
                    assemblyLinesWorking++;
                    constructionOnShipAssemblyLine1Display.SetActive(true);
                    assemblyLine1NameText.text = assemblyLine1.NAME;
                    assemblyLine1DescriptionText.text = "Cost:\r\n\tCorium: " + assemblyLine1.CORIUM_COST + "\r\n\tAntonium: " + assemblyLine1.ANTONIUM_COST + "\r\n\t " + (assemblyLine1.DRONE ? "(Energy: " + assemblyLine1.ENERGY_COST + ")" : "");
                }
                if (assemblyLine2 != null) {
                    assemblyLinesWorking++;
                    constructionOnShipAssemblyLine2Display.SetActive(true);
                    assemblyLine2NameText.text = assemblyLine2.NAME;
                    assemblyLine2DescriptionText.text = "Cost:\r\n\tCorium: " + assemblyLine2.CORIUM_COST + "\r\n\tAntonium: " + assemblyLine2.ANTONIUM_COST + "\r\n\t " + (assemblyLine2.DRONE ? "(Energy: " + assemblyLine2.ENERGY_COST + ")" : "");
                }
                if (assemblyLine3 != null) {
                    assemblyLinesWorking++;
                    constructionOnShipAssemblyLine3Display.SetActive(true);
                    assemblyLine3NameText.text = assemblyLine3.NAME;
                    assemblyLine3DescriptionText.text = "Cost:\r\n\tCorium: " + assemblyLine3.CORIUM_COST + "\r\n\tAntonium: " + assemblyLine3.ANTONIUM_COST + "\r\n\t " + (assemblyLine3.DRONE ? "(Energy: " + assemblyLine3.ENERGY_COST + ")" : "");
                }

                switch (assemblyLinesWorking) {
                    case 3: { constructionOnShipAssemblyDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 200); break; }
                    case 2: { constructionOnShipAssemblyDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 200); break; }
                    case 1: { constructionOnShipAssemblyDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(234, 200); break; }
                }

                constructionOnShipStartConstructionButton.onClick.RemoveAllListeners();
                constructionOnShipStartConstructionButton.onClick.AddListener(() => {
                    Utils.GetIntFromInputField(constructionOnShipAssemblyLine1InputField, out int assemblyLine1Amount);
                    Utils.GetIntFromInputField(constructionOnShipAssemblyLine2InputField, out int assemblyLine2Amount);
                    Utils.GetIntFromInputField(constructionOnShipAssemblyLine3InputField, out int assemblyLine3Amount);

                    if (assemblyLine1Amount < 0 || assemblyLine2Amount < 0 || assemblyLine3Amount < 0) {
                        constructionErrorText.text = "ERROR: A negative amount was given.";
                        return;
                    }

                    if (assemblyLine1Amount == 0 && assemblyLine2Amount == 0 && assemblyLine3Amount == 0) {
                        return;
                    }

                    int sumCoriumCost = (assemblyLine1 != null ? assemblyLine1Amount * assemblyLine1.CORIUM_COST: 0) + (assemblyLine2 != null ? assemblyLine2Amount * assemblyLine2.CORIUM_COST : 0) + (assemblyLine3 != null ? assemblyLine3Amount * assemblyLine3.CORIUM_COST : 0);
                    int sumAntoniumCost = (assemblyLine1 != null ? assemblyLine1Amount * assemblyLine1.ANTONIUM_COST : 0) + (assemblyLine2 != null ? assemblyLine2Amount * assemblyLine2.ANTONIUM_COST : 0) + (assemblyLine3 != null ? assemblyLine3Amount * assemblyLine3.ANTONIUM_COST : 0);

                    if (ResourceManager.IsEnougResourcesOnShip(sumCoriumCost, sumAntoniumCost, 0)) {

                        confirmActionHandler.ConfirmAction(() => {

                            resourceManager.SetShipResources(ResourceManager.GetCoriumOnShip() - sumCoriumCost, ResourceManager.GetAntoniumOnShip() - sumAntoniumCost, ResourceManager.GetSampleOnShip());

                            if (assemblyLine1Amount > 0) {
                                resourceManager.AddDronesOnShip(assemblyLine1.NAME, assemblyLine1Amount);
                            }
                            if (assemblyLine2Amount > 0) {
                                resourceManager.AddDronesOnShip(assemblyLine2.NAME, assemblyLine2Amount);
                            }
                            if (assemblyLine3Amount > 0) {
                                resourceManager.AddDronesOnShip(assemblyLine3.NAME, assemblyLine3Amount);
                            }

                            UpdateResourceDisplay();

                        }, "You want to construct the following objects:\r\n" + (assemblyLine1 != null ? assemblyLine1.NAME + ": " + assemblyLine1Amount + "\r\n" : "") + (assemblyLine2 != null ? assemblyLine2.NAME + ": " + assemblyLine2Amount + "\r\n" : "") + (assemblyLine3 != null ? assemblyLine3.NAME + ": " + assemblyLine3Amount + "\r\n" : ""));

                    }
                    else {
                        constructionErrorText.text = "ERROR: Not enough resources to construct everything.";
                    }
                });
            }
            else {
                constructionOnShipAssemblyDisplay.SetActive(false);
                constructionOnShipStartConstructionButton.gameObject.SetActive(false);
                constructionOnShipBuildingOperationalText.SetActive(true);
            }
        }
        else {
            constructionOnShipAssemblyDisplay.SetActive(false);
            constructionOnShipStartConstructionButton.gameObject.SetActive(false);
            constructionOnShipBuildingOperationalText.SetActive(false);
            constructionOnShipConstructBuildingButton.gameObject.SetActive(true);
            constructionOnShipBuildingCostText.SetActive(true);

            buildingCostText.text = (coriumCost > 0 ? "Corium: " + coriumCost + "\r\n" : "") + (antoniumCost > 0 ? "Antonium: " + antoniumCost + "\r\n" : "") + (sampleCost > 0 ? "Alien Sample: " + sampleCost + "\r\n" : "");

            constructionOnShipConstructBuildingButton.onClick.RemoveAllListeners();
            constructionOnShipConstructBuildingButton.onClick.AddListener(() => { TryConstructBuilding(name, description, coriumCost, antoniumCost, sampleCost, assemblyLine1, assemblyLine2, assemblyLine3); });
        }
    }

    private void TryConstructBuilding(string name, string description, int coriumCost, int antoniumCost, int sampleCost, OnShipAssemblySO assemblyLine1 = null, OnShipAssemblySO assemblyLine2 = null, OnShipAssemblySO assemblyLine3 = null) {
        if (ResourceManager.IsEnougResourcesOnShip(coriumCost, antoniumCost, sampleCost)) {
            confirmActionHandler.ConfirmAction(() => {

                resourceManager.SetShipResources(ResourceManager.GetCoriumOnShip() - coriumCost, ResourceManager.GetAntoniumOnShip() - antoniumCost, ResourceManager.GetSampleOnShip() - sampleCost);
                campaignHandler.AddBuildingOnShip(name);
                campaignHandler.SecondMissionCompletedCheck();
                campaignHandler.ThirdMissionCompletedCheck();

                WriteOnConstructionOnShipDisplay(name, description, coriumCost, antoniumCost, sampleCost, assemblyLine1, assemblyLine2, assemblyLine3);
                UpdateResourceDisplay();

            }, "You want to construct this building: " + name);
        }
        else {
            constructionErrorText.text = "ERROR: Not enough resources to build this building.";
        }
    }

    private void StartMission() {
        Utils.GetIntFromInputField(missionSetupStartingWorkersInputField, out int startingWorker);
        Utils.GetIntFromInputField(missionSetupStartingPatrollersInputField, out int startingPatroller);
        Utils.GetIntFromInputField(missionSetupStartingProtectorsInputField, out int startingProtector);
        Utils.GetIntFromInputField(missionSetupStartingGuardianInputField, out int startingGuardian);
        Utils.GetIntFromInputField(missionSetupStartingEngineersInputField, out int startingEngineer);
        Utils.GetIntFromInputField(missionSetupStartingIndustrialMinerInputField, out int startingIndustrialMiner);

        Dictionary<string, int> dronesOnShip = ResourceManager.GetDronesOnShip();

        if (startingIndustrialMiner + startingGuardian > 1) {
            missionSetupErrorText.text = "ERROR: You can only start the game with one large drone. (This includes guardians and industruial miners)";
            return;
        }
        if (startingWorker < 0 || startingPatroller < 0 || startingProtector < 0 || startingGuardian < 0 || startingEngineer < 0 || startingIndustrialMiner < 0) {
            missionSetupErrorText.text = "ERROR: You cannot send a negative value of drones to a mission.";
            return;
        }
        if (startingWorker > dronesOnShip.GetValueOrDefault(worker.NAME, 0) || startingPatroller > dronesOnShip.GetValueOrDefault(patroller.NAME, 0) || startingProtector > dronesOnShip.GetValueOrDefault(protector.NAME, 0) || startingGuardian > dronesOnShip.GetValueOrDefault(guardian.NAME, 0) || startingEngineer> dronesOnShip.GetValueOrDefault(engineer.NAME, 0) || startingIndustrialMiner > dronesOnShip.GetValueOrDefault(industrialMiner.NAME, 0)) {
            missionSetupErrorText.text = "ERROR: You have fewer drones than you want to take on the mission, or you selected drones you do not have on the ship.";
            return;
        }
        if (!hQ_AndDroneSpawning.CheckStartingDroneEnergyConsumption(startingWorker, startingPatroller, startingProtector, startingGuardian, startingEngineer, startingIndustrialMiner)) {
            missionSetupErrorText.text = "ERROR: The HQ can't provide enough energy for the drones. (choose fewer starting drones)";
            return;
        }
        if (dronesOnShip.GetValueOrDefault(hQ.NAME, 0) <= 0) {
            missionSetupErrorText.text = "ERROR: You have no HQ to start the mission with.";
            return;
        }

        confirmActionHandler.ConfirmAction(() => {

            resourceManager.AddDronesOnShip(hQ.NAME, -1);
            resourceManager.AddDronesOnShip(worker.NAME, -1 * startingWorker);
            resourceManager.AddDronesOnShip(patroller.NAME, -1 * startingPatroller);
            resourceManager.AddDronesOnShip(protector.NAME, -1 * startingProtector);
            resourceManager.AddDronesOnShip(guardian.NAME, -1 * startingGuardian);
            resourceManager.AddDronesOnShip(engineer.NAME, -1 * startingEngineer);
            resourceManager.AddDronesOnShip(industrialMiner.NAME, -1 * startingIndustrialMiner);

            GameHandler.SetSandboxMission(false);

            campaignHandler.SetupChosenMission(startingWorker, startingPatroller, startingProtector, startingGuardian, startingEngineer, startingIndustrialMiner);

        }, "Are you ready to start the mission?");
    }
}