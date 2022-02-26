using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    public float maxSpeed = 10f;
    public float acceleration = 1f;
    public float turnRate = 200f;
    public float frictionRate = .2f;
}
