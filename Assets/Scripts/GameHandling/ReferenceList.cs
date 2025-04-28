using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceList{
    public static GameHandler GameHandler { get; set; }
    public static MessageSystem MessageSystem { get; set; }
    public static EnemySpawning EnemySpawning { get; set; } 
    public static CampaignHandler CampaignHandler { get; set; }
    public static ConfirmActionHandler ConfirmActionHandler { get; set; }
    public static PopupMessageHandler PopupMessageHandler { get; set; }
    public static ResourceSpawning ResourceSpawning { get; set; }
    public static ResourceManager ResourceManager { get; set; }
    public static BuildingGridSystem BuildingGridSystem { get; set; }
    public static CameraControl cameraControl { get; set; }
    public static PopupMessageHandler popupMessageHandler { get; set; }
}
