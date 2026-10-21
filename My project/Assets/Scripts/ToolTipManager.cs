using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI tooltipTextComponent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideTooltip();
    }

    public void HideTooltip()
    {
        tooltipTextComponent.text = string.Empty;
        tooltipTextComponent.gameObject.SetActive(false);
    }

    public void ShowTooltip(string message)
    {
        tooltipTextComponent.text = message;
        tooltipTextComponent.gameObject.SetActive(true);
    }
}

