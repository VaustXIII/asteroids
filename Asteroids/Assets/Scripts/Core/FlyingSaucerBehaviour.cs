using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSaucerBehaviour : MonoBehaviour, IShootable, IScoreable {
    public event System.Action<int> scored;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private int pointsForDestroying = 300;

    private Transform target;
    private Vector2 velocity;

    private void Start() {
        Initialize(FindObjectOfType<PlayerShipBehaviour>().transform);
    }

    private void Update() {
        Move();
    }

    public void Initialize(Transform target) {
        this.target = target;
    }

    public void GetShot() {
        scored?.Invoke(pointsForDestroying);
        Destroy(gameObject);
    }

    private void Move() {
        if (target == null) {
            return;
        }

        velocity = target.position - transform.position;
        velocity.Normalize();
        velocity *= maxSpeed;

        var positionDelta = Time.deltaTime * velocity;
        transform.Translate(positionDelta);
    }

}
