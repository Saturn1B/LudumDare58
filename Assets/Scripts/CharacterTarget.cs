using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterTarget : MonoBehaviour
{
	private Transform playerCamera;
	private CharacterMovement characterMovement;
	[SerializeField] private int playerReach;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private Transform hand;
	[SerializeField] private Transform stick;
	[SerializeField] private GameObject trashOnStick;
	[SerializeField] private TMP_Text actionText;
	Quaternion baseOrientation;

	bool isPicking;

	private void Start()
	{
		playerCamera = GetComponentInChildren<Camera>().transform;
		characterMovement = GetComponent<CharacterMovement>();
		baseOrientation = hand.localRotation;
	}

	private void Update()
	{
		RaycastHit hit;

		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerReach, layerMask))
		{
			Debug.DrawRay(playerCamera.position, playerCamera.forward * playerReach, Color.green, 1);
			Debug.Log(hit.collider.transform.name);

			if (hit.collider.transform.GetComponent<InteractionObject>() && DayCounter.Instance.currentTask == Task.PICKUP)
			{
				if (!actionText.gameObject.activeSelf)
				{
					actionText.text = "Left Clic to pick trash";
					actionText.gameObject.SetActive(true);
				}

				InteractionObject interact = hit.collider.transform.GetComponent<InteractionObject>();
				hand.LookAt(interact.GetComponent<Transform>().position - Vector3.up + playerCamera.transform.forward * 4, hand.transform.up);
				if (Input.GetKeyDown(KeyCode.Mouse0) && !isPicking)
				{
					StartCoroutine(PickUpAnim(interact));
				}
			}

			if (hit.collider.transform.GetComponent<InteractDumpster>() && DayCounter.Instance.currentTask == Task.THROW)
			{
				if (!actionText.gameObject.activeSelf)
				{
					actionText.text = "E to throw trash";
					actionText.gameObject.SetActive(true);
				}

				InteractDumpster interact = hit.collider.transform.GetComponent<InteractDumpster>();
				if (Input.GetKeyDown(KeyCode.E))
				{
					interact.Interact();
					actionText.text = "";
					actionText.gameObject.SetActive(false);
				}
			}

			if (hit.collider.transform.GetComponent<InteractShed>() && DayCounter.Instance.currentTask == Task.REST)
			{
				if (!actionText.gameObject.activeSelf)
				{
					actionText.text = "E to go rest";
					actionText.gameObject.SetActive(true);
				}

				InteractShed interact = hit.collider.transform.GetComponent<InteractShed>();
				if (Input.GetKeyDown(KeyCode.E))
				{
					interact.Interact();
					actionText.text = "";
					actionText.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			Debug.DrawRay(playerCamera.position, playerCamera.forward * playerReach, Color.red, 1);
			if (hand.localRotation != baseOrientation)
				hand.localRotation = baseOrientation;

			if (actionText.gameObject.activeSelf)
			{
				actionText.text = "";
				actionText.gameObject.SetActive(false);
			}
		}
	}

	IEnumerator PickUpAnim(InteractionObject interact)
	{
		isPicking = true;
		characterMovement.BlockMovement(true);
		yield return stick.transform.DOLocalMoveZ(stick.transform.localPosition.z + 1, .5f).WaitForCompletion();
		interact.PickUp(trashOnStick.GetComponent<MeshRenderer>(), trashOnStick.GetComponent<MeshFilter>());
		yield return stick.transform.DOLocalMoveZ(stick.transform.localPosition.z - 1, .5f).WaitForCompletion();
		trashOnStick.GetComponent<MeshRenderer>().sharedMaterial = null;
		trashOnStick.GetComponent<MeshFilter>().mesh = null;
		isPicking = false;
		characterMovement.BlockMovement(false);
	}
}
