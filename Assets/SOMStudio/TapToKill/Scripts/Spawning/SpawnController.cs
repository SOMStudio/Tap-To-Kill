using UnityEngine;

namespace SOMStudio.TapToKill.Scripts.Spawning
{
	public class SpawnController
	{
		private Transform tempTrans;
		private GameObject tempGameObject;

		private static SpawnController instance;
	
		public SpawnController()
		{
			if (instance != null)
			{
				Debug.LogWarning("Tried to generate more than one instance of singleton SpawnController.");
				return;
			}
		
			instance = this;
		}

		public static SpawnController Instance
		{
			get
			{
				instance ??= new SpawnController();

				return instance;
			}
		}
	
		public Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
		{
			return SpawnGameObject(anObject, aPosition, aRotation).transform;
		}
	
		public GameObject SpawnGameObject(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
		{
			tempGameObject = Object.Instantiate(anObject, aPosition, aRotation);
			tempTrans = tempGameObject.transform;
		
			return tempGameObject;
		}
	}
}
