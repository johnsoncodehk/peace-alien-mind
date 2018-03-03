using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class Satellite : MonoBehaviour, ISignalReceiverHandler {

	[System.Serializable]
	public struct Transfer {
		public Direction inDirection, outDirection;
		public Transform outTransform;
		public Transfer(Direction i, Direction o) {
			this.inDirection = i;
			this.outDirection = o;
			this.outTransform = null;
		}
	}

	[HideInInspector] public GridTransform gridTransform;
	public List<Transfer> transfers = new List<Transfer>();
	public Signal signal;
	public Transform receiver;
	public bool isRelay = true, hasSignal;

	private float angleSpeed;

	void Awake() {
		this.gridTransform = this.GetComponent<GridTransform>();
	}
	void Start() {
		this.gridTransform.enableControl = true;
	}

	public void OnSignalReceiver(Signal signal) {
		List<Direction> outDirs = this.GetOutDirections(signal.gridTransform.direction);
		var outTrans = this.GetOutTransform(signal.gridTransform.direction);
		if (outDirs.Count == 0) {
			Destroy(signal.gameObject);
			return;
		}
		signal.sendPositions.Add(this.gridTransform.position);
		if (this.isRelay) {
			// 中繼
			signal.target = this.receiver;
			if (signal.isLast) {
				StartCoroutine(this.DelayShoot(outDirs, outTrans, signal, signal.sendPositions));
			}
		}
		else {
			// 反射
			AudioManager.instance.PlaySignalBounce();
			for (int i = 0; i < outDirs.Count; i++) {
				var outDir = outDirs[i];
				var outTran = outTrans[i];
				signal.gridTransform.direction = outDir;
				signal.transform.position = outTran.position;
				signal.createBy = this.transform;
				signal.ResetStartTime();
			}
		}
	}
	public List<Direction> GetOutDirections(Direction globalInDir) {
		int inDirInt = (int)globalInDir - (int)this.gridTransform.direction;
		inDirInt = Mathf.RoundToInt(Mathf.Repeat(inDirInt, 8));

		return this.transfers
			.Where(t => (int)t.inDirection == inDirInt)
			.Select(t => (int)t.outDirection + (int)this.gridTransform.direction)
			.Select(dirInt => Mathf.RoundToInt(Mathf.Repeat(dirInt, 8)))
			.Select(dirInt => (Direction)dirInt)
			.ToList();
	}
	public List<Transform> GetOutTransform(Direction globalInDir) {
		int inDirInt = (int)globalInDir - (int)this.gridTransform.direction;
		inDirInt = Mathf.RoundToInt(Mathf.Repeat(inDirInt, 8));

		return this.transfers
			.Where(t => (int)t.inDirection == inDirInt)
			.Select(t => t.outTransform ? t.outTransform : this.transform)
			.ToList();
	}

	private IEnumerator DelayShoot(List<Direction> outDirs, List<Transform> outTrans, Signal signal, List<Vector2Int> line) {
		this.hasSignal = true;
		yield return new WaitForSeconds(0.2f);
		AudioManager.instance.PlaySignal();
		for (int i = 0; i < outDirs.Count; i++) {
			var outDir = outDirs[i];
			var outTran = outTrans[i];
			StartCoroutine(Signal.ShootAsync(this.signal, outTran.position, outDir, this.transform, signal.shootAt, line));
		}
		this.hasSignal = false;
	}
}
