using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    private Slider slider;
    public Color low;
    public Color high;

    public void UpdateHPBar(float currHealth, float maxHealth)
    {
        slider = this.transform.GetChild(0).GetComponent<Slider>();
        slider.gameObject.SetActive(currHealth < maxHealth);
        slider.value = currHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
