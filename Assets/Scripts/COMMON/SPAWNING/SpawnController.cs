using UnityEngine;

public class SpawnController : ScriptableObject
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
			if (instance == null)
			{
				CreateInstance<SpawnController>();
			}
			
			return instance;
		}
	}
	
	public Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGameObject = Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGameObject.transform;
		
		return tempTrans;
	}
	
	public GameObject SpawnGameObject(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGameObject = Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGameObject.transform;
		
		return tempGameObject;
	}
}
