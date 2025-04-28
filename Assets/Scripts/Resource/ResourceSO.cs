using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ResourceSO : ScriptableObject {
    [SerializeField] public Transform PREFAB;
    [SerializeField] public int ID;
    [SerializeField] public string NAME;
    [SerializeField] public float AMOUNT;
    [SerializeField] public float MINING_TIME;
    [SerializeField] public float SIZE;
}
