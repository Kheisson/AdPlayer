using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace CrossPromo.Scripts
{
    public class CrossPromoPlayer : MonoBehaviour, IPromoPlayer
    {
        [SerializeField] private int playerId;

        [SerializeField] private VideoPlayer videoPlayer;

        private string serverEndpoint = "https://run.mocky.io/v3/81fab340-9550-4ab4-8859-836b01ee48ff";

        private List<CrossPromoMetaData> _metaDataList;

        private int _currentVideoIndex = 0;

        private Coroutine _videoPlaylistCoroutine;

        private Dictionary<string, bool> _trackingUrls = new Dictionary<string, bool>();

        private void Awake()
        {
            if (GameObject.FindObjectOfType<EventSystem>() is null)
            {
                gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<StandaloneInputModule>();
            }
        }

        private IEnumerator Start()
        {
            var req = new GetCrossPromoMetaDataRequest(serverEndpoint);

            yield return StartCoroutine(req.SendRequest());

            _metaDataList = req.Data.results;

            _videoPlaylistCoroutine = StartCoroutine(RunVideo());
        }

        private IEnumerator RunVideo()
        {
            if (videoPlayer.url != _metaDataList[_currentVideoIndex].video_url)
            {
                videoPlayer.url = _metaDataList[_currentVideoIndex].video_url;
                videoPlayer.Prepare();
            }

            while (!videoPlayer.isPrepared)
                yield return new WaitForSeconds(0.5f);

            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                Debug.Log($"Playing video '{_currentVideoIndex}'");
                yield return null;
            }

            if (_currentVideoIndex < _metaDataList.Count && videoPlayer.isPaused)
            {
                _currentVideoIndex++;
                _currentVideoIndex = Mathf.Clamp(_currentVideoIndex, 0, _metaDataList.Count - 1);
                CoroutineCheck();
            }
        }

        private void CoroutineCheck()
        {
            if (_videoPlaylistCoroutine != null)
            {
                StopCoroutine(_videoPlaylistCoroutine);
            }

            _videoPlaylistCoroutine = StartCoroutine(RunVideo());
        }

        public void Next()
        {
            _currentVideoIndex++;
            _currentVideoIndex = Mathf.Clamp(_currentVideoIndex, 0, _metaDataList.Count - 1);
            if (_currentVideoIndex >= _metaDataList.Count)
            {
                Debug.LogError($"There are only {_currentVideoIndex} in playlist and this is the last one");
            }
            else
            {
                CoroutineCheck();
            }
        }

        public void Pause()
        {
            if (videoPlayer.isPaused)
                return;
            StopCoroutine(_videoPlaylistCoroutine);
            videoPlayer.Pause();
        }

        public void Previous()
        {
            _currentVideoIndex--;
            _currentVideoIndex = Mathf.Clamp(_currentVideoIndex, 0, _metaDataList.Count - 1);

            if (_currentVideoIndex < 0)
            {
                Debug.LogError($"There are only {_currentVideoIndex} in playlist and this is the first one");
            }
            else
            {
                CoroutineCheck();
            }
        }

        public void Resume()
        {
            if (videoPlayer.isPlaying)
                return;
            CoroutineCheck();
        }


        public void GetCurrentVideo()
        {
            Pause();
            var trackingUrl = _metaDataList[_currentVideoIndex].tracking_url
                .Replace("[PLAYER_ID]", playerId.ToString());
            if (!_trackingUrls.ContainsKey(trackingUrl))
            {
                _trackingUrls.Add(trackingUrl, true);
                var post = new PostCrossPromoMetaDataRequest(trackingUrl, string.Empty);
                StartCoroutine(post.SendRequest());
            }
            
            Application.OpenURL(_metaDataList[_currentVideoIndex].click_url);
        }
    }
}