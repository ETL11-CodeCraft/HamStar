using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class GoogleMapInterface : MonoBehaviour
{
    private const string BASE_URL = "https://maps.googleapis.com/maps/api/staticmap?";
    private string API_KEY = "";
    private Texture2D _cachedTexture;
    private TextAsset _textFile;


    public void LoadMap(float latitude, float longitude, float zoom, Vector2 size, Action<Texture2D> onComplete)
    {
        try
        {
            _textFile = Resources.Load<TextAsset>("MapApiKey.txt");    //텍스트 에셋에 메모장 파일 불러오기
            Debug.Log($"{_textFile.text} MapApiKey");

            API_KEY = _textFile.text;

            StartCoroutine(C_LoadMap(latitude, longitude, zoom, size, onComplete));
        }
        catch (IOException e)
        {
            Debug.LogError($"파일을 읽는 도중 오류 발생: {e.Message}");
        }

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
