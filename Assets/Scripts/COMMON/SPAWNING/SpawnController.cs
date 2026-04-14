using UnityEngine;

[AddComponentMenu("Utility/Spawn Controller")]
public class SpawnController : ScriptableObject
{
	private Transform tempTrans;
	private GameObject tempGO;

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
				ScriptableObject.CreateInstance<SpawnController>();
			}
			
			return instance;
		}
	}
	
	public Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGO = (GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;
		
		return tempTrans;
	}
	
	public GameObject SpawnGameObject(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGO = (GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;
		
		return tempGO;
	}
}
