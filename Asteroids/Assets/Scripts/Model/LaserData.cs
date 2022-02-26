using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LaserData")]
public class LaserData : ScriptableObject {
    public int maxChargesCount = 3;
    public float chargeCooldown = 5f;
    public float beamLength = 10f;
    public float beamWidth = 3f;
    public float activeDuration = 0.1f;
}
