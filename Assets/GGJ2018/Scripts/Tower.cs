using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public Signal signal;
	public Vector2 shootDelay;
	public Vector2 shootPosition;
	public Planet fromPlanet;

	private float nextShootTime;

	void Awake() {
		this.SetDelay();
	}
	void FixedUpdate() {
		if (Time.time >= this.nextShootTime) {
			this.SetDelay();
			StartCoroutine(Signal.ShootAsync(this.signal, this.shootPosition, Direction.Up, this.transform, GameManager.instance.step));
		}
	}

	private void SetDelay() {
		this.nextShootTime += Random.Range(this.shootDelay.x, this.shootDelay.y);
	}
}
