using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public static class MonoBehaviourExtensions {
    // Шорткат для запуска функции с задержкой
    public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay = 0f) {
        return mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    public static T[] GetRequiredComponentsInChildren<T>(this MonoBehaviour mb, int? expectedCount = null) where T : class {
        var result = mb.GetComponentsInChildren<T>();

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

