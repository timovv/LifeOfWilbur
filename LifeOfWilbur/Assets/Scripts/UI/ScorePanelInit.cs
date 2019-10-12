using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ScorePanelInit : MonoBehaviour
{
    [Header("Scoreboard")]
    public Transform _container;
    public Transform _entryTemplate;

    private string ScoreboardPath => $"{Application.persistentDataPath}/scoreboard.json";
    private Color userHilight = new Color(1f, 0.878f, 0.212f);

    void Awake()
    {
        List<Entry> entries = LoadEntries();

        Entry newEntry = new Entry(GameTimer.FormattedElapsedTime);
        entries.Add(newEntry);

        UpdateList(entries, newEntry);

        SaveScores(entries);
    }

    private void UpdateList(List<Entry> entries, Entry newEntry)
    {
        entries.Sort();

        int userRank = entries.IndexOf(newEntry);
        int nEntries = entries.Count;

        for (int i = 0; i < Math.Min(nEntries, 10); i++)
        {
            AddListItem("#" + (i + 1), entries[i]._time, userRank == i);
        }

        if (userRank >= 20)
        {
            AddListItem("...", "...", false);
        }
        int startIndex = userRank - userRank % 10;

        if (startIndex >= 10)
        {
            for (int i = startIndex; i < Math.Min(nEntries, startIndex + 10); i++)
            {
                AddListItem("#" + (i + 1), entries[i]._time, userRank == i);
            }
        }

    }

    private void AddListItem(string posValue, string timeValue, bool isUsers)
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI pos = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();

        pos.text = posValue;
        time.text = timeValue;

        if(isUsers)
        {
            pos.color = userHilight;
            time.color = userHilight;
        }
    }

    private List<Entry> LoadEntries()
    {
        if (!File.Exists(ScoreboardPath))
        {
            File.Create(ScoreboardPath).Dispose();
            return new List<Entry>();
        }

        using (StreamReader stream = new StreamReader(ScoreboardPath))
        {
            string contents = stream.ReadToEnd();
            return JsonUtility.FromJson<ListWrapper>(contents)._entries;
        }
    }

    private void SaveScores(List<Entry> entries)
    {
        using (StreamWriter stream = new StreamWriter(ScoreboardPath))
        {
            string output = JsonUtility.ToJson(new ListWrapper(entries), true);
            stream.Write(output);
        }
    }
}

struct ListWrapper
{
    public List<Entry> _entries;

    public ListWrapper(List<Entry> entries)
    {
        _entries = entries;
    }
}

[Serializable]
struct Entry : IComparable
{
    public string _time;

    public Entry(string time)
    {
        _time = time;
    }

    int IComparable.CompareTo(object obj)
    {
        Entry other = (Entry)obj;
        return _time.CompareTo(other._time);
    }
}
