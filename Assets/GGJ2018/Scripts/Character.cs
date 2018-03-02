using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public float smoothTime;
	public float maxSpeed;
	public float deltaTime;
	public Vector2 targetPosition;
	public bool enabledMove;
	public GameObject bodyLeft, bodyRight;

	private Vector2 m_MoveVelocity;

	void Update() {
		if (!enabledMove) return;
		// this.LookTo(this.targetPosition);
		this.MoveTo(this.targetPosition);
	}

	// private void LookTo(Vector3 targetPosition) {
	// 	Vector3 diff = targetPosition - this.transform.position;
	// 	diff.Normalize();
	// 	float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	// 	this.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	// }
	private void MoveTo(Vector3 targetPosition) {
		float dx = targetPosition.x - this.transform.position.x;
		if (dx < -0.1f) {
			this.ShowBody(this.bodyLeft);
		}
		else if (dx > 0.1f) {
			this.ShowBody(this.bodyRight);
		}
		this.transform.position = Vector2.SmoothDamp(this.transform.position, targetPosition, ref this.m_MoveVelocity, this.smoothTime, this.maxSpeed, this.deltaTime);
	}
	private void ShowBody(GameObject body) {
		this.bodyLeft.SetActive(this.bodyLeft == body);
		this.bodyRight.SetActive(this.bodyRight == body);
	}
}
