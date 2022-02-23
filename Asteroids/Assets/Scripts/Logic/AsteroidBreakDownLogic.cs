using UnityEngine;

public class AsteroidBreakDownLogic {
    private AsteroidBreakdownData data;

    public AsteroidBreakDownLogic(AsteroidBreakdownData data) {
        this.data = data;
    }

    public void BreakDownIntoDebris(
        System.Func<Spawnable<Vector2>, Vector3, Quaternion, Spawnable<Vector2>> instatiateFunction,
        Vector3 position,
        Quaternion rotation
    ) {
        if (data != null) {
            for (int childIndex = 0; childIndex < data.childrenCount; childIndex++) {
                var child = instatiateFunction(data.childAsteroidPrefab, position, rotation);
                child.Initialize(Random.onUnitSphere);
            }
        }
    }
}
