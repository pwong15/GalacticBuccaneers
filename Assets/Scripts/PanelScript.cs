using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public GameObject panel;
    public bool status;
    public void DisplayPanel()
    {
        panel.gameObject.SetActive(true);
        status = true;
    }
    public void HidePanel()
    {
        panel.gameObject.SetActive(false);
        status = false;
    }
    public bool PanelStatus()
    {
        return status;
    }

    public void Move()
    {
        Debug.Log("Move Button Clicked");
        HidePanel();
    }
    public void Attack()
    {
        Debug.Log("Attck Button Clicked");
        HidePanel();
    }
}
