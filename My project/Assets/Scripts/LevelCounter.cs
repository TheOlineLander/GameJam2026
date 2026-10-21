using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    private int currantLevel;

    void Start(){DontDestroyOnLoad(gameObject);}

    private void OnLevelWasLoaded(int level){currantLevel++; print("Level:" + currantLevel); }

    public int GetCurrantLevel(){return currantLevel;}
}
