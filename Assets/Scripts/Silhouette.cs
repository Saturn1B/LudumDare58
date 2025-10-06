using UnityEngine;

public class Silhouette : MonoBehaviour
{
    public Transform player;            // Player's transform
    public float viewAngle = 30f;       // Max angle to consider "looking at" the object
    public float rotationSpeed = 5f;    // How fast the object rotates to face the player

    private bool isBeingLookedAt = false;

	private void Start()
	{
        player = FindObjectOfType<CharacterController>().transform;
	}

    void Update()
    {
        if (player == null) return;

        // --- ROTATE TOWARDS PLAYER (Y-axis only) ---
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Keep only horizontal rotation
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- CHECK IF PLAYER IS LOOKING AT OBJECT ---
        Vector3 toObject = (transform.position - player.position).normalized;
        Vector3 playerForward = player.forward;
        float angle = Vector3.Angle(playerForward, toObject);

        if (angle < viewAngle)
        {
            isBeingLookedAt = true; // Player is looking
        }
        else if (isBeingLookedAt)
        {
            // Player looked away after looking at it -> destroy the object
            Destroy(gameObject);
        }
    }
}
