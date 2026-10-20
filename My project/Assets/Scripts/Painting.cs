using UnityEngine;
using TMPro;

public class Painting : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private bool isReal;
    private Sprite painting;

    private SpriteRenderer rend;

    public void Constructor(bool isReal, Sprite painting)
    {
         this.isReal = isReal;
         this.painting = painting;

        rend = GetComponent<SpriteRenderer>();

        rend.sprite = this.painting;

        text.text = "This is AI = " + isReal;
    }
}
