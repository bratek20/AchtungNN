﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotTail : MonoBehaviour
{
    [SerializeField]
    private float minPointsDist = 0.15f;
    [SerializeField]
    private float minGapSize = 5f;
    [SerializeField]
    private CircleCollider2D dotCol;
    [SerializeField]
    private GameObject linePartPrefab;

    private List<GameObject> lineParts = new List<GameObject>();
    private LineRenderer line;
    private EdgeCollider2D edges;
    private List<Vector2> points = new List<Vector2>();
    private Queue<Vector2> waitPoints = new Queue<Vector2>();
    private Color color;
    private bool makeGap = false;
    private float curGapSize = 0f;

    public int PointsCount { get { return points.Count; } }
    public DotHead Head { get { return dotCol != null ? dotCol.GetComponent<DotHead>() : null; } }
    public void Init(Color color)
    {
        this.color = color;

        foreach (var part in lineParts)
        {
            Destroy(part);
        }
        lineParts.Clear();

        makeGap = false;
        LoadNewPart();
    }

    private void LoadNewPart()
    {
        GameObject newPart = Instantiate(linePartPrefab, transform);
        lineParts.Add(newPart);
        line = newPart.GetComponent<LineRenderer>();
        edges = newPart.GetComponent<EdgeCollider2D>();
        line.startColor = color;
        line.endColor = color;
        points.Clear();
        waitPoints.Clear();
    }

    public int GetLinePartNum(GameObject line)
    {
        for(int i=lineParts.Count -1; i >= 0; i--)
        {
            if(lineParts[i] == line)
            {
                return lineParts.Count - i - 1;
            }
        }
        return lineParts.Count;
    }

    public void MakeGap()
    {
        makeGap = true;
        curGapSize = 0f;
    }

    public void AddPoint(Vector2 point)
    {
        waitPoints.Enqueue(point);
        while(waitPoints.Count > 0 && !CheckDotOverlap(waitPoints.Peek()))
        {
            InternalAddPoint(waitPoints.Dequeue());
        }
    }

    private bool CheckDotOverlap(Vector2 point)
    {
        if(dotCol == null)
        {
            return false;
        }
        return dotCol.OverlapPoint(point);
    }

    private void InternalAddPoint(Vector2 point)
    {
        float pointsDist = 0f;
        if (points.Count > 0)
        {
            pointsDist = Vector2.Distance(points.Last(), point);
            if(pointsDist < minPointsDist)
            {
                return;
            }
        }

        if(makeGap)
        {
            curGapSize += pointsDist;
            if(curGapSize > minGapSize)
            {
                makeGap = false;
                LoadNewPart();
            }
            return;
        }

        if (points.Count > 1)
        {
            edges.points = points.ToArray();
        }

        points.Add(point);
        line.positionCount = points.Count;
        line.SetPosition(points.Count - 1, point);
    }

    public void SetEndColor(Color c)
    {
        line.endColor = c;
    }

    public EdgeCollider2D GetEdgeCollider()
    {
        return edges;
    }

    public void MakeLine(Vector2 p1, Vector2 p2)
    {
        AddPoint(p1);
        AddPoint(p2);
        edges.points = points.ToArray();
    }

    public void MakeVertSymmetry()
    {
        line = GetComponentInChildren<LineRenderer>();
        edges = GetComponentInChildren<EdgeCollider2D>();
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);
        positions = positions.Select(p => new Vector3(-p.x, p.y, p.z)).ToArray();
        line.SetPositions(positions);
        edges.points = positions.Select(v => new Vector2(v.x, v.y)).ToArray();
    }


}
