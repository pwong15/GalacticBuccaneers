﻿using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using Views;

namespace GalaxyMap
{
    public class Grid : global::Grid {
        public FogSquare[,] fog;
        public string paths = "";
        int lastLevelCompleted = 0;

        public void Start() {
            this.GRID_HEIGHT = 22;
            this.GRID_HEIGHT = 22;
            this.FILE_NAME = "GalaxyMap";

            IntializeValidMoves();
            CreateGrid();
            ShowSavedPaths();
            LoadFog();
            LoadEncounterInfo();
            LoadLocation();
            LoadUpgradeStats();
        }

        public override void CreateGrid() {
            int gridIndex = 0;
            gridSquares = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
            string loadFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\" + FILE_NAME + ".txt";
            string gridValues = string.Join("", File.ReadAllLines(loadFile));
            string sceneLink = "";

            // Create a GridSquare for each posistion on the board
            for (int row = 0; row < GRID_HEIGHT; row++) {
                for (int column = 0; column < GRID_WIDTH; column++) {
                    sceneLink = "";
                    GameObject gridVisual;
                    char gridValue = gridValues[gridIndex];
                    // Load the appropriate prefab based on the .txt file
                    switch (gridValue) {
                        case '1':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship1") as GameObject);
                            sceneLink = "Ship2";
                            break;
                        case '2':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship2") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '3':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship3") as GameObject);
                            sceneLink = "Ship2";
                            break;
                        case '4':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship4") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '5':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship5") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '6':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship6") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '7':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship7") as GameObject);
                            sceneLink = "Ship2";
                            break;
                        case '8':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship8") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '9':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship9") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        case '0':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship0") as GameObject);
                            sceneLink = "Ship2";
                            break;
                        case '!':
                            gridVisual = Instantiate(Resources.Load("Prefabs/ship10") as GameObject);
                            sceneLink = "Ship1";
                            break;
                        default:
                            gridVisual = Instantiate(Resources.Load("Prefabs/blankSquare") as GameObject);
                            break;

                    }


                    // Initialize and save each GridSquare
                    GridSquare gridSquare = gridVisual.AddComponent<GridSquare>();
                    gridSquare.Initialize(this, column, -row, -3);
                    gridSquare.LinkedScene = sceneLink;
                    gridSquare.shipChar = gridValue;
                    gridSquares[column, row] = gridSquare;
                    gridIndex++;
                }
            }
        }

        public void LoadFog() {
            string writeFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\FogS.txt";
            string readText = File.ReadAllText(writeFile);

            if (readText.Length == 0)
                CreateFog('E', '1');
            else
                CreateFog('S', ' ');
        }

        public void CreateFog(char first, char second) {
            fog = new FogSquare[GRID_HEIGHT, GRID_WIDTH];

            for (int row = 0; row < GRID_HEIGHT; row++) {
                for (int column = 0; column < GRID_WIDTH; column++) {
                    GameObject fogGameObject = Instantiate(Resources.Load("Prefabs/fog") as GameObject);
                    FogSquare fogSquare = fogGameObject.AddComponent<FogSquare>();
                    fogSquare.Initialize(this, column, -row, -7);
                    fog[column, row] = fogSquare;
                }
            }

            if(second == ' ') {
                RemoveFog(first);
            }
            else {
                RemoveFog(first);
                RemoveFog(second);
            }
        }

        public void IntializeValidMoves() {
            validSelections.Add('0', new List<char>() { '3', '4', '7' });
            validSelections.Add('1', new List<char>() { '2', '4', '5' });
            validSelections.Add('2', new List<char>() { '1', '3' });
            validSelections.Add('3', new List<char>() { '0', '2', '6' });
            validSelections.Add('4', new List<char>() { '0', '5', '1' });
            validSelections.Add('5', new List<char>() { '1', '4' });
            validSelections.Add('6', new List<char>() { '3', '7', '9' });
            validSelections.Add('7', new List<char>() { '0', '6', '8', '!' });
            validSelections.Add('8', new List<char>() { '7' });
            validSelections.Add('9', new List<char>() { '6', '!' });
            validSelections.Add('!', new List<char>() { '7', '9' });
            validSelections.Add('.', new List<char>() { });

            pathAssociations.Add('0', new List<string>() {"path0to3", "path0to7", "path0to4"});
            pathAssociations.Add('1', new List<string>() {"path1to4", "path1to5", "path1to2"});
            pathAssociations.Add('2', new List<string>() {"path2to3", "path1to2"});
            pathAssociations.Add('3', new List<string>() {"path2to3", "path3to6", "path0to3"});
            pathAssociations.Add('4', new List<string>() {"path4to5", "path0to4", "path1to4"});
            pathAssociations.Add('5', new List<string>() {"path5toRedPlanet", "path4to5", "path1to5"});
            pathAssociations.Add('6', new List<string>() {"path6to7", "path6to9", "path3to6"});
            pathAssociations.Add('7', new List<string>() {"path7to8", "path7to10", "path6to7", "path0to7", "path7toRedPlanet"});
            pathAssociations.Add('8', new List<string>() {"path8toEarth", "path7to8"});
            pathAssociations.Add('9', new List<string>() {"path9to10", "path6to9"});
            pathAssociations.Add('!', new List<string>() {"path7to10", "path10toEarth", "path9to10"});
        }

        public void ShowSavedPaths() {
            string readFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Paths.txt";
            paths = string.Join("", File.ReadAllLines(readFile));

            // Show saved paths
            if(paths.Length != 0) {
                foreach (char c in paths)
                    ShowPaths(c);
            }
        }

        public override void ShowPaths(char ship) {
            string writeFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Paths.txt";
            List<string> pathsToShow = pathAssociations[ship];

            foreach (var path in pathsToShow) {
                Vector3 curPos = GameObject.Find(path).transform.position;
                curPos.z = -2f;
                GameObject.Find(path).transform.position = curPos;
            }

            if (!paths.Contains(ship + "")) {
                paths += ship;
            }

            File.WriteAllText(writeFile, string.Empty);
            File.WriteAllText(writeFile, paths);
        }

        public override void RemoveFog(char removalKey) {
            string loadFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Fog" + removalKey + ".txt";
            string unfogValues = string.Join("", File.ReadAllLines(loadFile));
            int gridIndex = 0;

            // Hide end indicator after first turn
            if(removalKey == '2' | removalKey == '4' | removalKey == '5' | removalKey == 'S') {
                try {
                    GameObject.Find("EndGame").SetActive(false);
                }
                catch(Exception e) {}
            }

            // Remove fog squares
            for (int row = 0; row < GRID_HEIGHT; row++) {
                for (int column = 0; column < GRID_WIDTH; column++) {

                    // Avoid index outofbounds
                    if(gridIndex >= (GRID_WIDTH * GRID_HEIGHT -1)) {
                        return;
                    }

                    char unfogValue = unfogValues[gridIndex];
                    if (unfogValue == 'w') {
                        fog[column, row].gameObject.SetActive(false);
                    }
                    gridIndex++;
                }
            }
            SaveFog();
        }

        public override void SaveFog() {
            string writeFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\FogS.txt";
            string stringToWrite = "";

            for (int row = 0; row < GRID_HEIGHT; row++) {
                stringToWrite += "\n";
                for (int column = 0; column < GRID_WIDTH; column++) {
                    FogSquare square = fog[column, row];
                    if (square.isActiveAndEnabled) {
                        stringToWrite += '.';
                    }
                    else {
                        stringToWrite += 'w';
                    }
                }
            }
            File.WriteAllText(writeFile, string.Empty);
            File.WriteAllText(writeFile, stringToWrite);
        }

        public void LoadLocation() {
            string readFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Location.txt";
            GameObject greenPlaceMarker = GameObject.Find("PlaceMarker");
            string[] saveData = File.ReadAllText(readFile).Split(' ');

            // Set location if not null
            if (saveData.Length != 0) {
                float x = float.Parse(saveData[0]);
                float y = float.Parse(saveData[1]);
                char lastLocation = char.Parse(saveData[2]);
                float cameraDepth = float.Parse(saveData[3]);
                float camerax = float.Parse(saveData[4]);
                float cameray = float.Parse(saveData[5]);
                float nextLocationX = float.Parse(saveData[6]);
                float nextLocationY = float.Parse(saveData[7]);
                char nextLocation = char.Parse(saveData[8]);
                Vector3 newLocation;
                    
                // Don't move marker if lvl was failed
                if (lastLevelCompleted != 1) {
                    currentLocation = lastLocation;
                    newLocation = new Vector3(x, y, -1.0f);
                    greenPlaceMarker.transform.position = newLocation;
                }
                else {
                    currentLocation = nextLocation;
                    newLocation = new Vector3(nextLocationX, nextLocationY, -1.0f);
                    greenPlaceMarker.transform.position = newLocation;
                }

                Vector3 cameraLocation = new Vector3(camerax, cameray, -10.0f);

                Camera.main.transform.position = cameraLocation;
                Camera.main.orthographicSize = cameraDepth;
                SaveLocation(newLocation, currentLocation);
            }
        }

        public override void SaveLocation(Vector3 nextLocation, char nextShip) {
            string writeFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Location.txt";
            GameObject greenPlaceMarker = GameObject.Find("PlaceMarker");
            float currentX = greenPlaceMarker.transform.position.x;
            float currentY = greenPlaceMarker.transform.position.y;
            float cameraDepth = Camera.main.orthographicSize;
            float camerax = Camera.main.transform.position.x;
            float cameray = Camera.main.transform.position.y;


            string textToWrite =
                currentX.ToString() + " " +
                currentY.ToString() + " " +
                currentLocation + " " +
                cameraDepth.ToString() + " " +
                camerax.ToString() + " " +
                cameray.ToString() + " " +
                nextLocation.x.ToString() + " " +
                nextLocation.y.ToString() + " " +
                nextShip;

            File.WriteAllText(writeFile, string.Empty);
            File.WriteAllText(writeFile, textToWrite);
        }
        
        public override void LoadEncounterInfo() {
            Dictionary<string, string> data = Storage.LoadEncounterInfo();
            List<Character> characters = new List<Character>();

            characters.Add(Character.Deserialize(data["char1"]));
            characters.Add(Character.Deserialize(data["char2"]));
            characters.Add(Character.Deserialize(data["char3"]));
            characters.Add(Character.Deserialize(data["char4"]));

            GameObject.Find("NumCredits").GetComponent<TextMeshProUGUI>().text = data["credits"];
            lastLevelCompleted = Int32.Parse(data["levelCompleted"]);
        }

       public void LoadUpgradeStats() {
            string[] crew = { "1", "2", "3", "4" };
            string[] stats = { "health", "defense", "speed" };
            ButtonListener listener = new ButtonListener();
            foreach(var num in crew) {
                foreach(var skill in stats) {
                    listener.LevelUp(num + " " + skill + " " + 0);
                }
            }
        }
    }
}
