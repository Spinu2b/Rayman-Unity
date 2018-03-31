using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Animator animator;
	private GameObject model;
	private PhysicsTools physicsTools;
	private InputTools inputTools;

	private float gravityStrength = 0.3f;
	private float rotation = 0f;
	private float playerMovementSpeed = 0.3f;
	private Vector3 playerVelocity = new Vector3(0, 0, 0);

	private Vector3 groundRay = new Vector3 (0, -1.0f, 0);

	void Start () {
		animator = GetComponent<Animator> ();
		physicsTools = GetComponent<PhysicsTools> ();
		inputTools = GetComponent<InputTools> ();
		model = transform.Find ("rayman").gameObject;
	}

	bool WallRaycast(Vector3 origin, Vector3 castDirection, out RaycastHit raycastHit, out Vector3 direction) {
		raycastHit = new RaycastHit ();
		direction = new Vector3 ();
		int iterations = 30;
		float length = 1.0f;
		return physicsTools.SideRaycast (iterations, origin + (Vector3.up * 0.3f), castDirection, length, out raycastHit, out direction, Color.green);
	}

	bool DirectionalRaycast(Vector3 origin, Vector3 direction) {
		return physicsTools.Raycast (origin, direction);
	}

	bool GroundRaycast(out RaycastHit raycastHit) {
		return physicsTools.Raycast (transform.position, groundRay, out raycastHit, Color.green);
	}

	void ApplyGravity() {
		RaycastHit raycastHit;
		bool isGrounded = GroundRaycast (out raycastHit);
		if (!isGrounded) {
			transform.position = new Vector3 (transform.position.x, transform.position.y - gravityStrength, transform.position.z); 
		} else {
			float positionYOffset = 0.8f;
			transform.position = new Vector3(transform.position.x, raycastHit.point.y + positionYOffset, transform.position.z);
		}
	}

	bool AreVectorsDirectionsClose(Vector3 vecA, Vector3 vecB, float angleTolerance) {
		return Mathf.Abs(Vector3.Angle (vecA, vecB)) <= angleTolerance;
	}

	void HandleMovement() {
		float horizontalAxis = inputTools.GetAxisRaw ("Horizontal");
		float verticalAxis = inputTools.GetAxisRaw ("Vertical");
		Vector3 forwardFlatVelocity = new Vector3 (horizontalAxis, 0, 0);
		Vector3 sideFlatVelocity = new Vector3 (0, 0, verticalAxis);
		Vector3 resultFlatVelocity = forwardFlatVelocity + sideFlatVelocity;
		Vector3 resultVelocity = resultFlatVelocity;
		resultVelocity.Normalize ();
		resultVelocity = Quaternion.AngleAxis (rotation, Vector3.up) * resultVelocity;
		resultVelocity = resultVelocity * playerMovementSpeed;
		resultVelocity = new Vector3 (resultVelocity.x, 0, resultVelocity.z);

		animator.SetFloat ("FlatSpeedAbsoluteValue", resultVelocity.magnitude);

		//check if collision with wall in given direction
		RaycastHit raycastHit;
		Vector3 collisionDirection;
		bool collided = WallRaycast (transform.position, resultVelocity, out raycastHit, out collisionDirection);

		if (!collided) {
			transform.position += resultVelocity;
		}
		if (inputTools.InputTyped ()) {
			model.transform.rotation = Quaternion.Euler (model.transform.rotation.x,
				physicsTools.GetFlatVelocityAbsoluteAngle (resultFlatVelocity, rotation),
				model.transform.rotation.z);
		}
	}

	public void SetRotation(float rotation) {
		this.rotation = rotation;
	}

	void FixedUpdate () {
		ApplyGravity ();
		HandleMovement ();
	}
}
