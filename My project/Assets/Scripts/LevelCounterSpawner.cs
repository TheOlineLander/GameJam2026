using UnityEngine;

public class LevelCounterSpawner : MonoBehaviour
{

    [SerializeField] private GameObject levelCounter;

    private LevelCounter levelCounterHere;
    void Start()
    {
        levelCounterHere = FindAnyObjectByType<LevelCounter>();


        if(levelCounterHere != null)
        {
            Destroy(levelCounterHere.gameObject);
        }

            Instantiate(levelCounter);
    }
}
