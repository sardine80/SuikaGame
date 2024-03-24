using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class UGUITween : MonoBehaviour {

	private enum PLAY_TYPE { Once, Loop, PingPong, }

	[SerializeField]
	private PLAY_TYPE playType = PLAY_TYPE.Once;

	[SerializeField]
	private AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	[SerializeField]
	public float duration = 1f;

	[SerializeField]
	private float delay = 0f;

	public UnityEvent onEndTweenCallback = null;

    [Tooltip("When unchecked this variable use UnscaledDeltaTime")]
	public bool useTimeScale = false;
	
	private void OnValidate() {
		duration = Mathf.Max(duration, 0);
		delay = Mathf.Max(delay, 0);
	}

	private Coroutine coroutine = null;
	
    private void OnEnable() 
    {
	    coroutine = StartCoroutine(Play(true));
	}

	private IEnumerator Play(bool isForward) {
		float x = 0f;
		SetValue(isForward ? x : 1f - x);
		if (useTimeScale)
			yield return new WaitForSeconds(delay);
		else
			yield return new WaitForSecondsRealtime(delay);

		bool isLoop = true;
		while (isLoop) {
			x = 0f;
			while (x < 1f) {
				if (useTimeScale)
					x += Time.deltaTime / duration;
				else
					x += Time.unscaledDeltaTime / duration;
				SetValue(isForward ? x : 1f - x);
				yield return null;
			}

			switch (playType) {
				case PLAY_TYPE.Once:
					isLoop = false;
					break;

				case PLAY_TYPE.Loop:
					break;

				case PLAY_TYPE.PingPong:
					isForward = !isForward;
					break;
			}
		}

		coroutine = null;

		onEndTweenCallback?.Invoke();
	}

	protected abstract void SetValue(float time);

	protected float GetAnimationCurveValue(float time) {
		return Mathf.Clamp(animationCurve.Evaluate(time), 0, 1);
	}

	public void PlayReverse() {
		if (coroutine != null) {
			StopCoroutine(coroutine);
		}

		coroutine = StartCoroutine(Play(false));
	}

	public void StopPlay()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}
}
