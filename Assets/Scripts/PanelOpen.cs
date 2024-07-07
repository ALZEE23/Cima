using UnityEngine;
using UnityEngine.UI;

public class PanelOpen : MonoBehaviour
{
    public GameObject panel; 
    public Button toggleButton; 

    void Start()
    {
        panel.SetActive(false);
        toggleButton.onClick.AddListener(TogglePanel);
    }

    void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
