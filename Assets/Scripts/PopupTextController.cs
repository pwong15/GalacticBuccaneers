using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PopupTextController : MonoBehaviour
{
    private static PopupText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("CbtCanvas");
        popupText = Resources.Load<PopupText>("Prefabs/ParentPopup");
    }


    public static void CreatePopupText(string text, Transform location)
    {
        PopupText instance = Instantiate(popupText);
        instance.transform.SetParent(canvas.transform, false);
        instance.SetText(text);

    }
}
