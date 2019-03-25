using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public GameObject lineSource;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //update the start of the line renderer to the source's position
        _lineRenderer.SetPosition(0, lineSource.transform.position);
    }
}
