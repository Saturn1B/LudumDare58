using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
	public AudioClip trashSound;
	public bool bleed;
	[SerializeField] private GameObject[] objectsToActivate;

	public void PickUp(MeshRenderer renderer, MeshFilter mesh)
	{
		foreach (var obj in objectsToActivate)
		{
			obj.SetActive(true);
		}
		if(objectsToActivate.Length > 0)
		{
			AmbiancePlayer.Instance.PlayCreepySound();
		}

		renderer.material = GetComponent<MeshRenderer>().material;
		mesh.mesh = GetComponent<MeshFilter>().mesh;
		DayCounter.Instance.PickUpTrash();
		Destroy(gameObject);
	}
}
