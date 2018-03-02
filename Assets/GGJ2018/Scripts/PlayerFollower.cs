using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {
	
	public float p = 0.1f;
	
	void Update() {
		// Vector3 pos = this.GetPlayersPos();
		Vector3 pos = GameManager.instance.player.transform.position;
		pos *= this.p;
		pos.z = this.transform.position.z;
		this.transform.position = pos;
	}

	// private Vector3 GetPlayersPos() {
	// 	if (Player.instances.Count == 0) {
	// 		return Vector3.zero;
	// 	}
	// 	Vector3 all = Vector3.zero;
	// 	foreach (Player player in Player.instances) {
	// 		all += player.transform.position;
	// 	}
	// 	return all / Player.instances.Count;
	// }
}
