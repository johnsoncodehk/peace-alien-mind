using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransformHook : MonoBehaviour {

	public Transform target;

	void LateUpdate() {
		if (this.target) {
			this.transform.position = target.position;
		}
	}
}
