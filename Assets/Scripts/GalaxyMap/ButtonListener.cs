using UnityEngine;
namespace GalaxyMap
{
    public class ButtonListener : MonoBehaviour
    {
        public void ChangeVisibility(string objectName)
        {
            GameObject popout = GameObject.Find(objectName);
            var group = popout.GetComponent<CanvasGroup>();
            group.alpha = group.alpha == 0 ? group.alpha = 1 : group.alpha = 0;
        }
    }
}