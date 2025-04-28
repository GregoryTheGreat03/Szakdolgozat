using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringInputHandler : MonoBehaviour{
    [SerializeField] Button confirmButton;
    [SerializeField] Text description;
    [SerializeField] InputField inputField;

    private string result;

    void Start() {
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public string GetResult() {
        return result;
    }

    public void CreateGetStringInput(Action action, string description) {
        this.description.text = description;
        gameObject.SetActive(true);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(() => result = inputField.text);
        confirmButton.onClick.AddListener(() => action());
    }
}
