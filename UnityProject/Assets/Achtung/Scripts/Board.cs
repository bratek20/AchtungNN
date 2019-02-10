using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField]
    private bool autoReset = true;
    [SerializeField]
    private float size = 10f;
    [SerializeField]
    private float aspectRatio = 1.3f;
    [SerializeField]
    private bool loadBuiltBoard = false;
    [SerializeField]
    private DotTail wallPrefab;
    [SerializeField]
    private DotHead[] dots;
    [SerializeField]
    private Transform background;

    private DotTail[] walls = null;
    private BoardBuilder builder = null;
    private List<BoardBuilder> builderPrefabs = new List<BoardBuilder>();
    private int curPrefabIdx = 0;

    public int CountAllive { get; private set; }
    public float MaxDist { get; private set; }

    private void Start()
    {
        //if (loadBuiltBoard)
        //{
        //    InitBuilderPrefabs();
        //}
        if(autoReset)
        {
            Init();
        }
    }

    private void SetWall(int idx, float x1, float y1, float x2, float y2)
    {
        walls[idx].Init(Color.black);
        walls[idx].MakeLine(new Vector2(x1, y1), new Vector2(x2, y2));
    }

    public void Init(BoardBuilder builderPrefab = null, bool makeSymmetry = false)
    {
        InitWalls();

        Camera.main.orthographicSize = size;
        float heigth = 2 * size;
        float width = aspectRatio * heigth;
        background.localScale = new Vector3(width, heigth, 1);
        background.position = new Vector3(0, 0, 1);
        MaxDist = Vector2.Distance(Vector2.zero, background.localScale);

        float halfW = width / 2;
        float halfH = heigth / 2;

        SetWall(0, -halfW, halfH, halfW, halfH);
        SetWall(1, -halfW, -halfH, halfW, -halfH);
        SetWall(2, halfW, halfH, halfW, -halfH);
        SetWall(3, -halfW, halfH, -halfW, -halfH);

        foreach (var dot in dots)
        {
            dot.Init(Random.insideUnitCircle * size * 0.8f);
        }
        CountAllive = dots.Length;

        if(builderPrefab != null)
        {
            LoadBuiltBoard(builderPrefab, makeSymmetry);
            if(dots.Length > 0 && builder.StartPointSet)
            {
                dots[0].Init(builder.StartPoint, builder.Direction);
            }
            if (dots.Length > 1 && builder.StartPoint2Set)
            {
                dots[1].Init(builder.StartPoint2, builder.Direction2);
            }
        }
    }

    private void InitBuilderPrefabs()
    {
        DirectoryInfo dir = new DirectoryInfo(BoardBuilder.ASSETS_PATH);
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info)
        {
            string path = BoardBuilder.SHORT_PATH + f.Name.Substring(0, f.Name.Length - 7);
            BoardBuilder prefab = Resources.Load<BoardBuilder>(path);
            builderPrefabs.Add(prefab);
        }
    }

    private void LoadBuiltBoard(BoardBuilder builderPrefab, bool makeSymmetry)
    {
        if(builder != null)
        {
            Destroy(builder.gameObject);
        }

        builder = Instantiate(builderPrefab, transform);
        if (makeSymmetry)
        {
            builder.MakeVertSymmetry();
        }
        Debug.Log("Loaded board: " + builder.name + ", sym: " + makeSymmetry);
    }

    private BoardBuilder GetRandomBuilderPrefab()
    {
        int idx = Random.Range(0, builderPrefabs.Count);
        return builderPrefabs[idx];
    }

    private BoardBuilder GetNextBuilderPrefab()
    {
        BoardBuilder prefab = builderPrefabs[curPrefabIdx];
        curPrefabIdx = (curPrefabIdx + 1) % builderPrefabs.Count;
        return prefab;
    }

    private void InitWalls()
    {
        if(walls != null)
        {
            return;
        }

        walls = new DotTail[4];
        for (int i = 0; i < 4; i++)
        {
            walls[i] = Instantiate(wallPrefab, transform);
        }
    }

    private void Update()
    {
        int countAllive = 0; 
        foreach(var dot in dots)
        {
            if (!dot.Killed)
                countAllive += 1;
        }

        CountAllive = countAllive;
        if(countAllive == 0 && autoReset)
        {
            Init();
        }
    }
}
