using UnityEngine;

[AddComponentMenu("SOM Studio/Tap-To-Kill/Utility/Animation Open Close")]
public class AnimationOpenClose : MonoBehaviour
{
	public bool hasHideAnimation;

	private readonly int openAnim = Animator.StringToHash("open");
	private readonly int hideAnim = Animator.StringToHash("hide");

	private Animator animator;
	
	private void Awake()
	{
		animator = GetComponent<Animator>();

		if (animator.parameters.Length > 1)
		{
			hasHideAnimation = true;
		}
	}
	
	public void Click()
	{
		if (IsOpen())
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	public void Open()
	{
		if (!IsOpen())
		{
			animator.SetBool(openAnim, true);
		}
	}

	public void Close()
	{
		if (IsOpen())
		{
			animator.SetBool(openAnim, false);
		}
	}

	public bool IsOpen()
	{
		return animator.GetBool(openAnim);
	}

	public void Hide()
	{
		if (hasHideAnimation)
		{
			animator.SetBool(hideAnim, true);
		}
	}

	public void Show()
	{
		if (hasHideAnimation)
		{
			animator.SetBool(hideAnim, false);
		}
	}

	public bool IsShow()
	{
		if (hasHideAnimation)
		{
			return animator.GetBool(hideAnim);
		}
		else
		{
			return true;
		}
	}
}
