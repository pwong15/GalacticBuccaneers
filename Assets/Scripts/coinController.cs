using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coinController : MonoBehaviour
{
    public static int coinAmt = 0;
    Text coin;
    
    public void Start()
    {
        coin = GetComponent<Text>();
    }
    public void Update()
    {
        coin.text = " " + coinAmt;
    }
    public void setCoinAmt(int c)
    {
        coinAmt = c;
    }
}
