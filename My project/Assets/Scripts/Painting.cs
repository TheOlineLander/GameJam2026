using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Painting : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private bool isReal, canDestroy, destroyed;
    private Sprite painting;

    private SpriteRenderer rend;

    private GameObject gameHandler;

    public void Constructor(bool isReal, Sprite painting, GameObject gameHandler)
    {
         this.isReal = isReal;
         this.painting = painting;
         this.gameHandler = gameHandler;

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
        gameHandler.GetComponent<GameHandler>().RemoveMe(isReal);
        destroyed = true;
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
