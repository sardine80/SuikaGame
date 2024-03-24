using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UGUITweenAlpha : UGUITween {
	private CanvasGroup canvasGroup = null;

	[Range(0, 1)]
	[SerializeField]
	private float from = 0f;

	[Range(0, 1)]
	[SerializeField]
	private float to = 1f;

	protected void  Awake() {
		canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
	}

	protected override void SetValue(float time) {
		canvasGroup.alpha = from + (to - from) * GetAnimationCurveValue(time);
	}
}
