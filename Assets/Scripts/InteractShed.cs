using UnityEngine;

public class InteractShed : MonoBehaviour
{
	public void Interact()
	{
		StartCoroutine(DayCounter.Instance.EndDayFade());
	}
}
