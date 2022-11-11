using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    void Awake()
    {
        instance = this;
    }

    public void UpdateTiredBar(float curHp, float maxHp)
    {
        tiredBar.value = curHp / maxHp;
    }

    public static Canvas instance = null;

    public Slider tiredBar;
}
