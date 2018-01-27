using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static List<Player> instances = new List<Player>();

	public float smoothTime;
	public float maxSpeed;
	public float deltaTime;
	public Signal signal;
	public Transform shooter, attractPoint;
	public bool isPlayerOne {
		get { return Player.instances[0] == this; }
	}
	public bool isNewPlayer;

	private Vector2 m_MoveVelocity;
	
	void Awake() {
		Player.instances.Add(this);
	}
	void Update() {
		if (this.isNewPlayer) {
			if (Vector3.Distance(this.transform.position, new Vector3(0, 0, 0)) > 10) {
				this.LookTo(Vector3.zero);
				this.MoveTo(Vector3.zero);
				return;
			}
			this.isNewPlayer = false;
		}
		if (this.isPlayerOne) {
			this.LookToMouse();
			this.MoveToMouse();
		}
		else {
			this.LookToInput();
			this.MoveToInput();
		}
	}

	private void LookToMouse() {
		Vector3 targetPosition = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		this.LookTo(targetPosition);
	}
	private void MoveToMouse() {
		Vector3 targetPosition = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		this.MoveTo(targetPosition);
	}
	private void LookToInput() {
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		Vector3 targetPosition = this.transform.position + input * 10;
		this.LookTo(targetPosition);
	}
	private void MoveToInput() {
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		Vector3 targetPosition = this.transform.position + input * 10;
		this.MoveTo(targetPosition);
	}
	private void LookTo(Vector3 targetPosition) {
		Vector3 diff = targetPosition - this.transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}
	private void MoveTo(Vector3 targetPosition) {
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
