using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
	public AudioClip trashSound;
	public bool bleed;

	public void PickUp(MeshRenderer renderer, MeshFilter mesh)
	{
		renderer.material = GetComponent<MeshRenderer>().material;
		mesh.mesh = GetComponent<MeshFilter>().mesh;
		DayCounter.Instance.PickUpTrash();
		Destroy(gameObject);
	}
}
