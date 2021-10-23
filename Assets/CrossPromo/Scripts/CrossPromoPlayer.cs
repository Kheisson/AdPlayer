using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CrossPromo.Scripts
{
    public class CrossPromoPlayer : MonoBehaviour, IPromoPlayer
    {
        [SerializeField] private int playerId;

        [SerializeField] private VideoPlayer videoPlayer;

        [SerializeField] private Button adButton;

        private string serverEndpoint = "https://run.mocky.io/v3/81fab340-9550-4ab4-8859-836b01ee48ff";

        private List<CrossPromoMetaData> _metaDataList;

        private int _currentVideoIndex = 0;

        private bool _isRunning;

        private Coroutine _videoPlaylistCoroutine;

        private HashSet<string> _trackingUrls = new HashSet<string>();

        #region Public Events

        public event Action OnPlaylistFinished;
        public event Action OnPlaylistStarted;

        #endregion

        private void Awake()
        {
            _isRunning = true;

            //Required components in case canvas is missing defaulted event system GameObject
            if (FindObjectOfType<EventSystem>() is null)
            {
                gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<StandaloneInputModule>();
            }

            DontDestroyOnLoad(gameObject); //In case of instant load, GameObject is kept
            adButton.onClick.AddListener(VideoClicked);
        }

        private void OnDestroy()
        {
            adButton.onClick.RemoveListener(VideoClicked);
        }

        private IEnumerator Start()
        {
            var req = new GetCrossPromoRequest(serverEndpoint, error =>
            {
                Debug.LogError($"Web request failed, terminating player error: {error}");
                Destroy(gameObject);
            });

            yield return StartCoroutine(req.SendRequest());

            _metaDataList = req.DataDto.results
                .Select(dto => new CrossPromoMetaData(dto.id, dto.tracking_url, dto.click_url, dto.video_url))
                .ToList();

            OnPlaylistStarted?.Invoke();

            _videoPlaylistCoroutine = StartCoroutine(RunVideo());
        }

        private IEnumerator RunVideo()
        {
            if (videoPlayer.url != _metaDataList[_currentVideoIndex].VideoUrl)
            {
                videoPlayer.url = _metaDataList[_currentVideoIndex].VideoUrl;
                videoPlayer.Prepare();
            }

            while (!videoPlayer.isPrepared)
                yield return new WaitForSeconds(0.5f);

            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            if (_currentVideoIndex < _metaDataList.Count && videoPlayer.isPaused)
            {
                _currentVideoIndex++;
                _currentVideoIndex = Mathf.Clamp(_currentVideoIndex, 0, _metaDataList.Count);

                //Playlist finished playing all videos
                if (_currentVideoIndex == _metaDataList.Count)
                {
                    gameObject.SetActive(false);
                    yield break;
                }

                CoroutineCheck();
            }
        }

        private void CoroutineCheck()
        {
            //Coroutine stopper in case of interruption, e.g. - Pause is called.
            if (_videoPlaylistCoroutine != null)
            {
                StopCoroutine(_videoPlaylistCoroutine);
            }

            _videoPlaylistCoroutine = StartCoroutine(RunVideo());
        }

        private void VideoClicked()
        {
            //Attached to renderer, will be called when Ad is clicked
            var trackingUrl = _metaDataList[_currentVideoIndex].TrackingUrl
                .Replace("[PLAYER_ID]", playerId.ToString());

            if (!_trackingUrls.Contains(trackingUrl))
            {
                _trackingUrls.Add(trackingUrl);
                var post = new PostCrossPromoRequest(trackingUrl, error =>
                {
                    Debug.Log($"Failed to send tracking url, {error}");
                });
                StartCoroutine(post.SendRequest());
            }
            
            Application.OpenURL(_metaDataList[_currentVideoIndex].ClickUrl);
        }

        private void OnEnable()
        {
            if (_isRunning) return;
            OnPlaylistStarted?.Invoke();
            _currentVideoIndex = 0;
            _videoPlaylistCoroutine = StartCoroutine(RunVideo());
            videoPlayer.Play();
        }

        private void OnDisable()
        {
            OnPlaylistFinished?.Invoke();
            _isRunning = false;
            _videoPlaylistCoroutine = null;
        }

        #region Player Controls

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
            if (videoPlayer.isPaused) return;
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
            if (videoPlayer.isPlaying) return;
            CoroutineCheck();
        }

        #endregion
    }
}