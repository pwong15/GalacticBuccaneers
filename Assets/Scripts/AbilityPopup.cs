using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Runtime.Versioning;
using System.Threading;
using System;
public class AbilityPopup : MonoBehaviour
{
     public static AbilityPopup Create(Vector3 position, string ability)
    {
        Transform abilityPopupTransform = Instantiate(GameAssets.i.pfAbilityPopup, position, Quaternion.identity);
        AbilityPopup abilityPopup = abilityPopupTransform.GetComponent<AbilityPopup>();
        abilityPopup.Setup(ability);

        return abilityPopup;
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(string ability)
    {
        textMesh.SetText("+" + ability);
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
