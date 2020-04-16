using UnityEngine;
using UnityEngine.UI;

namespace GalaxyMap
{
    public class ButtonListener : MonoBehaviour {
        public void ShowInventory(string objectName) {
            CanvasGroup inventory = GameObject.Find(objectName).GetComponent<CanvasGroup>();

            inventory.alpha = inventory.alpha == 0 ? inventory.alpha = 1 : inventory.alpha = 0;
        }


        public void ShowGear(string gear) {
            CanvasGroup gearGroup = GameObject.Find(gear).GetComponent<CanvasGroup>();
            gearGroup.alpha = gearGroup.alpha == 0 ? gearGroup.alpha = 1 : gearGroup.alpha = 0;
        }

        public void Glow(string buttonName) {
            Button button = GameObject.Find(buttonName).GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.red;
        }

        public void CloseWindow(string window) {
            CanvasGroup windowGroup = GameObject.Find(window).GetComponent<CanvasGroup>();
            windowGroup.alpha = 0;
        }
    }
}