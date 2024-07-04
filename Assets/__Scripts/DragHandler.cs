using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragHandler : MonoBehaviour
{
    public VerticalDrag drag;
    public Color[] colors;
    public Image image;
    public Image image2;
    public Button settingButton;

    void Start()
    {
        if (gameObject.tag == "Interval")
        {
            EditInputPanel panelManager = FindObjectOfType<EditInputPanel>();
            if (panelManager != null)
            {
                settingButton.onClick.AddListener(() => panelManager.ShowPanel(GetComponent<SwipeToDelete>().index, this.gameObject));
            }
        }
        
    }
    public void EditSetup(bool isEditMode)
    {
        drag.isEdit = isEditMode;
        transform.GetChild(0).gameObject.SetActive(!isEditMode);
        transform.GetChild(1).gameObject.SetActive(!isEditMode);
        transform.GetChild(2).gameObject.SetActive(isEditMode);
        if (isEditMode)
        {
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = transform.GetChild(1).GetChild(0).GetComponent<Text>().text;
            transform.GetChild(2).GetChild(1).GetComponent<Text>().text = transform.GetChild(1).GetChild(1).GetComponent<Text>().text;
            //index = GetComponent<SwipeToDelete>().index;
        }
        else
        {
            //GetComponent<SwipeToDelete>().index = index;
        }
        GetComponent<SwipeToDelete>().enabled = !isEditMode;
    }

    public void SetColor(int index)
    {
        image.color = colors[index];
        image2.color = colors[index];
    }

}
