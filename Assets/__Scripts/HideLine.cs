using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLine : MonoBehaviour
{
    public GameObject scrollBar;
    public GameObject botLine;

    // Update is called once per frame
    void Update()
    {
        botLine.SetActive(scrollBar.activeSelf);
    }
}
