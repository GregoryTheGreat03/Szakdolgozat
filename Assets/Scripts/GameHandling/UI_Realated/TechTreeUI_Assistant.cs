using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeUI_Assistant : MonoBehaviour{
    [SerializeField] private ConfirmActionHandler confirmActionHandler;
    [SerializeField] private PopupMessageHandler popupMessageHandler;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private MessageSystem messageSystem;

    [SerializeField] private GameObject betterCircuitsTechTunnel;
    [SerializeField] private GameObject DroneSpeed2TechTunnel;
    [SerializeField] private GameObject damage1TechTunnel;
    [SerializeField] private GameObject workerEfficiencyTechTunnel;
    [SerializeField] private GameObject droneHealth2TechTunnel;
    [SerializeField] private GameObject damage2TechTunnel;
    [SerializeField] private GameObject workerSorage1TechTunnel;
    [SerializeField] private GameObject buildingHealth1TechTunnel;
    [SerializeField] private GameObject industrialMinerTechTunnel;
    [SerializeField] private GameObject buildingHealth2TechTunnel;
    [SerializeField] private GameObject industrialMinerResourceMultiply1TechTunnel;
    [SerializeField] private GameObject buildingHealth3TechTunnel; 
    [SerializeField] private GameObject hQ_Energy1TechTunnel;
    [SerializeField] private GameObject hQ_Energy2TechTunnel;
    [SerializeField] private GameObject constructionSpeed1TechTunnel;
    [SerializeField] private GameObject constructionSpeed2TechTunnel;
    [SerializeField] private GameObject engineerTechTunnel;
    [SerializeField] private GameObject engineerRepairSpeed1TechTunnel;

    [SerializeField] private Button betterCircuitsButton;
    [SerializeField] private Button droneSpeed2Button;
    [SerializeField] private Button damage1Button;
    [SerializeField] private Button workerEfficiencyButton;
    [SerializeField] private Button droneHealth2Button;
    [SerializeField] private Button droneSpeed3Button;
    [SerializeField] private Button damage2Button;
    [SerializeField] private Button workerAI_Button;
    [SerializeField] private Button droneHealth3Button;
    [SerializeField] private Button workerStorage1Button;
    [SerializeField] private Button attackSpeedButton;
    [SerializeField] private Button workerStorage2Button;
    [SerializeField] private Button buildingHealth1Button;
    [SerializeField] private Button industrialMinerButton;
    [SerializeField] private Button buildingHealth2Button;
    [SerializeField] private Button industrialMinerResourceMultiply1Button;
    [SerializeField] private Button industrialMinerEfficiencyButton;
    [SerializeField] private Button buildingHealth3Button;
    [SerializeField] private Button industrialMinerResourceMultiply2Button;
    [SerializeField] private Button fusionReactorButton;
    [SerializeField] private Button hQ_Energy1Button;
    [SerializeField] private Button hQ_Energy2Button;
    [SerializeField] private Button hQ_Energy3Button;
    [SerializeField] private Button constructionSpeed1Button;
    [SerializeField] private Button constructionSpeed2Button;
    [SerializeField] private Button constructionSpeed3Button;
    [SerializeField] private Button engineerButton;
    [SerializeField] private Button engineerRepairSpeed1Button;
    [SerializeField] private Button engineerRange1Button;
    [SerializeField] private Button engineerRepairSpeed2Button;


    public void UpdateDisplay() {
        UpdateTechTunnelVisuals(betterCircuitsTechTunnel, TechSystem.Tech.BetterCircuits);
        UpdateTechTunnelVisuals(DroneSpeed2TechTunnel, TechSystem.Tech.DroneSpeed2);
        UpdateTechTunnelVisuals(damage1TechTunnel, TechSystem.Tech.Damage1);
        UpdateTechTunnelVisuals(workerEfficiencyTechTunnel, TechSystem.Tech.WorkerEfficiency);
        UpdateTechTunnelVisuals(droneHealth2TechTunnel, TechSystem.Tech.DroneHealth2);
        UpdateTechTunnelVisuals(damage2TechTunnel, TechSystem.Tech.Damage2);
        UpdateTechTunnelVisuals(workerSorage1TechTunnel, TechSystem.Tech.WorkerStorage1);
        UpdateTechTunnelVisuals(buildingHealth1TechTunnel, TechSystem.Tech.BuildingHealth1);
        UpdateTechTunnelVisuals(industrialMinerTechTunnel, TechSystem.Tech.IndustrialMiner);
        UpdateTechTunnelVisuals(buildingHealth2TechTunnel, TechSystem.Tech.BuildingHealth2);
        UpdateTechTunnelVisuals(industrialMinerResourceMultiply1TechTunnel, TechSystem.Tech.IndustrialMinerResourceMultiply1);
        UpdateTechTunnelVisuals(buildingHealth3TechTunnel, TechSystem.Tech.BuildingHealth3);
        UpdateTechTunnelVisuals(hQ_Energy1TechTunnel, TechSystem.Tech.HQ_Energy1);
        UpdateTechTunnelVisuals(hQ_Energy2TechTunnel, TechSystem.Tech.HQ_Energy2);
        UpdateTechTunnelVisuals(buildingHealth3TechTunnel, TechSystem.Tech.BuildingHealth3);
        UpdateTechTunnelVisuals(constructionSpeed1TechTunnel, TechSystem.Tech.ConstructionSpeed1);
        UpdateTechTunnelVisuals(constructionSpeed2TechTunnel, TechSystem.Tech.ConstructionSpeed2);
        UpdateTechTunnelVisuals(engineerTechTunnel, TechSystem.Tech.Engineer);
        UpdateTechTunnelVisuals(engineerRepairSpeed1TechTunnel, TechSystem.Tech.EngineerRepairSpeed1);

        UpdateButtonVisual(betterCircuitsButton, TechSystem.Tech.BetterCircuits);
        UpdateButtonVisual(droneSpeed2Button, TechSystem.Tech.DroneSpeed2);
        UpdateButtonVisual(damage1Button, TechSystem.Tech.Damage1);
        UpdateButtonVisual(workerEfficiencyButton, TechSystem.Tech.WorkerEfficiency);
        UpdateButtonVisual(droneHealth2Button, TechSystem.Tech.DroneHealth2);
        UpdateButtonVisual(droneSpeed3Button, TechSystem.Tech.DroneSpeed3);
        UpdateButtonVisual(damage2Button, TechSystem.Tech.Damage2);
        UpdateButtonVisual(workerAI_Button, TechSystem.Tech.WorkerAI);
        UpdateButtonVisual(droneHealth3Button, TechSystem.Tech.DroneHealth3);
        UpdateButtonVisual(workerStorage1Button, TechSystem.Tech.WorkerStorage1);
        UpdateButtonVisual(attackSpeedButton, TechSystem.Tech.AttackSpeed);
        UpdateButtonVisual(workerStorage2Button, TechSystem.Tech.WorkerStorage2);
        UpdateButtonVisual(buildingHealth1Button, TechSystem.Tech.BuildingHealth1);
        UpdateButtonVisual(industrialMinerButton, TechSystem.Tech.IndustrialMiner);
        UpdateButtonVisual(buildingHealth2Button, TechSystem.Tech.BuildingHealth2);
        UpdateButtonVisual(industrialMinerResourceMultiply1Button, TechSystem.Tech.IndustrialMinerResourceMultiply1);
        UpdateButtonVisual(industrialMinerEfficiencyButton, TechSystem.Tech.IndustrialMinerEfficiency);
        UpdateButtonVisual(buildingHealth3Button, TechSystem.Tech.BuildingHealth3);
        UpdateButtonVisual(hQ_Energy1Button, TechSystem.Tech.HQ_Energy1);
        UpdateButtonVisual(industrialMinerResourceMultiply2Button, TechSystem.Tech.IndustrialMinerResourceMultiply2);
        UpdateButtonVisual(hQ_Energy2Button, TechSystem.Tech.HQ_Energy2);
        UpdateButtonVisual(hQ_Energy3Button, TechSystem.Tech.HQ_Energy3);
        UpdateButtonVisual(fusionReactorButton, TechSystem.Tech.FusionReactor);
        UpdateButtonVisual(constructionSpeed1Button, TechSystem.Tech.ConstructionSpeed1);
        UpdateButtonVisual(constructionSpeed2Button, TechSystem.Tech.ConstructionSpeed2);
        UpdateButtonVisual(constructionSpeed3Button, TechSystem.Tech.ConstructionSpeed3);
        UpdateButtonVisual(engineerButton, TechSystem.Tech.Engineer);
        UpdateButtonVisual(engineerRepairSpeed1Button, TechSystem.Tech.EngineerRepairSpeed1);
        UpdateButtonVisual(engineerRange1Button, TechSystem.Tech.EngineerRange1);
        UpdateButtonVisual(engineerRepairSpeed2Button, TechSystem.Tech.EngineerRepairSpeed2);
    }

    private void Start() {
        betterCircuitsButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.BetterCircuits, Texts.betterCircuitsName, Texts.betterCircuitsDescription, 100, 10, 2));
        droneSpeed2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.DroneSpeed2, Texts.droneSpeed2Name, Texts.droneSpeed2Description, 140, 60, 0));
        damage1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.Damage1, Texts.damage1Name, Texts.damage1Description, 200, 25, 2));
        workerEfficiencyButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.WorkerEfficiency, Texts.workerEfficiencyName, Texts.workerEfficiencDescription, 465,170, 5));
        droneHealth2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.DroneHealth2, Texts.droneHealth2Name, Texts.droneHealth2Description, 230, 12, 0));
        droneSpeed3Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.DroneSpeed3, Texts.droneSpeed3Name, Texts.droneSpeed3Description, 200, 100, 0));
        damage2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.Damage2, Texts.damage2Name, Texts.damage2Description, 500, 40, 8));
        workerAI_Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.WorkerAI, Texts.workerAI_Name, Texts.workerAI_Description, 165, 300, 22));
        droneHealth3Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.DroneHealth3, Texts.droneHealth3Name, Texts.droneHealth3Description, 560, 28, 2));
        workerStorage1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.WorkerStorage1, Texts.workerStorage1Name, Texts.workerStorage1Description, 600, 32, 0));
        attackSpeedButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.AttackSpeed, Texts.attackSpeedName, Texts.attackSpeedDescription, 600, 320, 18));
        workerStorage2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.WorkerStorage2, Texts.workerStorage2Name, Texts.workerStorage2Description, 1285, 48, 0));
        buildingHealth1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.BuildingHealth1, Texts.buildingHealth1Name, Texts.buildingHealth1Description, 455, 24, 0));
        industrialMinerButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.IndustrialMiner, Texts.industrialMinerName, Texts.industrialMinerDescription, 390, 45, 3));
        buildingHealth2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.BuildingHealth2, Texts.buildingHealth2Name, Texts.buildingHealth2Description, 968, 72, 3));
        industrialMinerResourceMultiply1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.IndustrialMinerResourceMultiply1, Texts.industrialMinerResourceMultiply1Name, Texts.industrialMinerResourceMultiply1Description, 800, 300, 0));
        industrialMinerEfficiencyButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.IndustrialMinerEfficiency, Texts.industrialMinerEfficiencyName, Texts.industrialMinerEfficiencyDescription, 1430, 410, 3));
        buildingHealth3Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.BuildingHealth3, Texts.buildingHealth1Name, Texts.buildingHealth1Description, 1580, 46, 5));
        industrialMinerResourceMultiply2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.IndustrialMinerResourceMultiply2, Texts.industrialMinerResourceMultiply2Name, Texts.industrialMinerResourceMultiply2Description, 2150, 430, 0));
        fusionReactorButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.FusionReactor, Texts.fusionReactorUpgradeName, Texts.fusionReactorUpgradeDescription, 600, 210, 3));
        hQ_Energy1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.HQ_Energy1, Texts.hQ_Energy1Name, Texts.hQ_Energy1Description, 350, 130, 0));
        hQ_Energy2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.HQ_Energy2, Texts.hQ_Energy2Name, Texts.hQ_Energy2Description, 700, 260, 0));
        hQ_Energy3Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.HQ_Energy3, Texts.hQ_Energy3Name, Texts.hQ_Energy3Description, 1560, 530, 0));
        constructionSpeed1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.ConstructionSpeed1, Texts.constructionSpeed1Name, Texts.constructionSpeed1Description, 350, 40, 0));
        constructionSpeed2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.ConstructionSpeed2, Texts.constructionSpeed2Name, Texts.constructionSpeed2Description, 600, 70, 0));
        constructionSpeed3Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.ConstructionSpeed3, Texts.constructionSpeed3Name, Texts.constructionSpeed3Description, 900, 100, 0));
        engineerButton.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.Engineer, Texts.engineerName, Texts.engineerDescription, 100, 150, 10));
        engineerRepairSpeed1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.EngineerRepairSpeed1, Texts.engineerRepairSpeed1Name, Texts.engineerRepairSpeed1Description, 280, 190, 3));
        engineerRange1Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.EngineerRange1, Texts.engineerRange1Name, Texts.engineerRange1Description, 200, 180, 2));
        engineerRepairSpeed2Button.onClick.AddListener(() => TryUnlockTech(TechSystem.Tech.EngineerRepairSpeed2, Texts.engineerRepairSpeed2Name, Texts.engineerRepairSpeed2Description, 560, 370, 4));
    }

    private void TryUnlockTech(TechSystem.Tech tech, string name, string description, int corium, int antonium, int sample) {
        if (TechSystem.IsTechUnlocked(tech)) {
            return;
        } else if (TechSystem.IsTechUnlockable(tech)) {
            description += "\n\nCost:\n" + (corium > 0 ? "\tcorium: " + corium : "") + (antonium > 0 ? "\n\tantonium: " + antonium : "") + (sample > 0 ? "\n\tsample: " + sample : "");
            confirmActionHandler.ConfirmAction(() => {
                if (ResourceManager.GetCoriumOnShip() >= corium && ResourceManager.GetAntoniumOnShip() >= antonium && ResourceManager.GetSampleOnShip() >= sample) {
                    resourceManager.SetShipResources(ResourceManager.GetCoriumOnShip() - corium, ResourceManager.GetAntoniumOnShip() - antonium, ResourceManager.GetSampleOnShip() - sample);
                    TechSystem.UnlockTech(tech);
                    messageSystem.BetweenMissionsDisplayUpdate();
                    UpdateDisplay();
                } 
                else {
                    popupMessageHandler.CreateMessage("You don't have enough resources on the ship, to test this technology.", "Error");
                }
            }, description, name, height:500);
        }
    }

    private void UpdateButtonVisual(Button button, TechSystem.Tech tech) {
        if (TechSystem.IsTechUnlocked(tech)) {
            button.gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            button.gameObject.GetComponentInChildren<Text>().color = new Color32(0, 255, 0, 255);
        } 
        else if (TechSystem.IsTechUnlockable(tech)) {
            button.gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            button.gameObject.GetComponentInChildren<Text>().color = new Color32(0, 100, 0, 255);
        }
        else {
            button.gameObject.GetComponent<Image>().color = new Color32(0, 100, 0, 255);
            button.gameObject.GetComponentInChildren<Text>().color = new Color32(0, 100, 0, 255);
        }
    }

    private void UpdateTechTunnelVisuals(GameObject tunnel, TechSystem.Tech tech) {
        if (TechSystem.IsTechUnlocked(tech)) {
            foreach (Transform lane in tunnel.transform) {
                lane.gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            }
        }
        else {
            foreach (Transform lane in tunnel.transform) {
                lane.gameObject.GetComponent<Image>().color = new Color32(0, 100, 0, 255);
            }
        }
    }
}
