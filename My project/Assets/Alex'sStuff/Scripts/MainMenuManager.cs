using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayerObject;
    [SerializeField] private GameObject videoImage;

    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
    }

    public void PlayUIVideo()
    {
        videoPlayerObject.SetActive(true);
        videoImage.SetActive(true);
        videoPlayer.Play();
    }
}
