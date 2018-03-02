using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class StageStartPosition : MonoBehaviour {
	
	[HideInInspector] public GridTransform gridTransform;
	
	void Awake() {
		this.gridTransform = this.GetComponent<GridTransform>();
		this.GetComponent<SpriteRenderer>().enabled = false;
	}
	void Update () {
		GameManager.instance.player.character.targetPosition = this.transform.position + new Vector3(0, 0.3f, 0);
	}
}
