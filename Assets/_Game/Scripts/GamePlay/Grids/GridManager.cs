using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GridAlignType
{
    Center,
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
    Custom
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;
    public GameObject cellPrefab;
    public float cellSize = 1f;
    public float cellSpacing = 0.1f;

    [Header("Position Settings")]
    public GridAlignType alignType = GridAlignType.Center;
    public Vector3 customOffset = Vector3.zero;

    private GridCell[,] gridArray;

    [Button("Generate Grid")]
    public void GenerateGrid()
    {
        ClearGrid();

        gridArray = new GridCell[width, height];

        Vector3 origin = Vector3.zero;

        float totalWidth = width * (cellSize + cellSpacing) - cellSpacing;
        float totalHeight = height * (cellSize + cellSpacing) - cellSpacing;

        switch (alignType)
        {
            case GridAlignType.Center:
                origin = new Vector3(-totalWidth / 2f + cellSize / 2f, 0, -totalHeight / 2f + cellSize / 2f);
                break;

            case GridAlignType.TopLeft:
                origin = new Vector3(0, 0, -totalHeight + cellSize / 2f);
                break;

            case GridAlignType.TopCenter:
                origin = new Vector3(-totalWidth / 2f + cellSize / 2f, 0, -totalHeight + cellSize / 2f);
                break;

            case GridAlignType.TopRight:
                origin = new Vector3(-totalWidth + cellSize / 2f, 0, -totalHeight + cellSize / 2f);
                break;

            case GridAlignType.MiddleLeft:
                origin = new Vector3(0, 0, -totalHeight / 2f + cellSize / 2f);
                break;

            case GridAlignType.MiddleRight:
                origin = new Vector3(-totalWidth + cellSize / 2f, 0, -totalHeight / 2f + cellSize / 2f);
                break;

            case GridAlignType.BottomLeft:
                origin = new Vector3(0, 0, 0);
                break;

            case GridAlignType.BottomCenter:
                origin = new Vector3(-totalWidth / 2f + cellSize / 2f, 0, 0);
                break;

            case GridAlignType.BottomRight:
                origin = new Vector3(-totalWidth + cellSize / 2f, 0, 0);
                break;

            case GridAlignType.Custom:
                origin = customOffset;
                break;
        }

        switch (alignType)
        {
            case GridAlignType.Center:
                origin = new Vector3(-totalWidth / 2f + cellSize / 2f, 0, -totalHeight / 2f + cellSize / 2f);
                break;

            case GridAlignType.BottomLeft:
                origin = Vector3.zero;
                break;

            case GridAlignType.Custom:
                origin = customOffset;
                break;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 localPos = new(
                    origin.x + x * (cellSize + cellSpacing),
                    0,
                    origin.z + y * (cellSize + cellSpacing)
                );

                GameObject cell = Instantiate(cellPrefab, transform);
                cell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                cell.transform.SetLocalPositionAndRotation(localPos, Quaternion.Euler(90, 0, 0));

                GridCell gridCell = cell.GetComponent<GridCell>();
                cell.name = $"Cell {x},{y}";
                gridArray[x, y] = gridCell;
            }
        }

        DOVirtual.DelayedCall(0.5f, () =>
        {
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    gridArray[i, j].UpdateUI();
                }
            }
        });

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    gridArray[i, j].UpdateUI();
                }
            }
        }
    }

    public void GenerateGrid(int size)
    {
        width = size;
        height = size;
        GenerateGrid();
    }

    [Button("Clear Grid")]
    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        gridArray = new GridCell[0, 0];
    }

    // ================================
    // ==== CÁC HÀM TIỆN ÍCH KHÁC ====
    // ================================

    public GridCell GetGridCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return gridArray[x, y];
        return null;
    }

    public List<GridCell> GetGridCells(List<Vector2Int> positions)
    {
        List<GridCell> list = new();
        foreach (var pos in positions)
        {
            if (IsInsideGrid(pos.x, pos.y))
                list.Add(gridArray[pos.x, pos.y]);
        }
        return list;
    }

    public Vector3 GetCellPosition(int x, int y)
    {
        if (IsInsideGrid(x, y))
            return gridArray[x, y].transform.position;
        return Vector3.zero;
    }

    public bool IsInsideGrid(int x, int y) => (x >= 0 && x < width && y >= 0 && y < height);

    public bool IsOccupied(int x, int y)
    {
        if (!IsInsideGrid(x, y)) return true;
        return gridArray[x, y].Occupied;
    }

    public void SetColorCell(int x, int y, Color color)
    {
        if (IsInsideGrid(x, y))
            gridArray[x, y].SetColorCell(color);
    }

    public void ResetAllCells()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                gridArray[x, y].ResetCell();
    }
}
