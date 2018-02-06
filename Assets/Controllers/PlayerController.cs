using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rigidbody;
	private Collider collider;
	private Animator animator;
	private GameObject model;
	private AudioSource audioSource;
	private float rotation = 80;
	private float distToGround;
	private float playerSpeedMultiplier = 13.0f;
	private float playerJumpStrength = 15.0f;
	private float axisInputThreshold = 0.01f;
	private float helicopterDescendingSpeed = -1.0f;

	private float helicopterMovingSpeed = 6.0f;
	private float normalMovingSpeed = 10.0f;

	public AudioClip helicopterStopSound;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		distToGround = collider.bounds.extents.y;
		model = transform.Find ("rayman").gameObject;
	}

	public void SetRotation(float rotation) {
		this.rotation = rotation;
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f) || Mathf.Abs(rigidbody.velocity.y) < 0.1f;
	}

	bool InputTyped() {
		return Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > axisInputThreshold || Mathf.Abs (Input.GetAxisRaw ("Vertical")) > axisInputThreshold; 
	}

	float GetFlatVelocityAbsoluteAngle(Vector3 flatVelocity) {
		Vector3 forwardVector = new Vector3 (0, 0, 1.0f);
		Vector3 standardizedFlatVelocity = Quaternion.AngleAxis (0.0f, Vector3.up) * flatVelocity;
		bool right = standardizedFlatVelocity.x >= 0.0f;
		Vector3 rightVector = new Vector3 (0, 0, -1.0f);
		float angle = Vector3.Angle(forwardVector, flatVelocity);
		return right ? angle + rotation : 360f - angle + rotation;
	}

	// Update is called once per frame
	void FixedUpdate () {
		float horizontalAxis = Input.GetAxisRaw ("Horizontal");
		float verticalAxis = Input.GetAxisRaw ("Vertical");
		bool jump = Input.GetButtonDown ("Jump");
		bool isGrounded = IsGrounded ();

		Vector3 forwardFlatVelocity = new Vector3 (horizontalAxis, 0, 0);
		Vector3 sideFlatVelocity = new Vector3 (0, 0, verticalAxis);
		Vector3 resultFlatVelocity = forwardFlatVelocity + sideFlatVelocity;
		Vector3 resultVelocity = resultFlatVelocity;
		resultVelocity.Normalize ();
		resultVelocity = Quaternion.AngleAxis (rotation, Vector3.up) * resultVelocity;
		resultVelocity *= playerSpeedMultiplier;
		resultVelocity = new Vector3 (resultVelocity.x, rigidbody.velocity.y, resultVelocity.z);
		rigidbody.velocity = resultVelocity;

		bool isUsingHelicopter = animator.GetBool ("IsUsingHelicopter");

		animator.SetFloat ("FlatSpeedAbsoluteValue", new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude);
		animator.SetBool ("IsGrounded", isGrounded);
		animator.SetBool ("JumpPressed", jump);

		if (jump && isGrounded) {
			Debug.Log ("JUMPED" + rigidbody.position.x.ToString());
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, playerJumpStrength, rigidbody.velocity.z);
		}

		if (jump && !isGrounded && !isUsingHelicopter) {
			audioSource.Play ();
			animator.SetBool ("IsUsingHelicopter", true);
		}

		if (jump && isUsingHelicopter) {
			AudioSource.PlayClipAtPoint (helicopterStopSound, transform.position, 0.75f);
		}

		if (isGrounded || (jump && isUsingHelicopter)) {
			audioSource.Stop ();
			animator.SetBool ("IsUsingHelicopter", false);
		}

		if (isUsingHelicopter) {
			playerSpeedMultiplier = helicopterMovingSpeed;
			rigidbody.velocity = new Vector3 (rigidbody.velocity.x, helicopterDescendingSpeed, rigidbody.velocity.z);
		} else {
			playerSpeedMultiplier = normalMovingSpeed;
		}

		animator.SetFloat ("VerticalSpeedValue", rigidbody.velocity.y);

		if (InputTyped ()) {
			model.transform.rotation = Quaternion.Euler (model.transform.rotation.x,
				GetFlatVelocityAbsoluteAngle (resultFlatVelocity),
				model.transform.rotation.z);
		}
	}
}
