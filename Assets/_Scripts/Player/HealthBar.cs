using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill, hearthLogo;
    public TextMeshProUGUI healthCount;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthCount.text = health + "/" + slider.maxValue;

        fill.color = gradient.Evaluate(1f);
        hearthLogo.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        healthCount.text = slider.value + "/" + slider.maxValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        hearthLogo.color = gradient.Evaluate(slider.normalizedValue);
    }
}
