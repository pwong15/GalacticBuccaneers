using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Views;

public class Storage {

    // levelCompleted: 1=successful completion, -1=fail, 0=na
    public static void SaveEncounterInfo(int numCredits, int levelCompleted, List<Character> characters) {
        List<string> lines = new List<string>();
        string writeFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Encounter.txt";

        lines.Add(numCredits.ToString());
        lines.Add(levelCompleted.ToString());
        foreach (Character character in characters)
            lines.Add(character.Serialize());

        File.WriteAllLines(writeFile, lines);
    }

    // Dictionary key example: "credits": "2010" , "levelCompleted": "-1" , "char1": "json", "char2: "json", ... "char4":
    public static Dictionary<string, string> LoadEncounterInfo() {

        Dictionary<string, string> data = new Dictionary<string, string>();
        string readFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\Encounter.txt";
        List<string> lines = new List<string>();

        // Read in all lines
        foreach (string line in File.ReadLines(readFile)) {
            lines.Add(line);
        }

        data["credits"] = lines[0];
        data["levelCompleted"] = lines[1];
        data["char1"] = lines[2];
        data["char2"] = lines[3];
        data["char3"] = lines[4];
        data["char4"] = lines[5];

        return data;
    }
}
