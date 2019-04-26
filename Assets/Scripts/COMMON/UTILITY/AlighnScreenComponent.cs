using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions;

[ExecuteInEditMode]
public class AlighnScreenComponent : MonoBehaviour {

	public Direction align = Direction.left;

	private Camera mainCamera;
	private Transform myTransform;

	// main event
	[ExecuteInEditMode]
	void Start () {
		if (!mainCamera) {
			mainCamera = Camera.main;
		}
		if (!myTransform) {
			myTransform = transform;
		}

		// update position
		UpdatePosition ();
	}

	// main logic
	void UpdatePosition() {
		Vector3 alignVector = Vector3.zero;

		if (align == Direction.left) {
			alignVector = new Vector3 (mainCamera.ScreenToWorldPoint (Vector2.zero).x, 0, 0);
		} else if (align == Direction.right) {
			alignVector = new Vector3 (mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x, 0, 0);
		}

		myTransform.position = alignVector;
	}
}
