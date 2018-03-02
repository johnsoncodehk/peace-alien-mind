using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAxis : MonoBehaviour {

	public float speed;

	void Awake() {
		this.transform.Rotate(new Vector3(0, Random.Range(0f, 360f), 0));
	}
	void Update() {
		this.transform.Rotate(new Vector3(0, this.speed * Time.deltaTime, 0));
	}
}
