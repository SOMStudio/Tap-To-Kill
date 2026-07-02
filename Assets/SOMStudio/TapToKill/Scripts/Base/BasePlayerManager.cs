using UnityEngine;

namespace SOMStudio.TapToKill.Scripts.Base
{
	public class BasePlayerManager : MonoBehaviour
	{
		[SerializeField] protected bool didInit;
	
		[SerializeField] protected BaseUserManager dataManager;
	
		private void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (!dataManager)
			{
				dataManager = gameObject.GetComponent<BaseUserManager>();

				if (!dataManager)
					dataManager = gameObject.AddComponent<BaseUserManager>();
			}
		
			dataManager.GetDefaultData();

			didInit = true;
		}

		public virtual void GameFinished()
		{
			dataManager.SetIsFinished(true);
		}

		public virtual void GameStart()
		{
			dataManager.SetIsFinished(false);
		}
	}
}
