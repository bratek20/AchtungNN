using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField]
    private bool autoReset = true;
    [SerializeField]
    private float size = 5f;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private DotHead[] dots;
    [SerializeField]
    private Transform background;

    private GameObject[] walls;

    public int CountAllive { get; private set; }

    private void Start()
    {
        walls = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            walls[i] = Instantiate(wallPrefab, transform);
        }

        Init();
    }

    private void SetWall(int idx, float x, float y, float scaleX, float scaleY)
    {
        Vector2 size = wallPrefab.GetComponent<BoxCollider2D>().size;
        walls[idx].transform.position = new Vector3(x, y, 0);
        walls[idx].transform.localScale = new Vector3(scaleX / size.x, scaleY / size.y, 1);
    }

    public void Init()
    {
        Camera.main.orthographicSize = size;
        float heigth = 2 * size;
        float width = Camera.main.aspect* heigth;
        background.localScale = new Vector3(width, heigth, 1);

        float wallSize = 1;
        SetWall(0, -width / 2, 0, wallSize, heigth);
        SetWall(1, width / 2, 0, wallSize, heigth);
        SetWall(2, 0, -heigth / 2, width, wallSize);
        SetWall(3, 0, heigth / 2, width, wallSize);

        foreach (var dot in dots)
        {
            dot.Init(Random.insideUnitCircle * size * 0.8f);
        }
        CountAllive = dots.Length;
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
