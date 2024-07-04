using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddSequence : MonoBehaviour
{
    public Transform panels;
    public int num_limit;

    public GameObject panelPrefab;
    public int currentIndex;

    public InputPanel inputPanel;
    public Text placeholderText;
    public InputField inputText;
    public InputField inputTextEdit;
    private Text timerName;
    private GameObject panel;
    public String title;
    private bool isNextReady = true;
    private Text timerTime;
    public StoreTimer gameManager;
    [SerializeField] ScrollSnap hour;
    [SerializeField] ScrollSnap min;
    [SerializeField] ScrollSnap sec;
    [SerializeField] ScrollSnap hourEdit;
    [SerializeField] ScrollSnap minEdit;
    [SerializeField] ScrollSnap secEdit;
    public int index;
    public Button editButton;
    public int colorIndex;
    public Button[] colorButtons;
    public Button[] colorButtonsEdit;
    public ColorPercentageBar bar;
    public EditInputPanel panelManager;
    public InputPanel editPanel;
    void Start()
    {
        if (index == 2)
        {
            // Assign onClick listeners to all buttons
            for (int i = 0; i < colorButtons.Length; i++)
            {
                int buttonIndex = i;
                colorButtons[i].onClick.AddListener(() => OnColorButtonClick(buttonIndex));
            }
            for (int i = 0; i < colorButtonsEdit.Length; i++)
            {
                int buttonIndex = i;
                colorButtonsEdit[i].onClick.AddListener(() => OnColorButtonClickEdit(buttonIndex));
            }
        }
    }

    public void ActivateSequence()
    {
        if (!isNextReady)
        {
            return;
        }
        panel = Instantiate(panelPrefab, panels);

        panel.GetComponent<SwipeToDelete>().SetIndex(currentIndex);
        currentIndex++;
        timerName = panel.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        timerName.text = "New " + title + " " + gameManager.GetSmallestAvailableNumber(index);
        placeholderText.text = timerName.text;
        //inputText.text = "";
        inputPanel.Show();
        isNextReady = false;
    }

    public void UpdatePage()
    {
        if (index == 1)
        {
            UpdateText();
            gameManager.AddToList(timerName.text);
        }
        else if (hour.inputTime + min.inputTime + sec.inputTime != 0)
        {
            UpdateText();
            UpdateTime();
            gameManager.AddToList(timerName.text, hour.inputTime, min.inputTime, sec.inputTime, colorIndex);
            bar.UpdateColorPercentages();
        }
        else
        {
            // error sfx
        }
    }

    public void EditPage()
    {
        if (hourEdit.inputTime + minEdit.inputTime + secEdit.inputTime != 0)
        {
            string name = EditText();
            //Debug.Log(name);
            EditTime();
            int index = panelManager.intervalIndex;
            int time = 60 * 60 * hourEdit.inputTime + 60 * minEdit.inputTime + secEdit.inputTime;
            gameManager.nameTimePairs[index] = new TimerData(name, time, colorIndex);
            bar.UpdateColorPercentages();
        }
        else
        {
            // error sfx
        }
        
    }

    private string EditText()
    {
        Text name = panels.transform.GetChild(panelManager.intervalIndex).GetChild(1).GetChild(0).GetComponent<Text>();
        if (inputTextEdit.text != "")
        {
            name.text = inputTextEdit.text;
        }
        else
        {
            inputTextEdit.text = name.text;
        }
        inputTextEdit.text = "";
        editPanel.Hide();
        return name.text;
    }

    private void EditTime()
    {
        timerTime = panels.transform.GetChild(panelManager.intervalIndex).GetChild(1).GetChild(1).GetComponent<Text>();
        if (hourEdit.inputTime == 0 && minEdit.inputTime == 0)
            timerTime.text = secEdit.inputTime + "s ";
        else if (hourEdit.inputTime == 0)
            timerTime.text = minEdit.inputTime + "m " + secEdit.inputTime + "s ";
        else
            timerTime.text = hourEdit.inputTime + "h " + minEdit.inputTime + "m " + secEdit.inputTime + "s ";
    }

    private void UpdateText()
    {
        if (inputText.text != "")
        {
            timerName.text = inputText.text;
        }
        inputText.text = "";
        inputPanel.Hide();
        isNextReady = true;
    }

    private void UpdateTime()
    {
        timerTime = panel.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        if (hour.inputTime == 0 && min.inputTime == 0)
            timerTime.text = sec.inputTime + "s ";
        else if (hour.inputTime == 0)
            timerTime.text = min.inputTime + "m " + sec.inputTime + "s ";
        else
            timerTime.text = hour.inputTime + "h " + min.inputTime + "m " + sec.inputTime + "s ";
    }

    private void OnColorButtonClick(int buttonIndex)
    {
        foreach (var button in colorButtons)
        {
            button.gameObject.GetComponent<Outline>().enabled = false;
        }
        //Debug.Log(buttonIndex);
        colorButtons[buttonIndex].gameObject.GetComponent<Outline>().enabled = true;
        colorIndex = buttonIndex+1;
        panel.GetComponent<DragHandler>().SetColor(colorIndex);
        //colorIndex = buttonIndex;
    }

    private void OnColorButtonClickEdit(int buttonIndex)
    {
        foreach (var button in colorButtonsEdit)
        {
            button.gameObject.GetComponent<Outline>().enabled = false;
        }
        //Debug.Log(buttonIndex);
        colorButtonsEdit[buttonIndex].gameObject.GetComponent<Outline>().enabled = true;
        colorIndex = buttonIndex+1;
        panels.GetChild(panelManager.intervalIndex).GetComponent<DragHandler>().SetColor(colorIndex);
        //colorIndex = buttonIndex;
    }

    public void ClearColor()
    {
        if (colorIndex != 0)
        {
            foreach (var button in colorButtons)
            {
                button.gameObject.GetComponent<Outline>().enabled = false;
            }
            colorIndex = 0;
        }
        
    }

    public void ClearColorEdit()
    {
        if (colorIndex != 0)
        {
            foreach (var button in colorButtonsEdit)
            {
                button.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
        
    }


}
