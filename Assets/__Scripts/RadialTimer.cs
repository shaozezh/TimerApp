using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialTimer : MonoBehaviour
{
    public Text timerText;
    public Image timerImage;
    public Text timerNameText;
    public float countdownSpeed = 1.0f; // Speed of the countdown

    private bool isCountdownFinished = true;
    public bool IsCountdownFinished => isCountdownFinished; // Property to check if the countdown is finished
    public bool isPaused;
    public bool isSkipped;
    public bool isRepeated;
    public GameObject pauseButton;
    public GameObject resumeButton;
    private Coroutine countdownCoroutine;
    void Start()
    {
        timerImage.fillAmount = 1;
        isPaused = false;
    }

    public void StartCountdown(string timerName, int duration)
    {
        timerNameText.text = timerName;
        timerText.text = ConvertTime(duration);
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownCoroutine(duration));
    }

    public string ConvertTime(int duration)
    {
        string hour = (duration / 3600).ToString();
        string min = (duration % 3600 / 60).ToString();
        string sec = (duration % 60).ToString();
        
        if (hour == "0" && min == "0")
            return sec;
        if (hour == "0")
            return min + " : " + sec;
        return hour + " : " + min + " : " + sec;
    }

    IEnumerator CountdownCoroutine(int duration)
    {
        isCountdownFinished = false;
        float currentTime = duration;
        while (currentTime > 0)
        {
            if (isSkipped || isRepeated)
            {
                timerImage.fillAmount = 1;
                break;
            }
            if (!isPaused)
            {
                currentTime -= Time.deltaTime * countdownSpeed;
                timerImage.fillAmount = currentTime / duration;
                timerText.text = ConvertTime((int)currentTime);
            }
            yield return null;
        }
        if (!isSkipped && !isRepeated)
        {
            timerImage.fillAmount = 0;
            isCountdownFinished = true;
        }
        
    }

    public void PauseCountdown()
    {
        isPaused = true;
        pauseButton.SetActive(false);
        resumeButton.transform.localScale = Vector3.one;
        resumeButton.SetActive(true);
    }

    public void ResumeCountdown()
    {
        isPaused = false;
        pauseButton.transform.localScale = Vector3.one;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void Reset()
    {
        isPaused = false;
        isSkipped = false;
        isRepeated = false;
        isCountdownFinished = true;
        pauseButton.transform.localScale = Vector3.one;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void Skip()
    {
        isSkipped = true;
    }

    public void Repeat()
    {
        isRepeated = true;
    }

}
