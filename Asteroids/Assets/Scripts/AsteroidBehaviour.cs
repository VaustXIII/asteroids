using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour, IShootable {
    [SerializeField] private float initialSpeedMin = 3f;
    [SerializeField] private float initialSpeedMax = 12f;

    [SerializeField] private AsteroidBehaviour childAsteroidPrefab;
    [Min(1)]
    [SerializeField] private int childrenCount;

    private Vector2 velocity;

    private void Awake() {
        Initialize(Random.onUnitSphere);
    }

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
