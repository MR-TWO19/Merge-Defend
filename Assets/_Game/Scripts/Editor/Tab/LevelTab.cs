using System.Collections.Generic;
using TwoCore;
using UnityEditor;
using UnityEngine;

public class LevelTab : TabContent
{
    private LevelConfig levelConfig;
    private Vector2 scrollPos;

    private List<bool> levelFoldouts = new List<bool>();

    private Dictionary<int, Vector2Int> selectedCells = new Dictionary<int, Vector2Int>();

    public LevelTab()
    {
        levelConfig = LevelConfig.Ins;
    }

    public override void DoDraw()
    {
        if (levelConfig == null)
        {
            EditorGUILayout.HelpBox("Không tìm thấy LevelConfig!", MessageType.Error);
            return;
        }

        Draw.Space(10);
        Draw.Label("LEVEL CONFIGURATION", EditorStyles.boldLabel);

        Draw.BeginHorizontal();

        // Nút thêm level
        if (Draw.Button("Thêm Level", Color.green, 150))
        {
            var newLevel = new LevelData
            {
                SizeGrid = 6,
                ItemControlLevelDatas = new List<ItemControlLevelData>()
            };

            levelConfig.levelDatas.Add(newLevel);
            levelFoldouts.Add(true);
        }
        Draw.EndHorizontal();
        Draw.Space();

        scrollPos = Draw.BeginScrollView(scrollPos);

        for (int i = 0; i < levelConfig.levelDatas.Count; i++)
        {
            if (i >= levelFoldouts.Count)
                levelFoldouts.Add(false);

            var level = levelConfig.levelDatas[i];

            Draw.BeginVertical("box", GUILayout.MaxWidth(1000));
            Draw.BeginHorizontal();
            levelFoldouts[i] = Draw.BeginFoldoutGroup(levelFoldouts[i], $"Level {i + 1}");

            if (Draw.Button("X", Color.red, Color.white, 40))
            {
                levelConfig.levelDatas.RemoveAt(i);
                levelFoldouts.RemoveAt(i);
                Draw.EndHorizontal();
                Draw.EndVertical();
                break;
            }

            Draw.EndFoldoutGroup();
            Draw.EndHorizontal();

            if (levelFoldouts[i])
            {
                EditorGUI.indentLevel++;

                level.SizeGrid = Draw.IntField("Size Grid:", level.SizeGrid, 200);

                DrawItemControlGrid(level, i);

                EditorGUI.indentLevel--;
            }

            Draw.EndVertical();
            Draw.Space();
        }

        Draw.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelConfig);
        }
    }

    private void DrawItemControlGrid(LevelData level, int levelIndex)
    {
        int gridSize = Mathf.Max(1, level.SizeGrid);

        if (!selectedCells.ContainsKey(levelIndex))
            selectedCells[levelIndex] = new Vector2Int(-1, -1);

        Vector2Int selectedCell = selectedCells[levelIndex];

        Draw.Space(5);
        Draw.BeginHorizontal();

        // Grid display
        Draw.BeginVertical(GUILayout.Width(300));
        for (int y = gridSize - 1; y >= 0; y--)
        {
            Draw.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int cellPos = new Vector2Int(x, y);
                var existingItem = level.ItemControlLevelDatas.Find(c => c.Cell == cellPos);

                Color oldColor = GUI.backgroundColor;

                if (selectedCell == cellPos)
                    GUI.backgroundColor = Color.blue;
                else if (existingItem != null)
                    GUI.backgroundColor = Color.green;
                else
                    GUI.backgroundColor = Color.gray;

                if (GUILayout.Button($"{x},{y}", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    if (existingItem == null)
                    {
                        var newItem = new ItemControlLevelData
                        {
                            Cell = cellPos
                        };
                        level.ItemControlLevelDatas.Add(newItem);
                    }

                    selectedCell = cellPos;
                    selectedCells[levelIndex] = selectedCell;
                }

                GUI.backgroundColor = oldColor;
            }
            Draw.EndHorizontal();
        }
        Draw.EndVertical();

        // Right panel: only show basic info (Cell) and delete
        Draw.Space(20);
        Draw.BeginVertical("box", GUILayout.Width(300));

        var selectedItemData = level.ItemControlLevelDatas.Find(c => c.Cell == selectedCell);

        if (selectedItemData != null)
        {
            Draw.LabelBoldBox($"Cell: ({selectedItemData.Cell.x}, {selectedItemData.Cell.y})", Color.cyan);
            if (Draw.Button("Xóa Cell", Color.red, 100))
            {
                level.ItemControlLevelDatas.Remove(selectedItemData);
                selectedCells[levelIndex] = new Vector2Int(-1, -1);
            }
        }
        else
        {
            Draw.LabelBoldBox("Nhấp vào 1 ô trong grid để chỉnh ItemControl", Color.gray);
        }

        Draw.EndVertical();
        Draw.EndHorizontal();
    }
}
