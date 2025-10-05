using UnityEngine;

public class InteractDumpster : MonoBehaviour
{
    public void Interact()
	{
		DayCounter.Instance.StartRestTask();
	}
}
