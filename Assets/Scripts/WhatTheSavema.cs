using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

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
        highScores = SaverLoader.Load();
    }
}

public class HighScores
{
    public List<SingleScore> scores;

    public string TurnIntoFile()
    {
        string big = new("");
        foreach (SingleScore score in scores)
        {
            string line = score.name + '|' + score.score + '|' + score.time + '|' + score.fish;
            big += line + Environment.NewLine;
        }
        return big;
    }

    public HighScores()
    {
        scores = new();
    }

    public HighScores(string[] fileLines)
    {
        scores = new();
        foreach (string line in fileLines)
        {
            SingleScore score = new SingleScore();
            Debug.Log("Full thing: " + line);

            // Get indexes of bars in line
            List<int> barIndex = new List<int>();
            int pos = 0;
            int mark = 0;
            while (pos < line.Length && mark > -1)
            {
                mark = line.IndexOf('|', pos);
                if (mark == -1) break;
                barIndex.Add(mark);
                pos = mark + 1;
            }
            Debug.Log(barIndex);

            // Name is start of string to first bar
            Debug.Log("Name:" + line[..(barIndex[0] - 1)]);
            score.name = (line[..(barIndex[0])]);

            // Score is first to second bar
            Debug.Log("Score: " + line[(barIndex[0] + 1)..(barIndex[1])]);
            score.score = int.Parse(line[(barIndex[0] + 1)..(barIndex[1])]);

            // Time is second to third bar
            Debug.Log("Time: " + line[(barIndex[1] + 1)..(barIndex[2])]);
            score.time = float.Parse(line[(barIndex[1] + 1)..(barIndex[2])]);

            // Unique fish is third bar to end of string
            Debug.Log("Fish: " + line[(barIndex[2] + 1)..(line.Length)]);
            score.fish = int.Parse(line[(barIndex[2] + 1)..(line.Length)]);

            // Add compiled single score to high score
            scores.Add(score);
        }
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
    public SingleScore()
    {

    }
}

public class SaverLoader
{
    // The stuff will be in AppData/LocalLow/Black Curtain/HighScores/HighScores.txt or similar
    public static string directory = "/HighScores/";
    public static string fileName = "HighScores.hydra";

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

        // Write json to file
        File.WriteAllText(dir + fileName, hs.TurnIntoFile(), Encoding.ASCII);
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
            string[] file = File.ReadAllLines(fullPath, Encoding.ASCII);
            hs = new HighScores(file);
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