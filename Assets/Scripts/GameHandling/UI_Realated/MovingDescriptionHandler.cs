using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingDescriptionHandler : MonoBehaviour{

    [SerializeField] private float timeUntilAppear;

    [SerializeField] private Camera uiCamera;

    [SerializeField] private RectTransform canvasRectTransform;

    [SerializeField] private GameObject dispay;

    [SerializeField] private GameObject titleDisplay;
    [SerializeField] private GameObject descriptionDisplay;
    [SerializeField] private GameObject costDisplay;
    [SerializeField] private GameObject statDisplay;

    [SerializeField] private BuildingSO wallSO;
    [SerializeField] private BuildingSO generatorSO;
    [SerializeField] private BuildingSO fusionReactorSO;
    [SerializeField] private BuildingSO droneFactorySO;
    [SerializeField] private BuildingSO heavyFactorySO;
    [SerializeField] private BuildingSO machinegunTurretSO;
    [SerializeField] private BuildingSO heavyTurretSO;
    [SerializeField] private BuildingSO railCannonSO;
    [SerializeField] private BuildingSO thumperSO;
    [SerializeField] private BuildingSO undergroundExplosionDeviceSO;

    private Text titleText;
    private Text descriptionText;
    private Text costText;
    private Text statText;
    private float timer = float.MinValue;

    public void WriteStatistics(ScriptableObject scriptableObject) {
        timer = timeUntilAppear;

        descriptionText.text = FindMatchingDescription(scriptableObject);

        if (scriptableObject is BuildingSO buildingSO) {

            titleText.text = buildingSO.NAME;
            costText.text = "Cost:\r\n" +
                (buildingSO.ENERGY_COST > 0 ? "\tEnergy: " + buildingSO.ENERGY_COST + "\r\n" : "") +
                (buildingSO.CORIUM_COST > 0 ? "\tCorium: " + buildingSO.CORIUM_COST + "\r\n" : "") +
                (buildingSO.ANTONIUM_COST > 0 ? "\tAntonium: " + buildingSO.ANTONIUM_COST : "");
            statText.text = "Stats:\r\n" +
                "\tHealth: " + buildingSO.HEALTH;

            if (buildingSO is TurretSO turretSO) {
                statText.text += "\r\n\tDamage: " + turretSO.DAMAGE + "\r\n" +
                    "\tCooldown: " + turretSO.COOLDOWN + " sec\r\n" +
                    "\tRange: " + turretSO.RANGE + "\r\n";
            }
        }
        else if (scriptableObject is DroneSO droneSO) {
            titleText.text = droneSO.NAME;
            costText.text = "Cost:\r\n" +
                (droneSO.ENERGY_COST > 0 ? "\tEnergy: " + droneSO.ENERGY_COST + "\r\n" : "") +
                (droneSO.CORIUM_COST > 0 ? "\tCorium: " + droneSO.CORIUM_COST + "\r\n" : "") +
                (droneSO.ANTONIUM_COST > 0 ? "\tAntonium: " + droneSO.ANTONIUM_COST : "");
            statText.text = "Stats:\r\n" +
                "\tHealth: " + droneSO.HEALTH + "\r\n" +
                "\tDamage: " + droneSO.DAMAGE + "\r\n" +
                "\tCooldown: " + droneSO.COOLDOWN + " sec\r\n" +
                "\tRange: " + droneSO.RANGE + "\r\n" +
                "\tMove speed: "+ droneSO.SPEED;
        }
    }


    public void Disappear() {
        timer = float.MinValue;
        dispay.SetActive(false);
    }

    private void Start(){
        titleText = titleDisplay.GetComponent<Text>();
        descriptionText = descriptionDisplay.GetComponent<Text>();
        costText = costDisplay.GetComponent<Text>();
        statText = statDisplay.GetComponent<Text>();
        dispay.SetActive(false);
    }

    private void Update() {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, uiCamera, out localPoint);
        dispay.transform.localPosition = localPoint;

        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && timer != float.MinValue) {
            dispay.SetActive(true);
        }
    }

    private string FindMatchingDescription(ScriptableObject scriptableObject) {
        if (scriptableObject is BuildingSO buildingSO) {
            if (buildingSO.NAME == wallSO.NAME) {
                return Texts.wallDescription;
            }
            if (buildingSO.NAME == generatorSO.NAME) {
                return Texts.generatorDescription;
            }
            if (buildingSO.NAME == fusionReactorSO.NAME) {
                return Texts.fusionReactorDescription;
            }
            if (buildingSO.NAME == droneFactorySO.NAME) {
                return Texts.droneFactoryDescription;
            }
            if (buildingSO.NAME == heavyFactorySO.NAME) {
                return Texts.heavyFactoryDescription;
            }
            if (buildingSO.NAME == machinegunTurretSO.NAME) {
                return Texts.machinegunDescription;
            }
            if (buildingSO.NAME == heavyTurretSO.NAME) {
                return Texts.heavyTurretDescription;
            }
            if (buildingSO.NAME == railCannonSO.NAME) {
                return Texts.railCannonDescription;
            }
            if (buildingSO.NAME == undergroundExplosionDeviceSO.NAME) {
                return Texts.undergroundExplosionDeviceDescription;
            }
            if (buildingSO.NAME == thumperSO.NAME) {
                return Texts.thumperDescription;
            }
        }

        return "";
    }
}
