using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject endCam;
    [SerializeField] private GameObject crossairPanel, HUDPanel, transitionPanel, endPanel;


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
        taskSlider.value = collectedTrash;
    }

    void StartThrowTask()
	{
        currentTask = Task.THROW;
        taskTitle.text = "Go throw the trash in the dumpster";
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
        if(currentDay == 7)
		{
            characterMovement.BlockMovement(true);
            endCam.SetActive(true);
            characterMovement.gameObject.SetActive(false);

            crossairPanel.SetActive(false);
            HUDPanel.SetActive(false);
            transitionPanel.SetActive(false);
            endPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
		else
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
            yield return new WaitForSeconds(1.5f);
            dayCounterText.gameObject.SetActive(false);
            characterMovement.BlockMovement(false);
            StartTrashTask();
            yield return transitionFade.DOFade(0, 1).WaitForCompletion();
        }
    }

    public void BackToMenu()
	{
        SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
}
