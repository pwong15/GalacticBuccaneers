using System.IO;
using TMPro;
using UnityEditor;
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

        public void LevelUp(string char_skill_amnt) {
            var args = char_skill_amnt.Split(' ');
            var character = args[0];
            var skill = args[1];
            int upgradeAmnt = System.Int32.Parse(args[2]);

            string creditText = GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text;
            int availableCredits = System.Int32.Parse(creditText);

            if(availableCredits >= upgradeAmnt) {
                availableCredits -= upgradeAmnt;
                GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text = availableCredits.ToString();
                //@TODO upgrade here
            }

        }

        public void Quit() {
            string fogFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\FogS.txt";
            string pathsFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Paths.txt";
            File.WriteAllText(fogFile, string.Empty);
            File.WriteAllText(pathsFile, string.Empty);

            EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}