using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShoorer : MonoBehaviour {

	public Vector2 minSpeed, maxSpeed;
	public Vector2 randomTime;
	public float nextShootTime;
	public List<Meteor> meteors = new List<Meteor>();
	
	void Awake() {
		this.nextShootTime += Random.Range(this.randomTime.x, this.randomTime.y);
	}
	void Update () {
		if (Time.time > this.nextShootTime) {
			this.nextShootTime += Random.Range(this.randomTime.x, this.randomTime.y);
			StartCoroutine(this.Shoot());
		}
	}

	private IEnumerator Shoot() {
		float v = Random.value;
		if (v > 0.9f) {
			this.ShootOne();
			yield return new WaitForSeconds(0.2f);
			this.ShootOne();
			yield return new WaitForSeconds(0.2f);
			this.ShootOne();
		}
		else if (v > 0.75f) {
			this.ShootOne();
			yield return new WaitForSeconds(0.2f);
			this.ShootOne();
		}
		else {
			this.ShootOne();
		}
	}
	private void ShootOne() {
		Meteor meteor = this.meteors[Random.Range(0, this.meteors.Count)];
		Vector2 speed = new Vector2(Random.Range(this.minSpeed.x, this.maxSpeed.x), Random.Range(this.minSpeed.y, this.maxSpeed.y));
		Meteor newMeteor = Instantiate(meteor);
		newMeteor.transform.position = this.transform.position;
		newMeteor.speed = speed;
	}
}
