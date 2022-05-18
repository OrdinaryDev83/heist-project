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
    protected virtual void FixedUpdate () {
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

	protected void Move()
	{
		float maxAcceleration = 5f;
		Vector2 force = inputAxis.normalized * (maxAcceleration * step * Time.timeScale);

		ApplyForceToReachVelocity(rigidbody2D, force, 7f, ForceMode2D.Force);
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
	
	public static void ApplyForceToReachVelocity(Rigidbody2D rigidbody, Vector3 velocity, float force = 1, ForceMode2D mode = ForceMode2D.Force)
	{
		if (force == 0 || velocity.magnitude == 0)
			return;

		velocity += velocity.normalized * (0.2f * rigidbody.drag);

		//force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
		var mass = rigidbody.mass;
		force = Mathf.Clamp(force, -mass / Time.fixedDeltaTime, mass / Time.fixedDeltaTime);

		//dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
		if (rigidbody.velocity.magnitude == 0)
		{
			rigidbody.AddForce(velocity * force, mode);
		}
		else
		{
			var velocityProjectedToTarget = (velocity.normalized * Vector2.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
			rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
		}
	}

}
