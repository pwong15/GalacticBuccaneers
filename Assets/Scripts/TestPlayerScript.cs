using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilitys;

public class TestPlayerScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int amt;
    public BarController healthBar;
    public coinController coin;
    public PanelScript panel;
  
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        panel.HidePanel();
        // coin.Update(currentAmt);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
            amt -= 10;
            coin.setCoinAmt(amt);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            IncreaseHealth(10);
            amt += 10;
            coin.setCoinAmt(amt);
        }

    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
    }
    void IncreaseHealth(int hel)
    {
        currentHealth += hel;
        healthBar.SetValue(currentHealth);
    }
    void OnMouseDown()
    {
        Debug.Log("Player Clicked");
        panel.DisplayPanel();
    }
    /*void IncreaseCoinAmt(int x)
    {
        currentAmt += x;
        coin.SetCoinAmt(currentAmt);
    }*/

}
