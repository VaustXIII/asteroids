using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AsteroidBreakdownData")]
public class AsteroidBreakdownData : ScriptableObject {
    public Spawnable<Vector2> childAsteroidPrefab;
    [Min(1)]
    public int childrenCount = 2;

}
