using TMPro;
using UnityEngine;

public class UISlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameConent;
    [SerializeField] private TextMeshProUGUI valueContent;

    public void SetContent(string name, int value)
    {
        nameConent.text = name;
        valueContent.text = value.ToString();
    }
}
