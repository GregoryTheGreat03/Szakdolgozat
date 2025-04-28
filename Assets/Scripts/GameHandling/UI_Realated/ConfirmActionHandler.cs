using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmActionHandler : MonoBehaviour{

    [SerializeField] Button ConfirmButton;
    [SerializeField] Button CancelButton;
    [SerializeField] Text title;
    [SerializeField] Text description;
    [SerializeField] GameObject background;

    void Start(){
        CancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        ReferenceList.ConfirmActionHandler = this;
        gameObject.SetActive(false);
    }

    public void ConfirmAction(Action action, string description = "You will lose your progress if you didn't save beforehand.", string title = "Alert!", string cancel = "cancel", string confirm = "confirm", int height = 310) {
        this.title.text = title;
        this.description.text = description;

        if (title.Length >= 38) {
            this.title.fontSize = 25;
        } else if (title.Length >= 16) {
            this.title.fontSize = 30;
        } else {
            this.title.fontSize = 35;
        }

            background.GetComponent<RectTransform>().sizeDelta = new Vector2(490, height);

        ConfirmButton.GetComponentInChildren<Text>().text = confirm;
        CancelButton.GetComponentInChildren<Text>().text = cancel;

        if (GameHandler.GetGameStateBeforePaused() != GameHandler.GameState.mainMenu && GameHandler.GetGameStateBeforePaused() != GameHandler.GameState.sandboxMissionSetup) {
            gameObject.SetActive(true);
            ConfirmButton.onClick.RemoveAllListeners();
            ConfirmButton.onClick.AddListener(() => gameObject.SetActive(false));
            ConfirmButton.onClick.AddListener(() => action());
        }
        else { 
            action();
        }
    }
}
