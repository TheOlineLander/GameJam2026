
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject painting;
    [SerializeField] private Transform[] spawnPoints;

    private int level = 0;

    private int destroyedAI, destroyedReal;

    private Level[] levelSettings;

    private Sprite[] real, fake;

    private GameObject[] paintingsPlaced = new GameObject[15];

    private void Start(){
        DontDestroyOnLoad(gameObject); SetupScene(); 

    }

    private void OnLevelWasLoaded(){SetupScene();}

    private void SetupScene()
    {
        level += 1;
        print("level is:" + level);
        destroyedAI = 0; destroyedReal = 0;

        //DestroyPaintings(paintingsPlaced);
        

        real = Resources.LoadAll<Sprite>("Real" + level);
        fake = Resources.LoadAll<Sprite>("AI" + level);

        levelSettings = Resources.LoadAll<Level>("ScriptableObjects");

        paintingsPlaced = new GameObject[15];

        Transform[] tempSpawnpoints = spawnPoints;
        int totalNumberOfPaintings = levelSettings[level - 1].totalNumberOfPaintings;
        int numberOfAIPaintings = levelSettings[level - 1].numberOfAIPaintings;
        int time = levelSettings[level - 1].time;

        for (int i = totalNumberOfPaintings; i > 0; i--)
        {
            bool isReal = i !>= numberOfAIPaintings + 1;
            int pos = Random.Range(0, tempSpawnpoints.Length - 1);
            int pic;
            Sprite paintingToSend;

            if (isReal)
            {
                pic = Random.Range(0, real.Length - 1);
                
                paintingToSend = real[pic];
            }
            else
            {
                pic = Random.Range(0, fake.Length - 1);
                paintingToSend = fake[pic];
            }

            GameObject instantiated = Instantiate(painting, tempSpawnpoints[pos]);

            instantiated.GetComponent<Painting>().Constructor(!isReal, paintingToSend, gameObject);

            paintingsPlaced[i] = instantiated;

            print(instantiated);

            tempSpawnpoints = RemoveAt(tempSpawnpoints, pos);

            if (isReal)
            {
                real = RemoveAt(real, pic);
            }
            else
            {
                fake = RemoveAt(fake, pic);
            }
        } 
    }

    public int GetDestroyedAI()
    {
        return destroyedAI;
    }

    public int GetDestroyedReal()
    {
        return destroyedReal;
    }

    public int GetTotalAI()
    {
        return levelSettings[level - 1].numberOfAIPaintings;
    }


    private void DestroyPaintings(GameObject[] paintings)
    {
        foreach (GameObject painting in paintings)
        {
            Destroy(painting);
        }

    }

    public void RemoveMe(bool isReal)
    {
        if (isReal)
        {
            destroyedReal += 1;
        }
        else
        {
            destroyedAI += 1;
        }
    }

    public static T[] RemoveAt<T>(T[] array, int position)
    {
        if (position < 0 || position >= array.Length)
            return array;

        T[] newArray = new T[array.Length - 1];

        int j = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i == position)
                continue;

            newArray[j] = array[i];
            j++;
        }

        return newArray;
    }
}
