using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {
	
	public Player player;
	public float p = 0.1f;
	
	void Update() {
		Vector3 pos = this.player.transform.position;
		pos *= 0.1f;
		pos.z = this.transform.position.z;
		this.transform.position = pos;
	}
}
