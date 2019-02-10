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
    [SerializeField]
    private Vector2 startPoint2;
    [SerializeField]
    private Vector2 direction2;
    [SerializeField]
    private bool startPoint2Set;
    [SerializeField]
    private int curPoint = 0;

    private List<DotTail> lines = new List<DotTail>();
    private DotTail startPointLine = null;
    private DotTail startPoint2Line = null;

    public Vector2 StartPoint { get { return startPoint; } }
    public Vector2 Direction { get { return direction; } }
    public bool StartPointSet { get { return startPointSet; } }
    public Vector2 StartPoint2 { get { return startPoint2; } }
    public Vector2 Direction2 { get { return direction2; } }
    public bool StartPoint2Set { get { return startPoint2Set; } }

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

        DestroyStartPointLine(ref startPointLine);
        DestroyStartPointLine(ref startPoint2Line);
        startPointSet = false;
        startPoint2Set = false;
    }

    public void Save()
    {
        if(startPointLine != null)
        {
            startPointLine.gameObject.SetActive(false);
        }
        if (startPoint2Line != null)
        {
            startPoint2Line.gameObject.SetActive(false);
        }
        Object prefab = PrefabUtility.CreateEmptyPrefab(ASSETS_PATH + boardName + ".prefab");
        PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curPoint = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curPoint = 2;
        }
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
            if(curPoint == 1)
            {
                SetStartPointBeg(GetMousePos(), ref startPoint);
            }
            if (curPoint == 2)
            {
                SetStartPointBeg(GetMousePos(), ref startPoint2);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (curPoint == 1)
            {
                SetStartPointEnd(ref startPointLine, GetMousePos(), startPoint, ref direction, ref startPointSet);
            }
            if (curPoint == 2)
            {
                SetStartPointEnd(ref startPoint2Line, GetMousePos(), startPoint2, ref direction2, ref startPoint2Set);
            }
        }
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void CreateNewLine()
    {
        if(lines.Count > 0 && lines.Last().PointsCount == 0)
        {
            return;
        }

        lines.Add(Instantiate(linePrefab, transform));
        lines.Last().Init(lineColor);
    }

    private void AddPoint(Vector3 point)
    {
        lines.Last().AddPoint(point);
    }

    private void SetStartPointBeg(Vector2 point, ref Vector2 startPoint)
    {
        startPoint = point;
    }

    private void SetStartPointEnd(ref DotTail line, Vector2 point, Vector2 startPoint, ref Vector2 direction, ref bool startPointSet)
    {
        direction = (point - startPoint).normalized;
        CreateStartPointLine(ref line, startPoint, direction, ref startPointSet);
    }

    private void DestroyStartPointLine(ref DotTail line)
    {
        if (line != null)
        {
            Destroy(line.gameObject);
        }
        line = null;
    }

    private void CreateStartPointLine(ref DotTail line, Vector2 startPoint, Vector2 direction, ref bool startPointSet)
    {
        DestroyStartPointLine(ref line);
        line = Instantiate(linePrefab, transform);
        line.Init(startPointColorBeg);
        line.SetEndColor(startPointColorEnd);
        line.AddPoint(startPoint);
        line.AddPoint(startPoint + direction);
        startPointSet = true;
    }

    private void MakeVertSymmetry(ref Vector2 vec)
    {
        vec.x = -vec.x;
    }

    public void MakeVertSymmetry()
    {
        DotTail[] lines = GetComponentsInChildren<DotTail>();
        foreach(var line in lines)
        {
            line.MakeVertSymmetry();
        }

        MakeVertSymmetry(ref startPoint);
        MakeVertSymmetry(ref startPoint2);
        MakeVertSymmetry(ref direction);
        MakeVertSymmetry(ref direction2);
    }

}
