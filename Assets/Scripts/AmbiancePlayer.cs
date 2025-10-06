using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip creepyAudio;

    public static AmbiancePlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
	}

	public void PlayCreepySound()
	{
        audioSource.PlayOneShot(creepyAudio);
    }
}
