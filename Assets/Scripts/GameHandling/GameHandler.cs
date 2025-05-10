using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameHandler : MonoBehaviour {

    [SerializeField] private MessageSystem messageSystem;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private BuildingGridSystem buildingGridSystem;

    [SerializeField] private static float timeScale;

    public enum GameState {
        mainMenu,
        gamePaused,
        sandboxMissionSetup,
        story,
        betweenMissions,
        inMission,
        missionOver
    }

    private static GameState gameState;
    private static GameState gameStateBeforePaused;
    private static bool sandboxMission = false;
    private static bool gameCompleted;

    public static void SetGameCompleted(bool completed) {
        gameCompleted = completed;
    }

    public static bool IsGameCompleted() {
        return gameCompleted;
    }

    public static void SetTimeScale(float scale) {
        timeScale = scale;
    }

    public static GameState GetGameSate() {
        return gameState;
    }

    public static bool IsMainMenu() {
        return gameState == GameState.mainMenu;
    }
    public static bool IsGamePaused() {
        return gameState == GameState.gamePaused;
    }
    public static bool IsSandboxMissionSetup() {
        return gameState == GameState.sandboxMissionSetup;
    }
    public static bool IsInMission() {
        return gameState == GameState.inMission;
    }
    public static bool IsMissionOver() {
        return gameState == GameState.missionOver;
    }
    public static bool IsStory() {
        return gameState == GameState.story;
    }
    public static bool IsBetweenMissions() {
        return gameState == GameState.betweenMissions;
    }
    public static bool IsSandboxMission() {
        return sandboxMission;
    }

    public void Save(SaveData save) {
        save.gameState = gameStateBeforePaused;
        save.timeScale = timeScale;
        save.sandboxMission = sandboxMission;
    }

    public void Load(SaveData save) {
        timeScale = save.timeScale;
        sandboxMission = save.sandboxMission;
        switch (save.gameState) {
            case GameState.mainMenu:
                Debug.LogError("Game shouldn't be saved in this state.");
                break;
            case GameState.sandboxMissionSetup:
                Debug.LogError("Game shouldn't be saved in this state.");
                break;
            case GameState.story:
                Story();
                break;
            case GameState.betweenMissions:
                BetweenMissions();
                break;
            case GameState.inMission:
                gameState = GameState.inMission;
                break;
            case GameState.gamePaused:
                Debug.LogError("Game shouldn't be saved in this state.");
                break;
            case GameState.missionOver:
                Debug.LogError("Game shouldn't be saved in this state.");
                break;
        }
    }

    public void MainMenu() {
        MissionOver();
        gameState = GameState.mainMenu;
        StatisticsTracker.SaveStatistics();
        resourceManager.TransferResourcesToShip();
    }
    public void SandboxMissionSetup() {
        gameState = GameState.sandboxMissionSetup;
    }
    public void MissionOver() {
        gameState = GameState.missionOver;
        messageSystem.MissionOver();
        UI_Assistant.ClearGameMessages();
    }
    public void Story() {
        gameState = GameState.story;
        gameStateBeforePaused = GameState.story;
    }
    public void BetweenMissions() {
        gameState = GameState.betweenMissions;
        gameStateBeforePaused = GameState.betweenMissions;
        StatisticsTracker.SaveStatistics();
        resourceManager.TransferResourcesToShip();
        messageSystem.BetweenMissionsDisplayUpdate();
    }
    public void StartMission() {
        TimeTickSystem.ResetTimer();
        gameState = GameState.inMission;
    }
    public void GamePauseSwitch() {
        if (gameState != GameState.gamePaused) {
            buildingGridSystem.CancelBuilding();
            gameStateBeforePaused = gameState;
            gameState = GameState.gamePaused;
        } else {
            gameState = gameStateBeforePaused;
        }
    }
    public static void SetSandboxMission(bool sandboxMission) {
        GameHandler.sandboxMission = sandboxMission;
    }

    public GameState GetGameState() {
        return gameState;
    }
    public static GameState GetGameStateBeforePaused() {
        return gameStateBeforePaused;
    }

    private void Start() {
        gameState = GameState.mainMenu;
        ReferenceList.GameHandler = this;


        SaveSystem.Init();
        GameData gameData = SaveSystem.LoadGameData();
        Debug.Log(gameData);
        if (gameData != null) {
            SetGameCompleted(gameData.gameCompleted);
        }
    }

    private void Update() {
        switch (gameState) {
            case GameState.mainMenu:
                break;
            case GameState.sandboxMissionSetup:
                Time.timeScale = 0;
                break;
            case GameState.story:
                Time.timeScale = 1;
                break;
            case GameState.betweenMissions:
                Time.timeScale = 0;
                break;
            case GameState.inMission:
                if (Time.timeScale != timeScale) {
                    Time.timeScale = timeScale;
                }
                break;
            case GameState.gamePaused:
                Time.timeScale = 0;
                break;
            case GameState.missionOver:
                Time.timeScale = 0;
                break;
        }
    }

    void OnApplicationQuit() {
        SaveSystem.SaveGameData();
    }
}
