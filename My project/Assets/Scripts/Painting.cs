using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph.Internal;
using System.Collections;
using UnityEngine.Assemblies;

public class Painting : MonoBehaviour
{
    [SerializeField] private float dissolveSpeed, dissolveAmount;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Material dissolve;
    [SerializeField] private ShaderGraphVfxAsset shader;
    [SerializeField] private AudioSource audioSource; //HANNA

    [SerializeField] private GameObject burnEffect;

    private float currentDissolve;

    private bool isReal, canDestroy, destroyed;
    private Sprite painting;

    private SpriteRenderer rend;

    private GameObject gameHandler, zippo;

    public void Constructor(bool isReal, Sprite painting, GameObject gameHandler)
    {
         this.isReal = isReal;
         this.painting = painting;
         this.gameHandler = gameHandler;

        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();//HANNA

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
                audioSource.Play(); //HANNA
                DestroyPainting();
            }
        }
    }

    private void DestroyPainting()
    {
        if (!destroyed)
        {
            destroyed = true;
            zippo = GameObject.FindGameObjectWithTag("Zip");
            zippo.GetComponent<Animator>().SetTrigger("Light");

            StartCoroutine(Burn());
        }
    }

    private IEnumerator Burn()
    {
        gameHandler.GetComponent<GameHandler>().RemoveMe(!isReal);
        yield return new WaitForSeconds(0.1f);

        dissolve.SetTexture("_Painting", painting.texture);
        rend.material = dissolve;
        currentDissolve = 0;

        burnEffect.SetActive(true);

        while (currentDissolve < 1)
        {
            dissolve.SetFloat("_DissolveStrength", currentDissolve);
            currentDissolve += dissolveAmount;
            yield return new WaitForSeconds(dissolveSpeed);
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
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
