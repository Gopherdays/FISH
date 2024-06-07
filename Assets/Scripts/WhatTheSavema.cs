using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

public class WhatTheSavema : MonoBehaviour
{
    //Actual list of the high scores
    public HighScores highScores = new HighScores();

    //Unpacked list of high scores
    public List<string> names;
    public List<float> times;
    public List<int> scores;
    public List<int> fishs;

    //High score texts
    public TextMeshProUGUI[] texts;

    //Sort scores by highest time
    public void ArrangeScores()
    {
        //Do the actual sorting in the background
        highScores.SortHighestTime();

        //Clear current stuff
        names.Clear();
        times.Clear();
        scores.Clear();
        fishs.Clear();

        //Unpack the scores, in order, now that they're sorted
        foreach (SingleScore thing in highScores.scores)
        {
            names.Add(thing.name);
            times.Add(thing.time);
            scores.Add(thing.score);
            fishs.Add(thing.fish);
        }

        //Blank the text, so += works
        foreach (TextMeshProUGUI text in texts)
        {
            text.text = "";
        }

        //Add each type of score stat into the corresponding text box
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

    //Tell the SaverLoader to save or load
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
    //List of scores, wow
    public List<SingleScore> scores;

    //Packs the scores into the custom .hydra format
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

    //Blank constructor
    public HighScores()
    {
        scores = new();
    }

    //Good thing they have a sorting function built in
    public void SortHighestTime()
    {
        scores.Sort((b, a) => a.time.CompareTo(b.time));
    }

    //Here's the actual loading part.
    public HighScores(string[] fileLines)
    {
        scores = new();
        //Get each string from the given array and do the following stuff to it
        foreach (string line in fileLines)
        {
            SingleScore score = new SingleScore();

            // Get indexes of bars in the line
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

            // Name is start of string to first bar
            score.name = (line[..(barIndex[0])]);

            // Score is first to second bar
            score.score = int.Parse(line[(barIndex[0] + 1)..(barIndex[1])]);

            // Time is second to third bar
            score.time = float.Parse(line[(barIndex[1] + 1)..(barIndex[2])]);

            // Unique fish is third bar to end of string
            score.fish = int.Parse(line[(barIndex[2] + 1)..(line.Length)]);

            // Add compiled single score to high score
            scores.Add(score);
        }
    }
}

//Class for a single entry into the high score
public class SingleScore
{
    public string name;
    public float time;
    public int score;
    public int fish;
    //Make out of actual data
    public SingleScore(string name, float time, int score, int fish)
    {
        this.name = name;
        this.time = time;
        this.score = score;
        this.fish = fish;
    }
    //Empty constructor
    public SingleScore()
    {

    }
}

//The thing that controls actually saving and actually loading
public class SaverLoader
{
    // The stuff will be in */AppData/LocalLow/Black Curtain/HighScores/HighScores.hydra or similar
    public static string directory = "/HighScores/";
    public static string fileName = "HighScores.hydra";

    //Save function
    public static void Save(HighScores hs)
    {
        // Get path to high scores folder
        string dir = Application.persistentDataPath + directory;

        // If there isn't a high score folder, make one
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log("No save directory, new one created");
        }

        // Write json to file
        File.WriteAllText(dir + fileName, hs.TurnIntoFile(), Encoding.ASCII);
        Debug.Log("Saved " + hs.scores.Count + " records to " + (dir + fileName));
    }

    //Load function
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