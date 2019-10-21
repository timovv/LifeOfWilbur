using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Submits user's scores and grabs leaderboard data
/// </summary>
public class ScoreboardInit : MonoBehaviour
{
    /// <summary>
    /// Scoreboard base container
    /// </summary>
    public Transform _container;

    /// <summary>
    /// Scoreboard entry/list prefab template
    /// </summary>
    public Transform _entryTemplate;

    /// <summary>
    /// Colour to highlight user's entry in leaderboard list
    /// </summary>
    public Color _userHighlight = new Color(1f, 0.878f, 0.212f);

    /// <summary>
    /// API endpoint to send score and get leaderboard data.
    /// </summary>
    public string _saveScoreEndPoint = "http://localhost:3000/scores";

    void Awake()
    {
        // Get stats from the various controllers
        NewScore score = new NewScore {
            name = MainMenu.PlayerName,
            time = GameTimer.ElapsedTimeSeconds,
            attempts = GameController.Resets + 1,
            timeswaps = TimeTravelController.Timeswaps
        };
        StartCoroutine(PostScore(score));
    }

    /// <summary>
    /// Sends scores to the defined API endpoing
    /// </summary>
    private IEnumerator PostScore(NewScore score)
    {
        // Had to use a custom raw upload handler as defalut escapes certain characters
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

        transform.Find("Disconnected").gameObject.SetActive(false);

        Scores recieved = JsonUtility.FromJson<Scores>(request.downloadHandler.text);
        FillList(recieved);
    }

    /// <summary>
    /// Adds entries from the API call to the scoreboard list
    /// </summary>
    private void FillList(Scores data)
    {
        // Always show top 10
        foreach (ListEntry entry in data.top)
        {
            AddListItem(entry, entry._id == data.id);
        }

        // We want to add a spacer if scores around the user's rank
        // isn't top 20
        int lastTopRank = data.top.Count;
        if (lastTopRank < data.near[0].rank)
        {
            AddSpacer();
        }

        // Entries that are +/- 5 ranks of the user's rank
        foreach (ListEntry entry in data.near)
        {
            if (lastTopRank < entry.rank)
            {
                AddListItem(entry, entry._id == data.id);
            }
        }
    }

    /// <summary>
    /// Instantiates a new scoreboard list entry
    /// </summary>
    private void AddListItem(ListEntry entry, bool shouldHighlight)
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI rank = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI name = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attempts = listItem.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswaps = listItem.GetChild(4).GetComponent<TextMeshProUGUI>();

        rank.text = String.Format("#{0:D6}", entry.rank);
        name.text = entry.name;
        time.text = GameTimer.FormatTime(entry.time);
        attempts.text = entry.attempts.ToString();
        timeswaps.text = entry.timeswaps.ToString();

        if (shouldHighlight)
        {
            rank.color = _userHighlight;
            name.color = _userHighlight;
            time.color = _userHighlight;
            attempts.color = _userHighlight;
            timeswaps.color = _userHighlight;
        }
    }

    /// <summary>
    /// Instantiates a spacer scoreboard entry
    /// </summary>
    private void AddSpacer()
    {
        Transform listItem = Instantiate<Transform>(_entryTemplate, _container);
        TextMeshProUGUI rank = listItem.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI name = listItem.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI time = listItem.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attempts = listItem.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswaps = listItem.GetChild(4).GetComponent<TextMeshProUGUI>();

        rank.text = "--------";
        name.text = "--------";
        time.text = "--------";
        attempts.text = "----";
        timeswaps.text = "----";
    }
}

// DTOs for API
// Send
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

// Recieve
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
    public float time;
    public int attempts;
    public int timeswaps;
    public int rank;
}
