using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BoardBuilder : MonoBehaviour
{
    public static readonly string SHORT_PATH = "Boards/";
    public static readonly string ASSETS_PATH = "Assets/Achtung/Resources/" + SHORT_PATH;
   
    [SerializeField]
    private DotTail linePrefab;
    [SerializeField]
    private Color lineColor;
    [SerializeField]
    private Color startPointColorBeg;
    [SerializeField]
    private Color startPointColorEnd;
    [SerializeField]
    private string boardName; 
    [SerializeField]
    private Vector2 startPoint;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private bool startPointSet;

    private List<DotTail> lines = new List<DotTail>();
    private DotTail startPointLine = null;
    
    public Vector2 StartPoint { get { return startPoint; } private set { startPoint = value; } }
    public Vector2 Direction { get { return direction; } private set { direction = value; } }
    public bool StartPointSet { get { return startPointSet; } private set { startPointSet = value; } }

    public void Clear()
    {
        foreach (var line in lines)
        {
            if (line.gameObject != null)
            {
                Destroy(line.gameObject);
            }
        }
        lines.Clear();

        DestroyStartPointLine();
        StartPointSet = false;
    }

    public void Save()
    {
        if(startPointLine != null)
        {
            startPointLine.gameObject.SetActive(false);
        }
        Object prefab = PrefabUtility.CreateEmptyPrefab(ASSETS_PATH + boardName + ".prefab");
        PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();           
        }
        if(Input.GetMouseButton(0))
        {
            AddPoint(GetMousePos());
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetStartPointBeg(GetMousePos());
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetStartPointEnd(GetMousePos());
        }
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void CreateNewLine()
    {
        lines.Add(Instantiate(linePrefab, transform));
        lines.Last().Init(lineColor);
    }

    private void AddPoint(Vector3 point)
    {
        lines.Last().AddPoint(point);
    }

    private void SetStartPointBeg(Vector2 point)
    {
        StartPoint = point;
    }

    private void SetStartPointEnd(Vector2 point)
    {
        Direction = (point - StartPoint).normalized;
        CreateStartPointLine();
    }

    private void DestroyStartPointLine()
    {
        if (startPointLine != null)
        {
            Destroy(startPointLine.gameObject);
        }
        startPointLine = null;
    }

    private void CreateStartPointLine()
    {
        DestroyStartPointLine();
        startPointLine = Instantiate(linePrefab, transform);
        startPointLine.Init(startPointColorBeg);
        startPointLine.SetEndColor(startPointColorEnd);
        startPointLine.AddPoint(StartPoint);
        startPointLine.AddPoint(StartPoint + Direction);
        StartPointSet = true;
    }

}
