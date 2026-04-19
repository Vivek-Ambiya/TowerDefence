using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Button backBtn;

    void OnEnable()
    {
        backBtn.onClick.AddListener(ClosePanel);
    }
    void OnDisable()
    {
        backBtn.onClick.RemoveListener(ClosePanel);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false); 
    }
}