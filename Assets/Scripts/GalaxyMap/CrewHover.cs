using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewHover : MonoBehaviour
{
    public string gear;

    // Update is called once per frame
    void Update() {
        //if (Input.GetMouseButtonDown(0){
            CanvasGroup gearGroup = GameObject.Find(gear).GetComponent<CanvasGroup>();

        bool mouseHovering = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(0) && mouseHovering){
                gearGroup.alpha = 1;
            }
            //else {
            //    //gearGroup.alpha = 0;
            //}
        }
    }

