using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSpawning : MonoBehaviour{
    [SerializeField] private ResourceSO coriumSO;
    [SerializeField] private ResourceSO antoniumSO;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private int coriumAmount;
    [SerializeField] private int antoniumAmount;

    private static List<Resource> resourceFieldsOnMap;

    public void Save(SaveData save) {
        save.resourceFieldsOnMap = new List<SaveData.ResourceSaveData>();
        foreach (Resource resource in resourceFieldsOnMap) {
            save.resourceFieldsOnMap.Add(resource.SaveResourceData());
        }
    }

    public void Load(SaveData save) {
        resourceFieldsOnMap = new List<Resource>();
        foreach (SaveData.ResourceSaveData resourceSaveData in save.resourceFieldsOnMap) {
            Transform resourceTransform = Instantiate(resourceSaveData.resourceSO.PREFAB, resourceSaveData.position, transform.rotation);
            Resource resource = resourceTransform.GetComponent<Resource>();
            resource.LoadResourceData(resourceSaveData);
            resourceFieldsOnMap.Add(resource);
        }
    }

    public static void RemoveFromResourceList(Resource resource) {
        resourceFieldsOnMap.Remove(resource);
    }

    public void SetCoriumAmount(int amount) {
        coriumAmount = amount;
    }

    public void SetAntoniumAmount(int amount) {
        antoniumAmount = amount;
    }

    public void SetMinDistance(float distance) {
        minDistance = distance;
    }

    public void SetMaxDistance(float distance) {
        maxDistance = distance;
    }

    public void InitializeMission() {

        for (int i = 0; i < coriumAmount; i++) {

            Transform coriumTransform = Instantiate(coriumSO.PREFAB);
            coriumTransform.localPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minDistance, maxDistance), Vector3.zero);
            Resource corium = coriumTransform.GetComponent<Resource>();
            resourceFieldsOnMap.Add(corium);
        }

        for (int i = 0; i < antoniumAmount; i++) {

            Transform antoniumTransform = Instantiate(antoniumSO.PREFAB);
            antoniumTransform.localPosition = Utils.GenerateRandomPosition(UnityEngine.Random.Range(minDistance, maxDistance), Vector3.zero);
            Resource antonium = antoniumTransform.GetComponent<Resource>();
            resourceFieldsOnMap.Add(antonium);
        }
    }

    private void Start() {
        resourceFieldsOnMap = new List<Resource>();
        ReferenceList.ResourceSpawning = this;
    }
}
