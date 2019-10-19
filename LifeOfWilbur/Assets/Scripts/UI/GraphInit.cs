using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class GraphInit : MonoBehaviour
{
    public RectTransform _timeContianer;
    public RectTransform _attemptsContianer;
    public RectTransform _timeswapContianer;

    public RectTransform _tooltip;

    public string _graphEndPoint = "http://localhost:3000/scores/graph";

    private void Awake()
    {
        float time = GameTimer.ElapsedTimeSeconds;
        int attempts = GameController.Resets;
        int timeswaps = TimeTravelController.Timeswaps;

        StartCoroutine(GetGraphData("time", _timeContianer, time));
        StartCoroutine(GetGraphData("attempts", _attemptsContianer, attempts));
        StartCoroutine(GetGraphData("timeswaps", _timeswapContianer, timeswaps));

        TextMeshProUGUI timeValue = _timeContianer.Find("Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attemptValue = _attemptsContianer.Find("Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeswapValue = _timeswapContianer.Find("Value").GetComponent<TextMeshProUGUI>();

        timeValue.text = GameTimer.FormattedElapsedTime;
        attemptValue.text = attempts.ToString();
        timeswapValue.text = timeswaps.ToString();
    }

    private IEnumerator GetGraphData(string field, RectTransform container, float value)
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

    private void FillGraph(GraphBars barData, RectTransform container, float value)
    {
        RectTransform histogram = (RectTransform)container.Find("Histogram");
        RectTransform line = (RectTransform)container.Find("Line");
        float horizontalPercentage = value / barData.max;

        Vector3 linePos = line.position;
        linePos.x += histogram.sizeDelta.x * horizontalPercentage;
        line.position = linePos;

        float highestPercentage = 0;
        foreach(Bar bar in barData.bars)
        {
            highestPercentage = Math.Max(highestPercentage, bar.percentage);
        }

        for (int i = 0; i < histogram.childCount; i++)
        {
            RectTransform bg = (RectTransform)histogram.GetChild(i);
            RectTransform fill = (RectTransform)bg.GetChild(0);

            Bar data = barData.bars[i];
            float yPos = bg.sizeDelta.y * 0.8f * data.percentage / highestPercentage;
            fill.sizeDelta = new Vector2(fill.sizeDelta.x, yPos);
            string tooltipText;
            if (container.name == "Time")
            {
                tooltipText = string.Format("{0} - {1}", GameTimer.FormatTime(data.range[0]), GameTimer.FormatTime(data.range[1]));
            }
            else
            {
                tooltipText = string.Format("{0} - {1}", data.range[0], data.range[1]);
            }
            bg.GetComponent<BarTooltip>().Text = tooltipText;
        }
    }

    public void Update()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);                            // This section prepares a list for all objects hit with the raycast
        eventData.position = Input.mousePosition;
        List<RaycastResult> uiHit = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, uiHit);

        if(uiHit.Count > 0)
        {
            GameObject target = uiHit[0].gameObject;
            if (target.name.StartsWith("Bar"))
            {
                _tooltip.position = target.transform.position;
                _tooltip.GetComponent<TextMeshProUGUI>().text = target.GetComponent<BarTooltip>().Text;
            }
        }
        else
        {
                _tooltip.position = Vector3.up * 10;
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
