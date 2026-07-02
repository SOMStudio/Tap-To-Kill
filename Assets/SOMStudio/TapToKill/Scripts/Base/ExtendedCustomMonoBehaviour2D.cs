using UnityEngine;

namespace SOMStudio.TapToKill.Scripts.Base
{
	public class ExtendedCustomMonoBehaviour2D : MonoBehaviour
	{
		[Header("Base")]
		protected Transform myTransform;
		protected GameObject myGameObject;
		protected Rigidbody2D myBody;

		protected bool didInit;
		protected bool canControl;

		protected int id;
	
		protected virtual void Init()
		{
			if (!myTransform)
			{
				myTransform = transform;
			}

			if (!myGameObject)
			{
				myGameObject = gameObject;
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
}
