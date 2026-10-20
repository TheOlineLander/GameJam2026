using TMPro;
using UnityEngine;

public class UISlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stringContent;
    [SerializeField] private TextMeshProUGUI valueContent;

    public void SetContent(string name, int value)
    {
        stringContent.text = name;
        valueContent.text = value.ToString();
    }
}
