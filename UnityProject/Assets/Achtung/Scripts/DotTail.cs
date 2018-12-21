using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class DotTail : MonoBehaviour
{
    [SerializeField]
    private float minPointsDist = 0.15f;
    [SerializeField]
    private CircleCollider2D dotCol;

    private LineRenderer line;
    private EdgeCollider2D edges;
    private List<Vector2> points = new List<Vector2>();
    private Queue<Vector2> waitPoints = new Queue<Vector2>();

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        edges = GetComponent<EdgeCollider2D>();
    }

    public void Init(Color lineColor)
    {
        points.Clear();
        waitPoints.Clear();
        line.positionCount = 0;
        line.startColor = lineColor;
        line.endColor = lineColor;
        edges.points = points.ToArray();
    }

    public void AddPoint(Vector2 point)
    {
        waitPoints.Enqueue(point);
        while(waitPoints.Count > 0 && !dotCol.OverlapPoint(waitPoints.Peek()))
        {
            InternalAddPoint(waitPoints.Dequeue());
        }
    }

    private void InternalAddPoint(Vector2 point)
    {
        if (points.Count > 0 && Vector2.Distance(points.Last(), point) < minPointsDist)
        {
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
}
