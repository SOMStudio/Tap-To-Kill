using UnityEngine;

[AddComponentMenu("Utility/Spawn Controller")]

public class SpawnController : ScriptableObject 
{
	private Transform tempTrans;
	private GameObject tempGO;

	private static SpawnController instance;
 
	// singletone
	public SpawnController () 
	{
		if (instance != null)
		{
			// drop out if instance exists, to avoid generating duplicates
			Debug.LogWarning("Tried to generate more than one instance of singleton SpawnController.");
			return;
		}
 
		// as no instance already exists, we can safely set instance to this one
		instance = this;
	}
 
	public static SpawnController Instance
	{
		get
		{
			// chack instance already exists
			if (instance == null)
			{
				// no instance exists yet, so we go ahead and create one
				 ScriptableObject.CreateInstance<SpawnController>(); // new SpawnController ();
			}
 			// now we pass the reference to this instance back
			return instance;
		}
	}

	// main Logic
	public Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		// instantiate the object
		tempGO=(GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans= tempGO.transform;

		// return the object to whatever was calling
		return tempTrans;
	}
	
	// here we just provide a convenient function to return the spawned objects gameobject rather than its transform
	public GameObject SpawnGO(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		// instantiate the object
		tempGO=(GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans= tempGO.transform;
		
		// return the object to whatever was calling
		return tempGO;
	}
}
