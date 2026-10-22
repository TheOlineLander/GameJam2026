using UnityEngine;

public class LevelCounterSpawner : MonoBehaviour
{

    [SerializeField] private GameObject levelCounter;
    void Start()
    {
        Instantiate(levelCounter);
    }
}
