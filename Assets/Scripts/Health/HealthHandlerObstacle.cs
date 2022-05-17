using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandlerObstacle : HealthHandler {

    public SpriteRenderer top;
    public BoxCollider2D box;

    public override void OnDeath() {
        Destroy(gameObject);
    }
}
