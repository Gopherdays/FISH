using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhatTheSavema : MonoBehaviour
{
    public HighScores highScores = new HighScores();

    public List<string> names;
    public List<float> times;
    public List<int> scores;
    public List<int> fishs;

    public TextMeshProUGUI[] texts;

    public void ArrangeScores()
    {
        highScores.SortHighestTime();
        names.Clear();
        times.Clear();
        scores.Clear();
        fishs.Clear();
        foreach (SingleScore thing in highScores.scores)
        {
            names.Add(thing.name);
            times.Add(thing.time);
            scores.Add(thing.score);
            fishs.Add(thing.fish);
        }
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = "";
        }
        for (int i = 0; i < 9; i++)
        {
            if (names.Count <= i)
                break;
            texts[0].text += names[i] + "\n";
        }
        for (int i = 0; i < 9; i++)
        {
            if (scores.Count <= i)
                break;
            texts[1].text += scores[i] + "\n";
        }
        for (int i = 0; i < 9; i++)
        {
            if (times.Count <= i)
                break;
            texts[2].text += GameManager.TimeFormat(times[i],true) + "\n";
        }
        for (int i = 0; i < 9; i++)
        {
            if (fishs.Count <= i)
                break;
            texts[3].text += fishs[i] + "\n";
        }
    }

    public (string, float, int, int) GetStats(int index)
    {
        SingleScore score = highScores.scores[index];
        return (score.name, score.time, score.score, score.fish);
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

    public HighScores()
    {
        scores = new();
    }

    public void SortHighestTime()
    {
        scores.Sort((b,a)=>a.time.CompareTo(b.time));
    }
}

public class SingleScore
{
    public string name;
    public float time;
    public int score;
    public int fish;
    public SingleScore(string name, float time, int score, int fish)
    {
        this.name = name;
        this.time = time;
        this.score = score;
        this.fish = fish;
    }
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
        {
            Directory.CreateDirectory(dir);
            Debug.Log("no save directory");
        }

        // Serialize high scores to json
        string json = JsonUtility.ToJson(hs);

        // Write json to file
        File.WriteAllText(dir + fileName, json);
        Debug.Log(JsonUtility.ToJson(hs));
        Debug.Log("Saved " + hs.scores.Count + " records to " + (dir + fileName));
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
            Debug.Log("Loaded " + hs.scores.Count + " records from " + fullPath);
        }
        else
        {
            Debug.Log("No high score file exists, new empty one created");
        }

        // Now you get scores, so epic
        return hs;
    }
}