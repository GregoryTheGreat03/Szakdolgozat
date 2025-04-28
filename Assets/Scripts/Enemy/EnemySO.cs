using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject{
    [SerializeField] public Transform prefab;
    [SerializeField] public string NAME;
    [SerializeField] public float HEALTH;
    [SerializeField] public float DAMAGE;
    [SerializeField] public float RANGE;
    [SerializeField] public float COOLDOWN;
    [SerializeField] public float SPEED;
    [SerializeField] public float SIZE;
    [SerializeField] public float AGRO_RANGE;
    [SerializeField] public float WAVE_WEIGHT; // Shows how many points one unit takes up from the wave.
    [SerializeField] public float FIRST_SPAWNING_WAVE;
}
