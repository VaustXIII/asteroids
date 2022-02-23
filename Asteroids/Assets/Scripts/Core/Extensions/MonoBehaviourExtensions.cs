using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public static class MonoBehaviourExtensions {
    // Шорткат для запуска функции с задержкой
    public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay = 0f) {
        return mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    public static T GetRequiredComponent<T>(this MonoBehaviour mb) where T : class {
        var result = mb.GetComponent<T>();

        Assert.IsNotNull<T>(result, $"{mb.name} needs a {typeof(T)} component");

        return result;
    }

    public static T[] GetRequiredComponentsInChildren<T>(
        this MonoBehaviour mb, int? expectedCount = null, bool includeInactive = true
    ) where T : class {
        var result = mb.GetComponentsInChildren<T>(includeInactive);

        if (expectedCount != null) {
            Assert.AreEqual<int>(
                expectedCount.Value, result.Length,
                $"{mb.name} needs {expectedCount.Value} {typeof(T)} components in children, but got {result.Length}"
            );
        }

        return result;
    }

    public static T GetRequiredComponentInChildren<T>(this MonoBehaviour mb) where T : class {
        var components = mb.GetRequiredComponentsInChildren<T>(1);

        return components[0];
    }

    private static IEnumerator InvokeRoutine(Action f, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        f();
    }
}

