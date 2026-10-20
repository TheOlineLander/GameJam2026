using UnityEngine;

public class Painting : MonoBehaviour
{
    private bool isReal;
    private Sprite painting;

    private SpriteRenderer rend;

    public void Constructor(bool isReal, Sprite painting)
    {
         this.isReal = isReal;
         this.painting = painting;

        rend = GetComponent<SpriteRenderer>();

        rend.sprite = this.painting;
    }
}
