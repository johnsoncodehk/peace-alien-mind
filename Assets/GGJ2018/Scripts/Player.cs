using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float smoothTime;
	public float maxSpeed;
	public float deltaTime;
	public Signal signal;
	public Transform shooter, attractPoint;

	private Vector2 m_MoveVelocity;
	
	void Update() {
		this.LookToMouse();
		this.MoveToMouse();
	}

	private void LookToMouse() {
		Vector3 targetPosition = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 diff = targetPosition - this.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}
	private void MoveToMouse() {
		Vector3 targetPosition = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		this.transform.position = Vector2.SmoothDamp(this.transform.position, targetPosition, ref this.m_MoveVelocity, this.smoothTime, this.maxSpeed, this.deltaTime);
	}

	public IEnumerator Shoot() {
		yield return new WaitForSeconds(0.2f);

		Vector3 shootPos = this.shooter.position;
		Quaternion shootRotation = this.shooter.rotation;
		Vector3 shooterAngle = shootRotation.eulerAngles;
		shooterAngle.z += 90;
		shootRotation.eulerAngles = shooterAngle;

		var signal1 = Instantiate(this.signal, shootPos, shootRotation);
		yield return new WaitForSeconds(0.05f);
		var signal2 = Instantiate(this.signal, shootPos, shootRotation);
		yield return new WaitForSeconds(0.05f);
		var signal3 = Instantiate(this.signal, shootPos, shootRotation);

		signal1.child = signal2;
		signal2.child = signal3;
	}
}
