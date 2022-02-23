using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AsteroidBreakdownData")]
public class AsteroidBreakdownData : ScriptableObject {
    public AsteroidBehaviour childAsteroidPrefab;
    [Min(1)]
    public int childrenCount = 2;

}
