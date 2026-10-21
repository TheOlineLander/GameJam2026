using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogueSequence;
    private LevelCounter levelCounter;

    void Start()
    {
        levelCounter = FindAnyObjectByType<LevelCounter>();

    }

    // Update is called once per frame
    void Update()
    {
        levelCounter.GetCurrantLevel();
        if (levelCounter.GetCurrantLevel() == 0)
        {
            dialogueSequence.ShowAfterLevel1Dialogue();
        }
        else if (levelCounter.GetCurrantLevel() == 1)
        {
            dialogueSequence.ShowAfterLevel2Dialogue();
        }
        else if (levelCounter.GetCurrantLevel() == 2)
        {
            dialogueSequence.ShowAfterLevel3Dialogue();
        }
    }
}
