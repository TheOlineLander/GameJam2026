
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject painting;
    [SerializeField] private Transform[] spawnPoints;

    private int level = 0;

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
        real = Resources.LoadAll<Sprite>("Real" + level);
        fake = Resources.LoadAll<Sprite>("AI" + level);

        levelSettings = Resources.LoadAll<Level>("ScriptableObjects");
        print("Real Painting: " + real.Length);
        print("Fake Painting: " + fake.Length);

        Transform[] tempSpawnpoints = spawnPoints;
        int totalNumberOfPaintings = levelSettings[level - 1].totalNumberOfPaintings;
        int numberOfAIPaintings = levelSettings[level - 1].numberOfAIPaintings;
        int time = levelSettings[level - 1].time;

        for (int i = totalNumberOfPaintings; i > 0; i--)
        {
            bool isReal = i > numberOfAIPaintings;
            int pos = Random.Range(0, tempSpawnpoints.Length - 1);
            int pic;
            Sprite paintingToSend;

            print("isreal: " + isReal);
            print(real.Length);
            print(fake.Length);

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

            instantiated.GetComponent<Painting>().Constructor(!isReal, paintingToSend);

            paintingsPlaced[i] = instantiated;

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
