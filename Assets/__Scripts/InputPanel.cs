using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    public Transform page;

    public Button[] buttons;
    public RectTransform checkButton;
    public RectTransform checkButtonEdit;
    public AddSequence page2;
    void Awake()
    {
        //gameObject.SetActive(false);
    }

    public void Show()
    {
        SetButtonsStatus(false);
        checkButton.localScale = Vector3.one;
        gameObject.SetActive(true);
        SetUniversalButtonsStatus(false);
        if (page.GetComponent<EditMode>().mode == 1)
        {
            page2.GetComponent<AddSequence>().ClearColor();
        }
        else
        {
            if (checkButtonEdit != null)
            {
                checkButtonEdit.localScale = Vector3.one;
                page2.GetComponent<AddSequence>().ClearColorEdit();
            }
            
        }
    }

    public void Hide()
    {
        SetButtonsStatus(true);

        gameObject.SetActive(false);
        SetUniversalButtonsStatus(true);

    }

    private void SetButtonsStatus(bool status)
    {
        for (int i = 0; i < page.childCount; i++)
        {
            page.GetChild(i).GetChild(1).gameObject.GetComponent<Button>().enabled = status;
            
                
        }
        if (!status)
        {
            EditMode editMode = page.GetComponent<EditMode>();
            if (editMode.isEditMode)
            {
                page.GetComponent<EditMode>().ToggleEditMode();
            }
        //page.GetChild(i).GetComponent<DragHandler>().EditSetup(status);
        }
    }

    private void SetUniversalButtonsStatus(bool status)
    {
        foreach (Button button in buttons)
        {
            button.enabled = status;
            if (status)
            {
                if (button.gameObject.name == "EditButton" || button.gameObject.name == "Play Button")
                {
                    button.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                //page.GetComponent<EditMode>().isEditMode = false;
                }
            }
        }
    }

}
