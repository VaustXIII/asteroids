using UnityEngine;

public static class RandomExt {
    public static bool Bool() {
        return Random.Range(0, 2) == 0;
    }
}
