using UnityEngine;
using UnityEngine.UI;

public class UGUITweenColor : UGUITween {
	private Image image;

	[SerializeField]
	private Color from = Color.black;
	[SerializeField]
	private Color to = Color.black;

	protected void Awake()
	{ 
		image = GetComponent<Image>();
		if (image == null)
			Debug.LogWarning("This Component need Image Component!");
	}

	protected override void SetValue(float time) 
	{
		image.color = from + (to - from) * GetAnimationCurveValue(time);
	}
}
