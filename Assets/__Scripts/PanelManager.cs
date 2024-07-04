using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public Text displayText;
    public GameObject page1;
    public GameObject page2;
    public GameObject inputPanel1;
    public GameObject inputPanel2;
    public GameObject inputPanel3;
    public StoreTimer storeTimer;
    public void OnButtonClick(Button button)
    {
        if (!CheckReady())
        {
            return;
        }
        page2.SetActive(true);
        displayText.text = button.gameObject.transform.GetChild(0).GetComponent<Text>().text;
        page1.SetActive(false);
        int index = button.gameObject.transform.parent.GetComponent<SwipeToDelete>().index;
        storeTimer.sequenceIndex = index;
        storeTimer.LoadTimerData(index);
    }

    public bool CheckReady()
    {
        return !inputPanel1.activeInHierarchy && !inputPanel2.activeInHierarchy && !inputPanel3.activeInHierarchy;
    }
}
