using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageHandler : MonoBehaviour{
    [SerializeField] Button okayButton;
    [SerializeField] Text title;
    [SerializeField] Text description;

    void Start() {
        ReferenceList.PopupMessageHandler = this;
        okayButton.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
        ReferenceList.PopupMessageHandler = this;
    }

    public void CreateMessage(string description, string title) {
        this.title.text = title;
        this.description.text = description;
        gameObject.SetActive(true);
    }
}
