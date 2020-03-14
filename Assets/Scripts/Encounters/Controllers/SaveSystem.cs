using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem {

    private const string SAVE_EXTENSION = "txt";
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Save/";
    private static bool isCreated = false;

    public static void Created() {
        if (!isCreated) {
            isCreated = true;
            if (!Directory.Exists(SAVE_FOLDER)) {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
    }

    public static string Load() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null) {
                mostRecentFile = fileInfo;
            } else {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
                    mostRecentFile = fileInfo;
                }
            }
        }
        if (mostRecentFile != null) {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        } else {
            return null;
        }
    }
}
