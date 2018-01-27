using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public Signal signal;
	public Vector2 shootDelay;
	public Vector2 shootPosition;

	private float nextShootTime;

	void Awake () {
		this.SetDelay();
	}
	
	void FixedUpdate () {
		if (Time.time >= this.nextShootTime) {
			this.SetDelay();
			StartCoroutine(this.Shoot());
		}
	}

	private void SetDelay() {
		this.nextShootTime = Random.Range(this.shootDelay.x, this.shootDelay.y);
	}
	private IEnumerator Shoot() {
		Vector3 shootPos = this.transform.TransformPoint(this.shootPosition);
		Quaternion shootRotation = this.transform.rotation;

		var firstSignal = Instantiate(this.signal, shootPos, shootRotation);
		firstSignal.isFirst = true;
		yield return new WaitForSeconds(0.2f);
		var signal2 = Instantiate(this.signal, shootPos, shootRotation);
		yield return new WaitForSeconds(0.2f);
		var signal3 = Instantiate(this.signal, shootPos, shootRotation);

		firstSignal.child = signal2;
		signal2.child = signal3;
	}
}
