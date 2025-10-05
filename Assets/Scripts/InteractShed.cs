using UnityEngine;

public class InteractShed : MonoBehaviour
{
	public AudioClip audioClip;

	public void Interact()
	{
		StartCoroutine(DayCounter.Instance.EndDayFade());
	}
}
