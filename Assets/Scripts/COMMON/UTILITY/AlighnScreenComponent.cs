using UnityEngine;
using Directions;

[ExecuteInEditMode]
public class AlighnScreenComponent : MonoBehaviour
{
	public Direction align = Direction.left;

	private Camera mainCamera;
	private Transform myTransform;
	
	[ExecuteInEditMode]
	private void Start()
	{
		if (!mainCamera)
		{
			mainCamera = Camera.main;
		}

		if (!myTransform)
		{
			myTransform = transform;
		}

		// update position
		UpdatePosition();
	}
	
	private void UpdatePosition()
	{
		Vector3 alignVector = Vector3.zero;

		if (align == Direction.left)
		{
			alignVector = new Vector3(mainCamera.ScreenToWorldPoint(Vector2.zero).x, 0, 0);
		}
		else if (align == Direction.right)
		{
			alignVector = new Vector3(mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x, 0, 0);
		}

		myTransform.position = alignVector;
	}
}
