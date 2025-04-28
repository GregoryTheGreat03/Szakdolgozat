using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DroneSO : ScriptableObject{
    
    [SerializeField] public Transform prefab;
    [SerializeField] public string NAME;
    [SerializeField] public float HEALTH;
    [SerializeField] public float DAMAGE;
    [SerializeField] public float RANGE;
    [SerializeField] public float COOLDOWN;
    [SerializeField] public float SPEED;
    [SerializeField] public int ENERGY_COST;
    [SerializeField] public int CORIUM_COST;
    [SerializeField] public int ANTONIUM_COST;
    [SerializeField] public float CONSTRUCTION_TIME;
    [SerializeField] public Vector3 HEALTH_BAR_POSITION;
    [SerializeField] public Vector3 HEALTH_BAR_SIZE;

}
