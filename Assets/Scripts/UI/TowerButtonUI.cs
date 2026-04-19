using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButtonUI : MonoBehaviour
{
    public TowerData towerData;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Button button;

    void Start()
    {
        if (icon && towerData.icon) icon.sprite = towerData.icon;
        if (nameText) nameText.text = towerData.towerName;
        if (costText)
        {
            if (towerData.gemCost > 0)
                costText.text = $"{towerData.gemCost}";
            else
                costText.text = $"{towerData.coinCost}";
        }
        button?.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        TowerPlacementManager.Instance.SelectTower(towerData);
        AudioManager.Instance?.PlaySFX("click");
    }
}