using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {
    [SerializeField] private float initialSpeedMin = 3f;
    [SerializeField] private float initialSpeedMax = 12f;

    private Vector2 velocity;

    private void Awake() {
        Initialize(Vector2.one);
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
}
