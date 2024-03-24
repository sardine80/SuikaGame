using UnityEngine;

public class UGUITweenRotation : UGUITween
{
	private RectTransform rectTransform = null;

	[SerializeField]
	private Vector3 from = Vector3.zero;

	[SerializeField]
	private Vector3 to = Vector3.zero;

	protected void Awake()
	{
		rectTransform = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
	}

	protected override void SetValue(float time) {
		rectTransform.localRotation = Quaternion.Euler(from + (to - from) * GetAnimationCurveValue(time));
	}
}
