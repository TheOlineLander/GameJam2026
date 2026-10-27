using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    private int currantLevel;
    [SerializeField] private float totalFines;

    void Start(){DontDestroyOnLoad(gameObject);}

    private void OnLevelWasLoaded(int level){currantLevel++; print("Level:" + currantLevel); }

    public int GetCurrantLevel(){
     return currantLevel;}

    public float GetTotalFines(){return totalFines; }

    public void AddFine(float fine)
    {
        totalFines += fine;
    }
}
