using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Travel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onMouseDown()
    {
        if(gameObject.name == "gotoL1")
        {
            
            SceneManager.LoadScene("Level 1");
        }
        if (gameObject.name == "gotoL2")
        {
            //Console.Write("Level 2");
            SceneManager.LoadScene("Level 2");
        }
        if (gameObject.name == "gotoL3")
        {
            //Console.Write("Level 3");
            SceneManager.LoadScene("Level 3");
        }
    }
}
