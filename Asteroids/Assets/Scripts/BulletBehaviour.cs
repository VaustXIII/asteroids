using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    [SerializeField] private float maxSpeed = 20f;

    private Vector2 velocity;

    private void Update() {
        var positionDelta = Time.deltaTime * velocity;
        transform.Translate(positionDelta.x, positionDelta.y, 0, Space.World);
    }

    public void Initialize(Vector2 direction) {
        velocity = maxSpeed * direction.normalized;
    }
}
