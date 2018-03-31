using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTools : MonoBehaviour {

	public bool Raycast(Vector3 origin, Vector3 direction, Color? color = null) {
		RaycastHit raycastHit = new RaycastHit ();
		return Raycast (origin, direction, out raycastHit, color);
	}

	public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, Color? color = null) {
		if (color != null) {
			Debug.DrawRay (origin, direction, (Color)color);
		}
		return Physics.Raycast (origin, direction, out hitInfo, direction.magnitude);
	}

	public bool SideRaycast(int count, Vector3 origin, Vector3 castDirection, float distance, Color? color = null) {
		RaycastHit raycastHit;
		Vector3 direction;
		return SideRaycast (count, origin, castDirection, distance, out raycastHit, out direction, color);
	}

	public bool SideRaycast(int count, Vector3 origin, Vector3 castDirection,
		float distance, out RaycastHit hitInfo, out Vector3 direction, Color? color = null) {
		float angleSpread = 50;
		float halfAngleSpread = angleSpread / 2;
		if (color != null) {
			for (int i = 0; i < count; i++) {
				Vector3 currentDirection = Quaternion.AngleAxis ((halfAngleSpread / count) * i, Vector3.up) * castDirection.normalized * distance;
				Debug.DrawRay (origin, currentDirection * distance, (Color)color);
			}
			for (int i = 0; i < count; i++) {
				Vector3 currentDirection = Quaternion.AngleAxis ((-halfAngleSpread / count) * i, Vector3.up) * castDirection.normalized * distance;
				Debug.DrawRay (origin, currentDirection * distance, (Color)color);
			}
		}
		for (int i = 0; i < count; i++) {
			Vector3 currentDirection = Quaternion.AngleAxis ((halfAngleSpread / count) * i, Vector3.up) * castDirection.normalized * distance;
			bool collided = Physics.Raycast (origin, currentDirection, out hitInfo, distance);
			if (collided) {
				direction = currentDirection;
				return true;
			}
		}
		for (int i = 0; i < count; i++) {
			Vector3 currentDirection = Quaternion.AngleAxis ((-halfAngleSpread / count) * i, Vector3.up) * castDirection.normalized * distance;
			bool collided = Physics.Raycast (origin, currentDirection, out hitInfo, distance);
			if (collided) {
				direction = currentDirection;
				return true;
			}
		}
		hitInfo = new RaycastHit ();
		direction = new Vector3 ();
		return false;
	}

	public float GetFlatVelocityAbsoluteAngle(Vector3 flatVelocity, float rotation) {
		Vector3 forwardVector = new Vector3 (0, 0, 1.0f);
		Vector3 standardizedFlatVelocity = Quaternion.AngleAxis (0.0f, Vector3.up) * flatVelocity;
		bool right = standardizedFlatVelocity.x >= 0.0f;
		Vector3 rightVector = new Vector3 (0, 0, -1.0f);
		float angle = Vector3.Angle(forwardVector, flatVelocity);
		return right ? angle + rotation : 360f - angle + rotation;
	}

}
