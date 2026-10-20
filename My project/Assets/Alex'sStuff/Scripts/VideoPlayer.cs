using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    [SerializeField] private VideoClip clip;

    private VideoPlayer videoPlayer;
    private VideoClip currentClip;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        currentClip = clip;
    }

    public void PlayVideoClip(VideoClip clipToPlay = null)
    {
        if (clipToPlay != null)
            videoPlayer.clip = clipToPlay;

        videoPlayer.Play();
    }
}
