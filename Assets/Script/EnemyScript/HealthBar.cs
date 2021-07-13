using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elle2D
{

    //summary : this class is control health bar for enemy
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
        }
        public void SetHealth(int health)
        {
            slider.value = health;
        }
    }
}