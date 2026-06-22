using UnityEngine;
using UnityEngine.UI;

public class SafeDigit : MonoBehaviour
{
    private Text digitText;

    public int value = 0;

    private void Awake()
    {
        digitText = GetComponentInChildren<Text>();

        value = 0;
        UpdateDisplay();
    }

    public void Increment()
    {
        value++;

        if (value > 7)
            value = 0;

        UpdateDisplay();
    }

    public int Value => value;

    private void UpdateDisplay()
    {
        digitText.text = value.ToString();
    }
}