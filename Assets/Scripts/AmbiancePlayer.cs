using UnityEngine;

public class AmbiancePlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip creepyAudio;
    [SerializeField] private AudioClip creepyLoudAudio;

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
        audioSource.pitch = Random.Range(.6f, 1.4f);
        audioSource.PlayOneShot(creepyAudio);
    }
    public void PlayCreepyLoudSound()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(creepyLoudAudio);
    }
}
