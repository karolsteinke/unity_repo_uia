using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLoadingBillboard : MonoBehaviour
{
    public void Operate() {
        //Call method which requests image from server and calls callback once ready
        Managers.Images.GetWebImage(OnWebImage);
    }

    //Callback to apply downloaded image to the material
    private void OnWebImage(Texture2D image) {
        GetComponent<Renderer>().material.mainTexture = image;
    }
}
