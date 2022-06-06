using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class ConwayOfLife : MonoBehaviour
{

    public int delay = 500;
    public int size = 100;
    public Material manitor;

    //LOGIC
    public bool[,] cells;
    private Texture2D texture;
    Thread world;
    bool active = false;
    bool show;

    private void Start()
    {
        GenerateWorld(size, size);
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);
        texture = new Texture2D(xLength, yLength, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;
        ShowWorld();
    }

    private void Update()
    {
        if (show)
        {
            ShowWorld();
            show = false;
        }
    }

    public void StartConwayOfLife()
    {
        PauseLife();
        active = true;
        world = new Thread(NextGeneration);
        world.Start();
    }

    public void SetRandomWorld()
    {
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);
        for (int x = 0; x < xLength; x++)
            for (int y = 0; y < yLength; y++)
            {
                cells[x, y] = Random.Range(0, 2) == 0 ? true : false;
            }

        ShowWorld();
    }

    public void PauseLife()
    {
        active = false;
        if (world != null)
            world.Abort();
    }

   

    public void GenerateWorld(int x, int y)
    {
        cells = new bool[x, y];
    }

    void NextGeneration()
    {
        cells = Calculate();
        show = true;
        print("t");
        Thread.Sleep(delay);
        if (active)
            NextGeneration();
    }

    bool GetCell(int x, int y)
    {
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);
        if (x < 0 || x >= xLength || y < 0 || y >= yLength)
            return false;
        return cells[x, y];
    }
    public void SetCell(bool value, int x, int y)
    {
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);
        if (!(x < 0 || x >= xLength || y < 0 || y >= yLength))
            cells[x, y] = value;
        ShowWorld();
    }

    bool[,] Calculate()
    {
        //make a new cells
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);
        var newCells = new bool[xLength, yLength];

        //calculate all cells
        ////functions
        bool CalculateSingleCell(int x, int y)
        {
            bool cell = GetCell(x, y);
            int count = 0;
            for (int xOffset = -1; xOffset <= 1; xOffset++)
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    if (xOffset == 0 && yOffset == 0)
                        continue;
                    if (GetCell(x + xOffset, y + yOffset))
                        count++;
                }
            if (cell)
            {
                if (count < 2)
                    return false;
                if (count > 3)
                    return false;
                return true;
            }
            else
            {
                if (count == 3)
                    return true;
            }
            return false;
        }

        for (int x = 0; x < xLength; x++)
            for (int y = 0; y < yLength; y++)
            {
                newCells[x, y] = CalculateSingleCell(x, y);
            }

        //next generation
        return newCells;
    }

    void ShowWorld()
    {
        var xLength = cells.GetLength(0);
        var yLength = cells.GetLength(1);


        for (int x = 0; x < xLength; x++)
            for (int y = 0; y < yLength; y++)
            {
                texture.SetPixel(x, y, cells[x, y] ? Color.black : Color.white);
            }
        texture.Apply();
        manitor.mainTexture = texture;
    }

    private void OnApplicationPause(bool pause)
    {
        PauseLife();
    }

    private void OnDisable()
    {
        PauseLife();
    }

}
