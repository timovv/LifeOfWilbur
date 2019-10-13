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
    public Color _userHighlight = new Color(1f, 0.878f, 0.212f);
    public int _scoresShown = 10;

    private string ScoreboardPath => $"{Application.persistentDataPath}/scoreboard.json";

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
        // Most of this should be on the server when implemented
        entries.Sort();

        int userRank = entries.IndexOf(newEntry);

        for (int i = 0; i < Math.Min(entries.Count, _scoresShown); i++)
        {
            AddListItem("#" + (i + 1), entries[i]._time, userRank == i);
        }

        if (userRank >= _scoresShown * 2)
        {
            AddListItem("...", "...", false);
        }
        int startIndex = Math.Max(userRank - _scoresShown / 2, _scoresShown);
        int endIndex = Math.Min(startIndex + _scoresShown, entries.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            AddListItem("#" + (i + 1), entries[i]._time, userRank == i);
        }
    }

    private void AddListItem(string posValue, string timeValue, bool isUsers)
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI pos = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();

        pos.text = posValue;
        time.text = timeValue;

        if (isUsers)
        {
            pos.color = _userHighlight;
            time.color = _userHighlight;
        }
    }

    private List<Entry> LoadEntries()
    {
        // Change to a get request
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
        // Change to POSTing a single score
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
