using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace Hawky.Video
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerManager : MonoSingleton<VideoPlayerManager>
    {
        private VideoPlayer _videoPlayer;
        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }
        public void PlayVideo(string videoPath)
        {
            StartCoroutine(PlayVideoRoutine(videoPath));
        }
        private IEnumerator PlayVideoRoutine(string videoFileName)
        {
        // Dừng video nếu đang chạy
            if (_videoPlayer.isPlaying)
                _videoPlayer.Stop();

            string videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);

    #if UNITY_ANDROID && !UNITY_EDITOR
            string url = videoPath; // Android không cần file://
    #else
            string url = "file://" + videoPath;
    #endif

            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = url;
            _videoPlayer.Prepare();

            // Đợi video chuẩn bị xong
            while (!_videoPlayer.isPrepared)
                yield return null;

            _videoPlayer.Play();
        }

        public void StopVideo()
        {
            if (_videoPlayer.isPlaying)
                _videoPlayer.Stop();
        }

        
    }
}

