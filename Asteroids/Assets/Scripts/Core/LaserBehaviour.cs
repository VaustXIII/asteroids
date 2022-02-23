using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour {
    [SerializeField] private int maxChargesCount = 3;
    [SerializeField] private float coldown = 5f;
    [SerializeField] private float beamLength = 10f;
    [SerializeField] private float beamWidth = 3f;
    [SerializeField] private float activeDuration = 0.5f;

    private Transform FirePoint => transform.parent;

    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private bool isActive = false;

    private void Awake() {
        sprite = this.GetRequiredComponentInChildren<SpriteRenderer>();
        collider = this.GetRequiredComponent<BoxCollider2D>();

        transform.localScale = new Vector3(beamWidth, beamLength, 1f);
        transform.position = FirePoint.position + 0.5f * beamLength * FirePoint.up;

        Deactivate();
    }

    private void Update() {
        HitTargets();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<IShootable>(out var target)) {
            target.GetShot();
        }
    }

    public void Shoot() {
        Activate();
        this.Invoke(() => Deactivate(), activeDuration);
    }

    private void HitTargets() {
        if (!isActive) { return; }

        var beamCentre = FirePoint.position + 0.5f * beamLength * FirePoint.up;
        var beamSize = new Vector3(beamWidth, beamLength, 1f);
        var angle = FirePoint.rotation.z;
        Physics2D.OverlapBox(beamCentre, beamSize, angle);

    }

    private void Activate() {
        isActive = true;
        sprite.enabled = true;
        collider.enabled = true;
    }

    private void Deactivate() {
        isActive = false;
        sprite.enabled = false;
        collider.enabled = false;
    }
}
