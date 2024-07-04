using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPagePanel : MonoBehaviour
{
    public Button button;
    public PanelManager panelManager;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        panelManager = FindObjectOfType<PanelManager>();
        if (panelManager != null)
        {
            // Register the OnClick event dynamically
            button.onClick.AddListener(() => panelManager.OnButtonClick(button));
        }
        else
        {
            Debug.LogError("PanelManager not found in the scene.");
        }
    }
}
