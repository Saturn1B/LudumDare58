using UnityEngine;

public class InteractDumpster : MonoBehaviour
{
	[SerializeField] private GameObject[] trashBags;
	public AudioClip trashBagSound;
	[SerializeField] private GameObject[] objectsToActivate;


	public void Interact()
	{
		trashBags[DayCounter.Instance.GetCurrentDay() - 1].SetActive(true);
		DayCounter.Instance.StartRestTask();

		switch (DayCounter.Instance.GetCurrentDay())
		{
			case 1:
				break;
			case 2:
				break;
			case 3:
				objectsToActivate[0].SetActive(true);
				break;
			case 4:
				objectsToActivate[1].SetActive(true);
				break;
			case 5:
				break;
			case 6:
				objectsToActivate[2].SetActive(true);
				break;
			case 7:
				AmbiancePlayer.Instance.PlayCreepyLoudSound();
				break;
			default:
				break;
		}
	}
}
