using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    [SerializeField] private Image transitionFade;
    [SerializeField] private TMP_Text dayCounterText;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private GameObject[] dailyTrashes;


    public static DayCounter Instance { get; private set; }

    private float collectedTrash;
    private float totalTrash;

    public Task currentTask;

    private int currentDay = 0;
    public int GetCurrentDay() { return currentDay; }

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
        transitionFade.color = Color.black;
        StartCoroutine(EndDayFade(true));
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

    public void StartTrashTask()
	{
        dailyTrashes[currentDay - 1].SetActive(true);
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

    public IEnumerator EndDayFade(bool skipFadeIn = false)
	{
        currentTask = Task.PICKUP;

        characterMovement.BlockMovement(true);
        currentDay++;
		if (!skipFadeIn)
		{
            yield return transitionFade.DOFade(1, 1).WaitForCompletion();
        }
        dayCounterText.gameObject.SetActive(true);
        dayCounterText.text = $"DAY {currentDay}";
        yield return new WaitForSeconds(1);
        dayCounterText.gameObject.SetActive(false);
        characterMovement.BlockMovement(false);
        StartTrashTask();
        yield return transitionFade.DOFade(0, 1).WaitForCompletion();
    }
}
