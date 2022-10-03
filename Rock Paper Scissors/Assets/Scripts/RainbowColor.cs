using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowColor : MonoBehaviour
{
    [SerializeField] private Color c1 = new Color(255f / 255f, 0f / 255f, 0f / 255f), 
                                   c2 = new Color(255f / 255f, 255f / 255f, 0f / 255f), 
                                   c3 = new Color(0f / 255f, 255f / 255f, 0f / 255f), 
                                   c4 = new Color(0f / 255f, 255f / 255f, 255f / 255f), 
                                   c5 = new Color(0f / 255f, 0f / 255f, 255f / 255f), 
                                   c6 = new Color(255f / 255f, 0f / 255f, 255f / 255f);
    [SerializeField] Color currentColor;
    Image img;
    public float changeTime = 0.1f;
    [SerializeField] float timer = 0f;
    [SerializeField] int cycle = 1;

    void Start()
    {
        img = GetComponent<Image>();
        img.color = c1;
        currentColor = c1;
    }

    void Update()
    {
        if (timer < changeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            switch (cycle)
            {
                case 1:
                    currentColor = c1;
                    cycle = 2;
                    break;
                case 2:
                    currentColor = c2;
                    cycle = 3;
                    break;
                case 3:
                    currentColor = c3;
                    cycle = 4;
                    break;
                case 4:
                    currentColor = c4;
                    cycle = 5;
                    break;
                case 5:
                    currentColor = c5;
                    cycle = 6;
                    break;
                case 6:
                    currentColor = c6;
                    cycle = 1;
                    break;
            }
            timer = 0f;
        }

        img.color = Color.Lerp(img.color, currentColor, 10f * Time.deltaTime);
    }
}
