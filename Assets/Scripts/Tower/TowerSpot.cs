using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private Tower placedTower;

    [Header("Highlight")]
    public MeshRenderer spotRenderer;
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color occupiedColor = Color.grey;

    void Start() => SetColor(defaultColor);

    void OnMouseEnter()
    {
        if (!IsOccupied) SetColor(hoverColor);
    }

    void OnMouseExit()
    {
        if (!IsOccupied) SetColor(defaultColor);
    }

    void OnMouseDown()
    {
        if (IsOccupied) return;
        TowerPlacementManager.Instance.TryPlaceTower(this);
    }

    public void PlaceTower(Tower tower)
    {
        IsOccupied = true;
        placedTower = tower;
        SetColor(occupiedColor);
    }

    void SetColor(Color c)
    {
        if (spotRenderer != null)
            spotRenderer.material.color = c;
    }
}