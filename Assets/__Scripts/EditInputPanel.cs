using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EditInputPanel : MonoBehaviour
{
    
    public InputPanel inputPanel;
    public Text inputName;
    public RectTransform _element;
    public Transform hourContent;
    public Transform minContent;
    public Transform secContent;
    public Text placeHolder;
    public AddSequence page2;
    public StoreTimer gameManager;
    public GameObject interval;
    public int intervalIndex;
    public void ShowPanel(int index, GameObject obj)
    {   
        inputPanel.Show();
        interval = obj;
        intervalIndex = index;
        TimerData datum = gameManager.nameTimePairs[index];
        ShowTime(datum.time);
        ShowName(datum.name);
        ShowColor(datum.colorIndex);
        page2.colorIndex = datum.colorIndex;
    }

    private void ShowTime(int time)
    {
        int hour = time / 3600;
        int min = time % 3600 / 60;
        int sec = time % 60;
        
        Vector2 newPosition = new Vector2 (hourContent.GetComponent<RectTransform>().anchoredPosition.x, -hourContent.GetChild(hour+2).GetComponent<RectTransform>().anchoredPosition.y);
		hourContent.GetComponent<RectTransform>().anchoredPosition = newPosition;
        newPosition = new Vector2 (minContent.GetComponent<RectTransform>().anchoredPosition.x, -minContent.GetChild(min+2).GetComponent<RectTransform>().anchoredPosition.y);
		minContent.GetComponent<RectTransform>().anchoredPosition = newPosition;
        newPosition = new Vector2 (secContent.GetComponent<RectTransform>().anchoredPosition.x, -secContent.GetChild(sec+2).GetComponent<RectTransform>().anchoredPosition.y);
		secContent.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }

    private void ShowName(string name)
    {
        placeHolder.text = name;
        inputName.text = name;
    }

    private void ShowColor(int color)
    {
        if (color != 0)
        {
            Debug.Log(color);
            page2.colorButtonsEdit[color-1].GetComponent<Outline>().enabled = true;
        }
        
    }
    
}
