using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingSO : ScriptableObject {

    [SerializeField] public Transform prefab;
    [SerializeField] public string NAME;
    [SerializeField] public float HEALTH;
    [SerializeField] public float SIZE;
    [SerializeField] public int ENERGY_COST;
    [SerializeField] public int CORIUM_COST;
    [SerializeField] public int ANTONIUM_COST;
    [SerializeField] public float CONSTRUCTION_TIME;
}
