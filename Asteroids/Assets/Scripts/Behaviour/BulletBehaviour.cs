using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    [SerializeField] private AsteroidMovementData movementData;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float lifeTime = 0.5f;

    private Coroutine selfDestructCoroutine;

    private LinearMovementLogic movementLogic;

    private void Update() {
        transform.Translate(movementLogic.GetPositionDelta(Time.deltaTime), Space.World);
    }

    public void Initialize(Vector2 direction) {
        movementLogic = new LinearMovementLogic(movementData, direction);
        selfDestructCoroutine = this.Invoke(SelfDestruct, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<IShootable>(out var target)) {
            target.GetShot();
            SelfDestruct();
        }
    }

    private void SelfDestruct() {
        if (selfDestructCoroutine != null) {
            StopCoroutine(selfDestructCoroutine);
            selfDestructCoroutine = null;
        }
        Destroy(gameObject);
    }
}
