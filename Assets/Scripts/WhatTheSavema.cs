using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class WhatTheSavema : MonoBehaviour
{
    public HighScores highScores = new HighScores();

    public (string, float, int, int) GetStats(int index)
    {
        SingleScore score = highScores.scores[index];
        return (score.name, score.time, score.day, score.fish);
    }

    public void Save()
    {
        SaverLoader.Save(highScores);
    }

    public void Load()
    {
        SaverLoader.Load();
    }
}

public class HighScores
{
    public List<SingleScore> scores;

    public void SortHighestTime()
    {
        scores.Sort((a,b)=>a.time.CompareTo(b.time));
    }
}

public class SingleScore
{
    public string name;
    public float time;
    public int day;
    public int fish;
}

public class SaverLoader
{
    // The stuff will be in AppData/LocalLow/Black Curtain/HighScores/HighScores.txt or similar
    public static string directory = "/HighScores/";
    public static string fileName = "HighScores.txt";

    public static void Save(HighScores hs)
    {
        // Get path to high scores folder
        string dir = Application.persistentDataPath + directory;

        // If there isn't a high score folder, make one
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // Serialize high scores to json
        string json = JsonUtility.ToJson(hs);

        // Write json to file
        File.WriteAllText(dir + fileName, json);
    }

    public static HighScores Load()
    {
        // Get path to high scores text file
        string fullPath = Application.persistentDataPath + directory + fileName;

        // get new high scores
        HighScores hs = new HighScores();

        // If the file exists, read it
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            hs = JsonUtility.FromJson<HighScores>(json);
        }
        else
        {
            Debug.Log("No high score file exists, new empty one created");
        }

        // Now you get scores, so epic
        return hs;
    }
}