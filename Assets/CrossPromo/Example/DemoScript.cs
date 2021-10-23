using CrossPromo.Scripts;
using UnityEngine;

/// <summary>
/// Demo Script for Example scene, to view documentation please visit
/// https://github.com/Kheisson/AdPlayer/blob/master/README.md
/// </summary>

public class DemoScript : MonoBehaviour
{
    public GameObject crossPromoPlayerPrefab; //CrossPromo Prefab should be placed in editor
    
    private CrossPromoPlayer _crossPromoInstance;
    
    private void Awake()
    {
        _crossPromoInstance = crossPromoPlayerPrefab.GetComponent<CrossPromoPlayer>();
        _crossPromoInstance.gameObject.SetActive(false);

        _crossPromoInstance.OnPlaylistStarted += PlaylistStarted;
        _crossPromoInstance.OnPlaylistFinished += PlaylistFinished;
    }

    private void OnDestroy()
    {
        _crossPromoInstance.OnPlaylistStarted -= PlaylistStarted;
        _crossPromoInstance.OnPlaylistFinished -= PlaylistFinished;
    }

    private void PlaylistStarted()
    {
        //Event that indicates that the crossPromo video player started playing
        Debug.Log("Playlist started, I should handle logic to stop gameplay");
    }
    
    private void PlaylistFinished()
    {
        //Event that indicates that the crossPromo video player finished playing
        Debug.Log("Playlist finished, I can continue gameplay ");
    }

    public void StartDemo()
    {
        _crossPromoInstance.gameObject.SetActive(true); //Will start playing the video loop
    }
    
    
}
