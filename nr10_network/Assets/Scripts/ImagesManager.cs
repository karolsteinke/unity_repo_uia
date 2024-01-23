using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImagesManager : MonoBehaviour, IGameManager {
    public ManagerStatus status {get; private set;}
    private NetworkService _network;
    private Texture2D _webImage;

    public void Startup(NetworkService service) {
        Debug.Log("Images manager starting...");
        _network = service;

        status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback) {
        if (_webImage == null) {
            //Call coroutine which requests image from server and calls callback once ready
            //Lambda function is executed as callback inside DownloadImage to save _webImage
            StartCoroutine(_network.DownloadImage(
                (Texture2D image) => {
                    _webImage = image;
                    callback(_webImage);
                    }
                ));
        }
        else {
            //Invoke callback right away if there's a stored image
            callback(_webImage);
        }
    }
}
