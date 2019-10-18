using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphInit : MonoBehaviour
{
    public Transform _container;
    public Transform _graphTemplate;

    public string _getDataEndPoint = "http://localhost:3000/scores/graph";

    private void Awake()
    {
        
    }
}
