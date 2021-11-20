using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutObstacle : MonoBehaviour
{
    public CuttingStartPoint Point { get; private set; }

    private void Awake()
    {
        Point = GetComponentInChildren<CuttingStartPoint>();
    }
}
