using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreboardInit : MonoBehaviour
{
    public Transform _container;
    public Transform _entryTemplate;
    public Color _userHighlight = new Color(1f, 0.878f, 0.212f);

    public string _saveScoreEndPoint = "http://localhost:3000/scores";

    void Awake()
    {
        NewScore score = new NewScore {
            name = "P",
            time = GameTimer.ElapsedTimeSeconds,
            attempts = GameController.Resets,
            timeswaps = TimeTravelController.Timeswaps
        };
        StartCoroutine(PostScore(score));
    }

    private IEnumerator PostScore(NewScore score)
    {
        byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(score));
        UnityWebRequest request = UnityWebRequest.Post(_saveScoreEndPoint, "");
        request.uploadHandler = new UploadHandlerRaw(data);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error sending score: " + request.error);
            yield break;
        }

        Scores recieved = JsonUtility.FromJson<Scores>(request.downloadHandler.text);
        FillList(recieved);
    }

    private void FillList(Scores data)
    {
        foreach (ListEntry entry in data.top)
        {
            AddListItem(entry, entry._id == data.id);
        }
        int lastTopRank = data.top.Count;
        if (lastTopRank < data.near[0].rank)
        {
            AddSpacer();
        }

        foreach (ListEntry entry in data.near)
        {
            if (lastTopRank < entry.rank)
            {
                AddListItem(entry, entry._id == data.id);
            }
        }
    }

    private void AddListItem(ListEntry entry, bool shouldHighlight)
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI rank = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attempts = listItem.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswaps = listItem.GetChild(3).GetComponent<TextMeshProUGUI>();

        rank.text = String.Format("#{0:D6}", entry.rank);
        time.text = GameTimer.FormatTime(entry.time);
        attempts.text = entry.attempts.ToString();
        timeswaps.text = entry.timeswaps.ToString();

        if (shouldHighlight)
        {
            rank.color = _userHighlight;
            time.color = _userHighlight;
            attempts.color = _userHighlight;
            timeswaps.color = _userHighlight;
        }
    }

    private void AddSpacer()
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI rank = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attempts = listItem.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswaps = listItem.GetChild(3).GetComponent<TextMeshProUGUI>();

        rank.text = "--------";
        time.text = "--------";
        attempts.text = "----";
        timeswaps.text = "----";
    }
}

[Serializable]
struct NewScore
{
    public string name;
    public float time;
    public int attempts;
    public int timeswaps;

    public NewScore(string name, float time, int attempts, int timeswaps)
    {
        this.name = name;
        this.time = time;
        this.attempts = attempts;
        this.timeswaps = timeswaps;
    }
}

[Serializable]
struct Scores
{
    public string id;
    public List<ListEntry> top;
    public List<ListEntry> near;
}

[Serializable]
struct ListEntry
{
    public string _id;
    public string name;
    public int time;
    public int attempts;
    public int timeswaps;
    public int rank;
}
