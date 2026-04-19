using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform cam;

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main.transform;
        transform.LookAt(transform.position + cam.forward);
    }

    public void SetMaxHealth(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }
    public void UpdateHealth(float val)
    {
        slider.value = val;
    }
}
