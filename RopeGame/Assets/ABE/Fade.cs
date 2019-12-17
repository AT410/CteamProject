using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Fade : MonoBehaviour
{
	IFade fade;

	void Start ()
	{
		Init ();
		fade.Range = cutoutRange;
	}

    public bool IsEnd = false;

    [SerializeField]
    [Range(0,1)]
	float cutoutRange;

	void Init ()
	{
		fade = GetComponent<IFade> ();
	}

	void OnValidate ()
	{
		Init ();
		fade.Range = cutoutRange;
	}

	IEnumerator FadeoutCoroutine (float time, System.Action action)
	{
		float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

		var endFrame = new WaitForEndOfFrame ();

		while (Time.timeSinceLevelLoad <= endTime) {
			cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 0;
		fade.Range = cutoutRange;

		if (action != null) {
			action ();
		}

        IsEnd = true;
        yield break;
	}

	IEnumerator FadeinCoroutine (float time, System.Action action)
	{
		float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);
		
		var endFrame = new WaitForEndOfFrame ();

		while (Time.timeSinceLevelLoad <= endTime) {
			cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 1;
		fade.Range = cutoutRange;

		if (action != null) {
			action ();
		}

        IsEnd = true;
        yield break;

    }

    public Coroutine FadeOut (float time, System.Action action)
	{
		StopAllCoroutines ();
		return StartCoroutine (FadeoutCoroutine (time, action));
	}

	public Coroutine FadeOut (float time)
	{
		return FadeOut (time, null);
	}

	public Coroutine FadeIn (float time, System.Action action)
	{
		StopAllCoroutines ();
		return StartCoroutine (FadeinCoroutine (time, action));
	}

	public Coroutine FadeIn (float time)
	{
		return FadeIn (time, null);
	}
}