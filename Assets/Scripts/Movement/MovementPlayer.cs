using UnityEngine;
using System.Collections;

public class MovementPlayer : MovementBase {

	AnimationPlayer _animPlayer;
	protected InventoryHandler Inv;

    protected override void Update() {
		base.Update();
		UpdateStep();
		inputAxis = Health.dead ? Vector2.zero : new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	protected virtual void UpdateStep() {
		if (Input.GetKey(KeyCode.LeftShift) && !Anim.isAiming) {
			step = inputAxis.normalized.magnitude * 2f;
		} else {
			step = inputAxis.normalized.magnitude;
		}
	}
}
