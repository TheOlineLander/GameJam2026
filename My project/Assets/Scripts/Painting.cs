using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Painting : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private bool isReal, canDestroy;
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

    private void Update()
    {
        if (canDestroy)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                print("E-pressed");
                DestroyPainting();
            }
        }
    }

    private void DestroyPainting()
    {
        print("DESTROYING");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDestroy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDestroy = false;
        }
    }
}
