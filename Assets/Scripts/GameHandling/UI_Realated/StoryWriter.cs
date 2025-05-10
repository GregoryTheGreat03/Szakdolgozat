using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoryWriter : MonoBehaviour{

    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private MessageSystem messageSystem;

    [SerializeField] private GameObject storyTitle;
    [SerializeField] private GameObject storyText;
    [SerializeField] private GameObject storyContinueMessage;

    private Text titleTextBox;
    private Text bodyTextBox;

    private static List<string> titleList;
    private static List<string> bodyList;
    private static List<float> timePerCharacter;
    private float timer = 0;
    private static int titleCharacterIndex;
    private static int bodyCharacterIndex;

    private bool canContinue;

    public static void Save(SaveData save) {
        save.titleList = new List<string>(titleList);
        save.bodyList = new List<string>(bodyList);
        save.timePerCharacter = new List<float>(timePerCharacter);
    }

    public static void Load(SaveData save) {
        ClearLists();

        titleList = new List<string>(save.titleList);
        bodyList = new List<string>(save.bodyList);
        timePerCharacter = new List<float>(save.timePerCharacter);

        titleCharacterIndex = 0;
        bodyCharacterIndex = 0;
    }

    void Start(){
        titleTextBox = storyTitle.GetComponent<Text>();
        bodyTextBox = storyText.GetComponent<Text>();

        titleList = new List<string>();
        bodyList = new List<string>();
        timePerCharacter = new List<float>();
    }

    void Update(){
        // continue story
        if (Input.GetKeyDown(KeyCode.Space) && canContinue) {
            titleList.RemoveAt(0);
            bodyList.RemoveAt(0);
            timePerCharacter.RemoveAt(0);
            titleCharacterIndex = 0;
            bodyCharacterIndex = 0;
            canContinue = false;
            Debug.Log("continue story");

            if (titleList.Count == 0) {
                gameHandler.BetweenMissions();
            }
        }

        // skip story
        else if (Input.GetKeyDown(KeyCode.Space) && titleList != null && titleList.Count != 0 && bodyCharacterIndex != bodyList[0].Length) {
            titleCharacterIndex = titleList[0].Length;
            bodyCharacterIndex = bodyList[0].Length;
            Debug.Log("skip story");
        }

        // write out story
        if (titleList != null && titleList.Count != 0 && titleTextBox != null && bodyTextBox != null) {
            timer -= Time.deltaTime;
            while (timer <= 0f) {

                titleTextBox.text = titleList[0].Substring(0, titleCharacterIndex);
                bodyTextBox.text = bodyList[0].Substring(0, bodyCharacterIndex);
                timer += timePerCharacter[0];

                if (titleCharacterIndex < titleList[0].Length) {
                    titleCharacterIndex++;
                    storyContinueMessage.SetActive(false);
                }
                else if (bodyCharacterIndex < bodyList[0].Length) {
                    bodyCharacterIndex++;
                    storyContinueMessage.SetActive(false);
                }
                else {
                    storyContinueMessage.SetActive(true);
                    canContinue = true;
                }
            }
        }
    }

    public void WriteStory(string title, string body, float time) {
        titleTextBox.text = "";
        bodyTextBox.text = "";
        storyContinueMessage.SetActive(false);

        titleCharacterIndex = 0;
        bodyCharacterIndex = 0;
        timer = time;

        titleList.Add(title);
        bodyList.Add(body);
        timePerCharacter.Add(time);
    }

    public static void ClearLists() {
        titleList.Clear();
        bodyList.Clear();
        timePerCharacter.Clear();
    }
}
