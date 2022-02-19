using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public static class MonoBehaviourExtensions {
    // Шорткат для запуска функции с задержкой
    public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay = 0f) {
        return mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        f();
    }
}

