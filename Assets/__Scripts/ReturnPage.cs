using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPage : MonoBehaviour
{
    public GameObject page1;
    public GameObject page2;

    public void Return()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }
}
