using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridTransform), true)]
public class GridTransformInspector : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		GridTransform t = this.target as GridTransform;
		if (GUILayout.Button("Up")) {
			t.position += Vector2Int.up;
			EditorUtility.SetDirty(t);
			t.OnValidate();
		}
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Left")) {
			t.position += Vector2Int.left;
			EditorUtility.SetDirty(t);
			t.OnValidate();
		}
		if (GUILayout.Button("Right")) {
			t.position += Vector2Int.right;
			EditorUtility.SetDirty(t);
			t.OnValidate();
		}
		EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Down")) {
			t.position += Vector2Int.down;
			EditorUtility.SetDirty(t);
			t.OnValidate();
		}
	}
}
#endif

public enum Direction {
	Up = 0,
	UpLeft = 1,
	Left = 2,
	DownLeft = 3,
	Down = 4,
	DownRight = 5,
	Right = 6,
	UpRight = 7,
}
public class GridTransform : MonoBehaviour
	, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public Vector2Int position;
	public Direction direction;
	public float angle { get { return (int)this.direction * 45; } }
	public bool ingnorePosition, ingnoreRotation;
	public float smoothTime;
	public bool enableControl {
		get { return this.clickCollider.enabled; }
		set { this.clickCollider.enabled = value; }
	}

	private float angleSpeed;
	private bool onDrag, isClick;
	private Vector3 dPos, moveSpeed;
	private CircleCollider2D clickCollider;

	public void OnValidate() {
		if (!Application.isPlaying) {
			this.TryUpdatePosition(0);
			this.TryUpdateRotation(0);
		}
	}
	void Awake() {
		this.clickCollider = new GameObject("HitBox").AddComponent<CircleCollider2D>();
		this.clickCollider.transform.SetParent(this.transform, false);
		this.clickCollider.radius = 0.5f;
		this.clickCollider.isTrigger = true;
		this.clickCollider.gameObject.layer = LayerMask.NameToLayer("Tile");
		this.clickCollider.enabled = false;
	}
	void Start() {
		this.TryUpdatePosition(0);
		this.TryUpdateRotation(0);
	}
	void Update() {
		this.TryUpdatePosition(this.smoothTime);
		this.TryUpdateRotation(this.smoothTime);
		if (this.onDrag) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = this.transform.position.z - GameManager.instance.mainCamera.transform.position.z;
			Vector3 targetPosition = GameManager.instance.mainCamera.ScreenToWorldPoint(mousePosition);
			this.dPos = Vector3.SmoothDamp(this.dPos, Vector3.zero, ref this.moveSpeed, 0.25f);
			this.transform.position = targetPosition - this.dPos;
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		this.isClick = true;
	}
	public void OnPointerUp(PointerEventData eventData) { }
	public void OnPointerClick(PointerEventData eventData) {
		if (this.isClick) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				this.direction++;
			}
			else if (eventData.button == PointerEventData.InputButton.Right) {
				this.direction--;
			}
			this.direction = (Direction)Mathf.Repeat((int)this.direction, 8);
			GameManager.instance.step++;
		}
	}
	public void OnBeginDrag(PointerEventData eventData) {
		Vector3 targetPosition = GameManager.instance.ScreenToWorldPoint(this.transform.position.z);
		this.dPos = targetPosition - this.transform.position;
		this.onDrag = true;
		this.isClick = false;
		GameManager.instance.step++;
	}
	public void OnDrag(PointerEventData eventData) { }
	public void OnEndDrag(PointerEventData eventData) {
		this.onDrag = false;
		Vector2Int pos = Vector2Int.FloorToInt(this.transform.position);
		bool canDrag = this.GetComponentInParent<Stage>().InBorder(pos);
		if (!canDrag) return;
		foreach (var s in this.transform.parent.GetComponentsInChildren<GridTransform>()) {
			if (s == this)
				continue;
			if (s.position == pos)
				canDrag = false;
		}
		if (!canDrag) return;
		this.position = pos;
		GameManager.instance.step++;
	}

	private void TryUpdatePosition(float smoothTime) {
		if (this.ingnorePosition || this.onDrag) return;

		Vector2 targetPosition = this.position + new Vector2(0.5f, 0.5f);
		this.transform.localPosition = Vector3.SmoothDamp(this.transform.localPosition, targetPosition, ref this.moveSpeed, smoothTime);
	}
	private void TryUpdateRotation(float smoothTime) {
		if (this.ingnoreRotation) return;

		Vector3 angle = this.transform.localEulerAngles;
		angle.z = Mathf.SmoothDampAngle(angle.z, this.angle, ref this.angleSpeed, smoothTime);
		this.transform.localEulerAngles = angle;
	}
}
