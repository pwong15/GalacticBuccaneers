using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GalaxyMap
{
    public class GalaxyMap : MonoBehaviour
    {
        public void GoToLocation1()
        {
            SceneManager.LoadScene("Spaceship 1");
        }


        public void GoToLocation2()
        {
            SceneManager.LoadScene("Encounter");
        }
    }
}
