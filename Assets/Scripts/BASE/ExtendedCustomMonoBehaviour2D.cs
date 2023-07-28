using UnityEngine;

public class ExtendedCustomMonoBehaviour2D : MonoBehaviour
{
	[Header("Base")]
	protected Transform myTransform;
	protected GameObject myGO;
	protected Rigidbody2D myBody;

	protected bool didInit;
	protected bool canControl;

	protected int id;
	
	protected virtual void Init()
	{
		// cache refs to our transform and gameObject
		if (!myTransform)
		{
			myTransform = transform;
		}

		if (!myGO)
		{
			myGO = gameObject;
		}

		if (!myBody)
		{
			myBody = GetComponent<Rigidbody2D>();
		}

		didInit = true;
	}

	protected virtual void SetID(int anID)
	{
		id = anID;
	}
}
