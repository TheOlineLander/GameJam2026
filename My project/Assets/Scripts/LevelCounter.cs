using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    private int currantLevel;
    private float totalFines;

    void Start(){DontDestroyOnLoad(gameObject);}

    private void OnLevelWasLoaded(int level){currantLevel++; print("Level:" + currantLevel); }

    public int GetCurrantLevel(){
        Debug.Log("Current Level:" + currantLevel); return currantLevel;}

    public float GetTotalFines(){return totalFines; }

    public void AddFine(float fine)
    {
        totalFines += fine;
    }
}
