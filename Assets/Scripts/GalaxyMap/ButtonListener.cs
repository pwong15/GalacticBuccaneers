using UnityEngine;
namespace GalaxyMap
{
    public class ButtonListener : MonoBehaviour
    {
        public void ChangeVisibility(string objectName)
        {
            CanvasGroup crewGroup = GameObject.Find("CrewPopout").GetComponent<CanvasGroup>();
            CanvasGroup invGroup = GameObject.Find("InventoryPopout").GetComponent<CanvasGroup>();

            CanvasGroup group = GameObject.Find(objectName).GetComponent<CanvasGroup>();
            group.alpha = group.alpha == 0 ? group.alpha = 1 : group.alpha = 0;

            if (objectName == "InventoryPopout")
                crewGroup.alpha = 0;
            else if (objectName == "CrewPopout")
                invGroup.alpha = 0;
        }
    }
}