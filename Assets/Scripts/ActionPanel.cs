using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public GameObject actPanel;
    // Start is called before the first frame update
    public void Start()
    {
        actPanel.SetActive(false);
    }
    public void OnMouseOver()
    {
        actPanel.SetActive(true);
        Debug.Log("Mouse entered Player");
    }

    public void OnMouseExit()
    {
        actPanel.SetActive(true);
        Debug.Log("Mouse exit Player");
    }
}
