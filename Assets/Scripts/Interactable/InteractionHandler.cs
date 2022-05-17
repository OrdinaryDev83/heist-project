using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {
    
    public LayerMask interactionLayer;
    public LayerMask propsLayer;

    public IInteractable GetInteraction(Vector2 dir, float maxDist, bool ignoreProps = true) {
        LayerMask mask = interactionLayer | (ignoreProps ? 0 : propsLayer);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir.normalized, maxDist, mask);
        foreach (var hit in hits) {
            IInteractable ib = null;
            if(hit.collider.TryGetComponent(out ib)) {
                return ib;
            }
        }
        return null;
    }

    public HealthHandler GetHealthHandler(HealthHandler our, Vector2 dir, float maxDist) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir.normalized, maxDist, interactionLayer);
        foreach (var hit in hits) {
            HealthHandler ib = null;
            if (hit.collider.TryGetComponent<HealthHandler>(out ib) && ib != our) {
                return ib;
            }
        }
        return null;
    }
}
