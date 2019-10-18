using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GraphInit : MonoBehaviour
{
    public RectTransform _timeContianer;
    public RectTransform _attemptsContianer;
    public RectTransform _timeswapContianer;

    public string _graphEndPoint = "http://localhost:3000/scores/graph";

    private void Awake()
    {
        int time = 0;
        int attempts = 0;
        int timeswaps = 0;

        StartCoroutine(GetGraphData("time", _timeContianer, time));
        StartCoroutine(GetGraphData("attempts", _attemptsContianer, attempts));
        StartCoroutine(GetGraphData("timeswaps", _timeswapContianer, timeswaps));

        TextMeshProUGUI timeValue = _timeContianer.Find("Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attemptValue = _attemptsContianer.Find("Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswapValue = _timeswapContianer.Find("Value").GetComponent<TextMeshProUGUI>();

        timeValue.text = "12:23:34";
        attemptValue.text = "123";
        timeswapValue.text = "456";
    }

    private IEnumerator GetGraphData(string field, RectTransform container, int value)
    {
        UnityWebRequest request = UnityWebRequest.Get(_graphEndPoint + "/" + field);
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error getting graph: " + request.error);
            yield break;
        }

        GraphBars recieved = JsonUtility.FromJson<GraphBars>(request.downloadHandler.text);
        FillGraph(recieved, container, value);
    }

    private void FillGraph(GraphBars barData, RectTransform container, int value)
    {
        RectTransform histogram = (RectTransform)container.Find("Histogram");
        RectTransform line = (RectTransform)container.Find("Line");
        float horizontalPercentage = value / barData.max;

        Vector3 linePos = line.position;
        linePos.x += histogram.sizeDelta.x * horizontalPercentage;
        line.position = linePos;

        for (int i = 0; i < histogram.childCount; i++)
        {
            RectTransform bar = (RectTransform)histogram.GetChild(i);;
            bar.sizeDelta = new Vector2(bar.sizeDelta.x, histogram.sizeDelta.y * barData.bars[i].percentage);
        }
    }
}

[Serializable]
struct GraphBars
{
    public Bar[] bars;
    public int max;
}

[Serializable]
struct Bar
{
    public int[] range;
    public int count;
    public float percentage;
}
