using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Task
{
    PICKUP,
    THROW,
    REST
}

public class DayCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text taskTitle;
    [SerializeField] private Slider taskSlider;

    public static DayCounter Instance { get; private set; }

    private float collectedTrash;
    private float totalTrash;

    public Task currentTask;

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

    void Start()
    {
        StartTrashTask();
    }

    public void PickUpTrash()
	{
        collectedTrash += 1;
        taskSlider.value = collectedTrash / totalTrash;

        Debug.Log($"pick up {collectedTrash} out of {totalTrash}");

        if (collectedTrash == totalTrash)
		{
            StartThrowTask();
		}
	}

    void StartTrashTask()
	{
        currentTask = Task.PICKUP;
        taskTitle.text = "Pick up trash in the forest";
        taskSlider.gameObject.SetActive(true);
        totalTrash = FindObjectsByType<InteractionObject>(FindObjectsSortMode.None).Length;
        collectedTrash = 0;
    }

    void StartThrowTask()
	{
        currentTask = Task.THROW;
        taskTitle.text = "Go throw the trash in the bin";
        taskSlider.gameObject.SetActive(false);
        totalTrash = 0;
        collectedTrash = 0;
    }

    public void StartRestTask()
	{
        currentTask = Task.REST;
        taskTitle.text = "Go rest in your cabin";
    }
}
