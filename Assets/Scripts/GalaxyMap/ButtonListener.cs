using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace GalaxyMap
{
    public class ButtonListener : MonoBehaviour {
        public void ToggleQuit(string objectName) {
            CanvasGroup inventory = GameObject.Find(objectName).GetComponent<CanvasGroup>();

            inventory.alpha = inventory.alpha == 0 ? inventory.alpha = 1 : inventory.alpha = 0;
        }

        public void ShowGear(string gear) {
            GameObject obj = GameObject.Find(gear);
            CanvasGroup gearGroup = GameObject.Find(gear).GetComponent<CanvasGroup>();
            gearGroup.alpha = gearGroup.alpha == 0 ? gearGroup.alpha = 1 : gearGroup.alpha = 0;

            List<Button> buttons = new List<Button>();

            var objs = GameObject.FindGameObjectsWithTag(gear);
            foreach(GameObject temp in objs) {
                buttons.Add(temp.GetComponent<Button>());
            }

            foreach (Button button in buttons) {
                if (gearGroup.alpha == 1) {
                    button.interactable = true;
                }
                else {
                    button.interactable = false;
                }
            }
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
            Dictionary<string, string> data = Storage.LoadEncounterInfo();
            List<Character> characters = new List<Character>();

            characters.Add(Character.Deserialize(data["char1"]));
            characters.Add(Character.Deserialize(data["char2"]));
            characters.Add(Character.Deserialize(data["char3"]));
            characters.Add(Character.Deserialize(data["char4"]));

            var args = char_skill_amnt.Split(' ');
            int characterIndex = System.Int32.Parse(args[0]) - 1; 
            var skill = args[1];
            int upgradeAmnt = System.Int32.Parse(args[2]);

            string creditText = GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text;
            int availableCredits = System.Int32.Parse(creditText);

            if(availableCredits >= upgradeAmnt) {
                availableCredits -= upgradeAmnt;
                GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text = availableCredits.ToString();
                string newText = characters[characterIndex].UpgradeAttribute(skill, upgradeAmnt);
                string textFieldName = args[0] + " " + args[1];
                GameObject.Find(textFieldName).GetComponent <TextMeshProUGUI>().text = newText;
            }
            Storage.SaveEncounterInfo(availableCredits, 0, characters);
        }

        public void Quit(string save) {
            string fogFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\FogS.txt";
            string pathsFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Paths.txt";
            string locationFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Location.txt";
            string encounterFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Encounter.txt";
            List<string> encounterData = new List<string> { "2020", "0" };

            if (save != ""){
                foreach (Character character in Character.GetCurrentCharacters())
                    encounterData.Add(character.Serialize());
                encounterData[0] = GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text;
            }

            if (save == "") {
                foreach (Character character in Character.GetDefaultCharacters())
                    encounterData.Add(character.Serialize());

                File.WriteAllText(fogFile, string.Empty);
                File.WriteAllText(pathsFile, string.Empty);
                File.WriteAllText(locationFile, "0 -25 1 10 10.81 -6 0 -25, 1");
            }

            File.WriteAllLines(encounterFile, encounterData);

            EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}