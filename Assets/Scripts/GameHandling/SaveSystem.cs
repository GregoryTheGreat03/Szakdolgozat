using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class SaveSystem{

    private static string GAME_DATA_NAME = "GameData.txt";

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static readonly string GAME_DATA_FOLDER = Application.dataPath + "/GameData/";

    public static void Init() {
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        if (!Directory.Exists(GAME_DATA_FOLDER)) {
            Directory.CreateDirectory(GAME_DATA_FOLDER);
        }
    }

    public static void Save(string fileName) {
        SaveData save = new SaveData();

        if (TutorialHandler.GetStepsCompleted() != 0) {
            ReferenceList.PopupMessageHandler.CreateMessage("You can't save inside of the tutorial simulation. Finish it first.", "Warning!");
            return;
        }

        // saving the game elements and statistics
        Hivemind.Save(save);
        ReferenceList.cameraControl.Save(save);
        TimeTickSystem.Save(save);
        ReferenceList.ResourceManager.Save(save);
        ReferenceList.EnemySpawning.Save(save);
        StatisticsTracker.Save(save);
        StoryWriter.Save(save);
        TechSystem.Save(save);
        ReferenceList.GameHandler.Save(save);
        ReferenceList.CampaignHandler.Save(save);
        ReferenceList.ResourceSpawning.Save(save);


        string saveDataJSON = JsonUtility.ToJson(save, true);
        //Debug.Log(saveDataJSON);

        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
        foreach (FileInfo fileInfo in saveFiles) {
            if (fileInfo.Name == fileName) {
                ReferenceList.ConfirmActionHandler.ConfirmAction(() => {
                    File.WriteAllText(SAVE_FOLDER + fileName, saveDataJSON);
                    ReferenceList.PopupMessageHandler.CreateMessage("Game saved successfully.", "System Message");
                    }, "With this action you will override a save file. Do you wish to proceed?");
                return;
            }
        }

        File.WriteAllText(SAVE_FOLDER + fileName, saveDataJSON);
        ReferenceList.PopupMessageHandler.CreateMessage("Game saved successfully.", "System Message");
        Debug.Log("Game saved.");
    }

    public static IEnumerator Load(string fileName) {
        if (!GetFileNames(SAVE_FOLDER).Contains(fileName)) {
            Debug.LogError("The file you want to load does not exist.");
            yield return null;
        }

        string saveDataJSON = File.ReadAllText(fileName);
        SaveData save = JsonUtility.FromJson<SaveData>(saveDataJSON);

        ReferenceList.MessageSystem.MissionOver();

        yield return new WaitForEndOfFrame();

        // loading the game elements and statistics
        try {
            Hivemind.Load(save);
            ReferenceList.cameraControl.Load(save);
            TechSystem.Load(save);
            ReferenceList.CampaignHandler.Load(save);
            ReferenceList.ResourceSpawning.Load(save);
            ReferenceList.ResourceManager.Load(save);
            ReferenceList.EnemySpawning.Load(save);
            StatisticsTracker.Load(save);
            StoryWriter.Load(save);
            ReferenceList.GameHandler.Load(save);
            TimeTickSystem.Load(save);
        }
        catch {
            ReferenceList.GameHandler.MainMenu();
            ReferenceList.PopupMessageHandler.CreateMessage("Game could not be loaded, probably because it's from an older version.", "Error");
        }
        //Debug.Log(ResourceManager.GetEnergy() + " load ended");

        UI_Assistant.ClearGameMessages();
    }

    public static void SaveGameData() {
        GameData gameData = new GameData();
        gameData.gameCompleted = GameHandler.IsGameCompleted();

        string gameDataJSON = JsonUtility.ToJson(gameData, true);

        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER); 
        File.WriteAllText(GAME_DATA_FOLDER + GAME_DATA_NAME, gameDataJSON);
    }

    public static GameData LoadGameData() {
        if (!GetFileNames(GAME_DATA_FOLDER).Contains((GAME_DATA_FOLDER + GAME_DATA_NAME).Replace("/", "\\"))) {
            return null;
        }

        string gameDataJSON = File.ReadAllText(GAME_DATA_FOLDER + GAME_DATA_NAME);
        return JsonUtility.FromJson<GameData>(gameDataJSON);
    }

    public static List<string> GetFileNames(string folderPath) {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");

        List<string> saveFileNames = new List<string>();

        foreach (FileInfo fileInfo in saveFiles) { 
            saveFileNames.Add(fileInfo.FullName);
        }

        return saveFileNames;
    }

    public static List<FileInfo> GetSaveFileInfos() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        return directoryInfo.GetFiles("*.txt").ToList();
    }
}
