using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class NetworkService {
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?q=Warsaw&mode=xml&appid=77baa391b2670e4ce261f2653c7ed7e2";
    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    //Method calls another 'IEnumerator' class, so it has no 'yield' (as it's nested deeper)
    public IEnumerator GetWeatherXML(Action<string> callback) {
        return CallAPI(xmlApi, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback) {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        
        yield return request.Send();
        //Retrive downloaded image using the DownloadHandler utility
        callback(DownloadHandlerTexture.GetContent(request));
    }

    //Method for HTTP request which calls callback once ready
    private IEnumerator CallAPI(string url, Action<string> callback) {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {

            yield return request.Send();

            if (request.isNetworkError) {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long) System.Net.HttpStatusCode.OK) {
                Debug.LogError("responser error: " + request.responseCode);
            }
            else {
                callback(request.downloadHandler.text);
            }
        }
    }
}
