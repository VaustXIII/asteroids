using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour, IShootable, IScoreable {
    public event System.Action<int> scored;

    [SerializeField] private int pointsForDestroying = 100;
    [Header("Movement")]
    [SerializeField] private float initialSpeedMin = 3f;
    [SerializeField] private float initialSpeedMax = 12f;

    [Header("Breaking into smaller parts")]
    [SerializeField] private AsteroidBehaviour childAsteroidPrefab;
    [Min(1)]
    [SerializeField] private int childrenCount;

    private Vector2 velocity;


    private void Update() {
        var positionDelta = Time.deltaTime * velocity;
        transform.Translate(positionDelta.x, positionDelta.y, 0f);
    }

    public void Initialize(Vector2 direction) {
        var speed = Random.Range(initialSpeedMin, initialSpeedMax);
        velocity = speed * direction.normalized;
    }

    private void OnValidate() {
        initialSpeedMin = Mathf.Min(initialSpeedMin, initialSpeedMax);
        initialSpeedMax = Mathf.Max(initialSpeedMin, initialSpeedMax);
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        BreakDownIntoDebris();
    }

    private void BreakDownIntoDebris() {
        if (childAsteroidPrefab != null) {
            for (int childIndex = 0; childIndex < childrenCount; childIndex++) {
                var child = Instantiate(childAsteroidPrefab, transform.position, transform.rotation);
                child.Initialize(Random.onUnitSphere);
            }
        }
        Destroy(gameObject);
    }
}
