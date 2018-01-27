using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

	// public float startTime = 10;
	public float lifeTime = 5;
	public float gravity = 1;
	public Vector2 speed;
	
	void Update() {
		// if (this.startTime > 0) {
		// 	this.startTime -= Time.deltaTime;
		// 	return;
		// }
		this.speed.y -= this.gravity * Time.deltaTime;
		this.transform.position += (Vector3)this.speed;
		this.lifeTime -= Time.deltaTime;
		if (this.lifeTime <= 0) {
			Destroy(this.gameObject);
		}
	}
}
