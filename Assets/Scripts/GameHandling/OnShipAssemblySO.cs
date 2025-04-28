using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OnShipAssemblySO : ScriptableObject {

    [SerializeField] public string NAME;
    [SerializeField] public int ENERGY_COST;
    [SerializeField] public int CORIUM_COST;
    [SerializeField] public int ANTONIUM_COST;
    [SerializeField] public bool DRONE;
}
