
using UnityEngine;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject painting;

    private int level = 0;

    private Level[] levelSettings;

    private Sprite[] real, fake;

    private GameObject[] paintingsPlaced;

    private void Start(){DontDestroyOnLoad(gameObject);}

    private void OnLevelWasLoaded(){SetupScene();}

    private void SetupScene()
    {
        level += 1;

        real = Resources.LoadAll<Sprite>("Real" + level);
        fake = Resources.LoadAll<Sprite>("AI" + level);

        Transform[] tempSpawnPoints = levelSettings[level].spawnPositions;
        int totalNumberOfPaintings = levelSettings[level].totalNumberOfPaintings;
        int numberOfAIPaintings = levelSettings[level].numberOfAIPaintings;
        int time = levelSettings[level].time;

        for (int i = totalNumberOfPaintings; i > 0; i--)
        {
            bool isReal = totalNumberOfPaintings > numberOfAIPaintings;
            int pos = Random.Range(0, tempSpawnPoints.Length - 1);
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

            GameObject instantiated = Instantiate(painting, tempSpawnPoints[pos]);

            instantiated.GetComponent<Painting>().Constructor(isReal, paintingToSend);

            paintingsPlaced[i] = instantiated;

            tempSpawnPoints = RemoveAt(tempSpawnPoints, pos);

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
