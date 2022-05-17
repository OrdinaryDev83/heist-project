using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class MovementBase : MonoBehaviour {

	protected AnimationBase Anim;
	protected HealthHandler Health;

	public float speed;
	[HideInInspector]
	public float step = 1f;

	[FormerlySerializedAs("rigidbody2D_")] public Rigidbody2D rigidbody2D;

	[HideInInspector]
	public Vector3 inputAxis;

	[HideInInspector]
	public bool lockRotation;

	protected virtual void Start() {
		Anim = GetComponent<AnimationBase>();
		Health = GetComponent<HealthHandler>();
	}

    // Update is called once per frame
    protected void FixedUpdate () {
		Move();
	}

	protected virtual void Update() {
		if (!lockRotation && !Health.dead) {
			Spine();
		}
	}

	public void SetAxis(Vector2 axis) {
		inputAxis = Health.dead ? Vector2.zero : axis;
	}

	protected void Move(){
		Vector2 force = inputAxis.normalized * ((speed * step) / 20 * Time.timeScale);
		rigidbody2D.AddForce(force, ForceMode2D.Impulse);
	}

	Quaternion _newRotation;
	public Transform sprite;
	protected void Spine(){
		if (inputAxis.magnitude > 0.1f)
			sprite.rotation = Quaternion.Slerp(sprite.rotation, _newRotation, Time.deltaTime * 7.5f);
		RotateCharacter (inputAxis.x, inputAxis.y);
	}

	protected void RotateCharacter(float v, float h){
		_newRotation = Quaternion.Euler((Mathf.Atan2(inputAxis.normalized.y, inputAxis.normalized.x) * Mathf.Rad2Deg) * Vector3.forward);
	}
}
