using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;

public class GoogleMapInterface : MonoBehaviour
{
    private const string BASE_URL = "https://maps.googleapis.com/maps/api/staticmap?";
    private const string API_KEY = "AIzaSyBfpTTdjktwg5_d5WVNSpoB8XSDEsgR9Ss";
    private Texture2D _cachedTexture;


    public void LoadMap(float latitude, float longitude, float zoom, Vector2 size, Action<Texture2D> onComplete)
    {
        StartCoroutine(C_LoadMap(latitude, longitude, zoom, size, onComplete));
    }

    IEnumerator C_LoadMap(float latitude, float longitude, float zoom, Vector2 size, Action<Texture2D> onComplete)
    {
        string url =
            BASE_URL +
            "center=" + latitude + "," + longitude +
            "&zoom=" + zoom.ToString() +
            "&size=" + size.x.ToString() + "x" + size.y.ToString() +
            "&key=" + API_KEY;

        Debug.Log($"[{nameof(GoogleMapInterface)}] : Request map texture ... {url}");

        url = UnityWebRequest.UnEscapeURL(url); 
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url); 

        yield return req.SendWebRequest();  

        _cachedTexture = DownloadHandlerTexture.GetContent(req); 
        onComplete.Invoke(_cachedTexture);
    }
}
