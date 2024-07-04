using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    public PanelManager panelManager;
    public Transform content;
    public GameObject page3;
    public GameObject addButton;
    public GameObject editButton;
    private bool isAnimating = false;
    private Vector2 hiddenPosition = new Vector2(0, -2000);
    private Vector2 visiblePosition = new Vector2(0, 0);
    public RectTransform panelRectTransform;
    public float animationDuration = 1.0f;
    //public GameObject mask;
    public void Play()
    {
        if (content.childCount == 0)
        {
            //mask.SetActive(true);
            return;
        }
        if (!panelManager.CheckReady())
        {
            return;
        }
        //mask.SetActive(false);
        //page3.SetActive(true);
        addButton.SetActive(false);
        editButton.SetActive(false);
        ActivatePanel();
    }

    public void Stop()
    {
        DeactivatePanel();
        //page3.SetActive(false);
        addButton.SetActive(true);
        editButton.SetActive(true);
    }

    public void ActivatePanel()
    {
        if (!isAnimating)
        {
            page3.SetActive(true);
            StartCoroutine(AnimatePanel(visiblePosition));
        }
    }

    public void DeactivatePanel()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimatePanel(hiddenPosition));
        }
    }

    private IEnumerator AnimatePanel(Vector2 targetPosition)
    {
        isAnimating = true;
        Vector2 initialPosition = panelRectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            panelRectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panelRectTransform.anchoredPosition = targetPosition;
        isAnimating = false;

        if (targetPosition == hiddenPosition)
        {
            page3.SetActive(false);
        }
    }
}
