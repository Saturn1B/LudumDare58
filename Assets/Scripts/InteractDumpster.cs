using UnityEngine;

public class InteractDumpster : MonoBehaviour
{
	[SerializeField] private GameObject[] trashBags;
	public AudioClip trashBagSound;

    public void Interact()
	{
		trashBags[DayCounter.Instance.GetCurrentDay() - 1].SetActive(true);
		DayCounter.Instance.StartRestTask();
	}
}
