using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPercentageBar : MonoBehaviour
{
    public List<Image> colorSegments;
    public StoreTimer gameManager;
    //private List<TimerData> timers;
    public Image startSemi;
    public Image endSemi;
    public int[] colorDurations;

    void Start()
    {
        //timers = gameManager.nameTimePairs;
        UpdateColorPercentages();
    }

    public void UpdateColorPercentages()
    {
        
        colorDurations = new int[9];
        foreach (var timer in gameManager.nameTimePairs)
        {
            colorDurations[timer.colorIndex] += timer.time;
        }

        int totalDuration = 0;
        foreach (var duration in colorDurations)
        {
            totalDuration += duration;
        }

        // Update the size of each color segment
        float totalWidth = GetComponent<RectTransform>().rect.width; // Get the width of the container
        float currentX = 0f;

        foreach (var segment in colorSegments)
        {
            segment.rectTransform.sizeDelta = new Vector2(0, segment.rectTransform.sizeDelta.y);
            segment.gameObject.SetActive(false); // Initially hide all segments
        }
        int firstColor = -1, lastColor = -1;
        for (int i = 0; i < colorSegments.Count; i++)
        {
            if (colorDurations[i] > 0)
            {
                if (firstColor == -1)
                {
                    firstColor = i;
                }
                lastColor = i;
                float percentage = (totalDuration > 0) ? (float)colorDurations[i] / totalDuration : 0;
                float segmentWidth = percentage * totalWidth;
                colorSegments[i].gameObject.SetActive(true);
                colorSegments[i].rectTransform.sizeDelta = new Vector2(segmentWidth, colorSegments[i].rectTransform.sizeDelta.y);
                colorSegments[i].rectTransform.anchoredPosition = new Vector2(currentX, 0);
                currentX += segmentWidth;
            }
        }
        if (firstColor == -1)
        {
            startSemi.color = Color.white;
            endSemi.color = Color.white;
        }
        else
        {
            startSemi.color = colorSegments[firstColor].color;
            endSemi.color = colorSegments[lastColor].color;
        }
        
    }
}
