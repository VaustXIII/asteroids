using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AsteroidMovementData")]
public class AsteroidMovementData : ScriptableObject {
    public float initialSpeedMin = 3f;
    public float initialSpeedMax = 12f;
    public float initialAngularSpeedMin = 10f;
    public float initialAngularSpeedMax = 420f;

    private void OnValidate() {
        initialSpeedMin = Mathf.Min(initialSpeedMin, initialSpeedMax);
        initialSpeedMax = Mathf.Max(initialSpeedMin, initialSpeedMax);

        initialAngularSpeedMin = Mathf.Min(initialAngularSpeedMin, initialAngularSpeedMax);
        initialAngularSpeedMax = Mathf.Max(initialAngularSpeedMin, initialAngularSpeedMax);
    }
}
