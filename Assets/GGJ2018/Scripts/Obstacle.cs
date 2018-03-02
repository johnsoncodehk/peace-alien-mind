using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class Obstacle : MonoBehaviour, ISignalReceiverHandler {

	[HideInInspector] public GridTransform gridTransform;
	public List<Sprite> sprites = new List<Sprite>();

	void Awake() {
		this.gridTransform = this.GetComponent<GridTransform>();
		this.gridTransform.direction = (Direction)Random.Range(0, 8);
		this.GetComponent<SpriteRenderer>().sprite = this.sprites[Random.Range(0, this.sprites.Count)];
	}

	public void OnSignalReceiver(Signal signal) {
		Destroy(signal.gameObject);
	}
}
