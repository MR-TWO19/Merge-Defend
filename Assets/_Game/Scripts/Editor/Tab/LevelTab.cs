using System.Collections.Generic;
using System.Linq;
using Hawky.Inventory;
using TwoCore;
using UnityEditor;
using UnityEngine;

public class LevelTab : TabContent
{
    private LevelConfig levelConfig;
    private Vector2 scrollPos;

    private List<bool> levelFoldouts = new List<bool>();

    private Dictionary<int, bool[]> subFoldouts = new Dictionary<int, bool[]>();

    private Dictionary<int, List<bool>> lineFoldouts = new Dictionary<int, List<bool>>();

    private Dictionary<int, Vector2Int> selectedCells = new Dictionary<int, Vector2Int>();

    private Dictionary<LineSpawnData, List<bool>> _lineSpawnFoldouts = new();

    private bool isCreateLineSpawn;

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
                ItemControlLevelDatas = new List<ItemControlLevelData>(),
                LineSpawnDatas = new List<LineSpawnData>(),
                Lines = new List<LineLevelData>()
            };

            levelConfig.levelDatas.Add(newLevel);
            levelFoldouts.Add(true);
            subFoldouts[levelConfig.levelDatas.Count - 1] = new bool[] { true, false };
        }
        Draw.Space(10);

        isCreateLineSpawn = Draw.ToggleField("Create Line Spawn", isCreateLineSpawn);

        Draw.EndHorizontal();
        Draw.Space();

        scrollPos = Draw.BeginScrollView(scrollPos);

        for (int i = 0; i < levelConfig.levelDatas.Count; i++)
        {
            if (i >= levelFoldouts.Count)
                levelFoldouts.Add(false);
            if (!subFoldouts.ContainsKey(i))
                subFoldouts[i] = new bool[] { false, false };

            var level = levelConfig.levelDatas[i];

            Draw.BeginVertical("box", GUILayout.MaxWidth(1000));
            Draw.BeginHorizontal();
            levelFoldouts[i] = Draw.BeginFoldoutGroup(levelFoldouts[i], $"Level {i + 1}");


            if (Draw.Button("X", Color.red, Color.white, 40))
            {
                levelConfig.levelDatas.RemoveAt(i);
                levelFoldouts.RemoveAt(i);
                subFoldouts.Remove(i);
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

                // ===================== ITEM CONTROL GRID =====================
                subFoldouts[i][0] = Draw.BeginFoldoutGroup(subFoldouts[i][0],
                    $"ItemControl Grid ({level.SizeGrid}x{level.SizeGrid})", Color.cyan);

                if (subFoldouts[i][0])
                    DrawItemControlGrid(level, i);

                Draw.EndFoldoutGroup();
                Draw.Space();

                // ===================== LINES =====================
                subFoldouts[i][1] = Draw.BeginFoldoutGroup(subFoldouts[i][1],
                    $"Lines ({level.Lines.Count})", Color.cyan);

                if (subFoldouts[i][1])
                    DrawLineList(level, i);

                Draw.EndFoldoutGroup();

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

    //====================== ITEM CONTROL GRID ======================

    private void DrawItemControlGrid(LevelData level, int levelIndex)
    {
        int gridSize = level.SizeGrid;

        if (!selectedCells.ContainsKey(levelIndex))
            selectedCells[levelIndex] = new Vector2Int(-1, -1);

        Vector2Int selectedCell = selectedCells[levelIndex];

        Draw.Space(5);
        Draw.BeginHorizontal();

        // ================= GRID HIỂN THỊ =================
        Draw.BeginVertical(GUILayout.Width(300));
        for (int y = gridSize - 1; y >= 0; y--)
        {
            Draw.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int cellPos = new Vector2Int(x, y);
                var existingItem = level.ItemControlLevelDatas.Find(c => c.Cell == cellPos);
                var existingLineSpawn = level.LineSpawnDatas.Find(c => c.CellLineSpawn == cellPos);

                Color oldColor = GUI.backgroundColor;

                // Màu của ô
                if (selectedCell == cellPos)
                    GUI.backgroundColor = Color.blue;
                else if (existingItem != null)
                {
                    if (existingItem.EventType == ItemEventType.Lock)
                        GUI.backgroundColor = Color.yellow;
                    else if (existingItem.KeepTheKey)
                        GUI.backgroundColor = new Color(1f, 0.5f, 0f);
                    else if (existingItem.EventType == ItemEventType.Hint)
                        GUI.backgroundColor = new Color(0.4f, 0f, 0.6f);
                    else
                        GUI.backgroundColor = Color.green;
                }    
                else if (existingLineSpawn != null)
                    GUI.backgroundColor = Color.red;
                else
                    GUI.backgroundColor = Color.gray;

                // Button grid
                if (GUILayout.Button($"{x},{y}", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    // Nếu đang bật chế độ tạo LineSpawn
                    if (isCreateLineSpawn)
                    {
                        if (existingItem == null && existingLineSpawn == null)
                        {
                            var newLineSpawn = new LineSpawnData
                            {
                                CellLineSpawn = cellPos,
                                ItemControlLevelDatas = new List<ItemControlLevelData>()
                            };
                            level.LineSpawnDatas.Add(newLineSpawn);
                        }
                    }
                    // Nếu không bật chế độ LineSpawn thêm ItemControl
                    else
                    {
                        if (existingItem == null && existingLineSpawn == null)
                        {
                            var newItem = new ItemControlLevelData
                            {
                                Cell = cellPos,
                                ItemIds = new List<int>()
                            };

                            for (int i = 0; i < 4; i++)
                            {
                                newItem.ItemIds.Add(0);
                            }
                            level.ItemControlLevelDatas.Add(newItem);
                        }
                    }

                    selectedCell = cellPos;
                    selectedCells[levelIndex] = selectedCell;
                }

                GUI.backgroundColor = oldColor;
            }
            Draw.EndHorizontal();
        }
        Draw.EndVertical();

        // ================= PANEL THÔNG TIN BÊN PHẢI =================
        Draw.Space(20);
        Draw.BeginVertical("box", GUILayout.Width(300));

        var selectedItemData = level.ItemControlLevelDatas.Find(c => c.Cell == selectedCell);
        var selectedLineSpawnData = level.LineSpawnDatas.Find(c => c.CellLineSpawn == selectedCell);

        if (selectedItemData != null)
        {
            Draw.LabelBoldBox($"Cell: ({selectedItemData.Cell.x}, {selectedItemData.Cell.y})", Color.cyan);
            if (Draw.Button("Xóa Cell", Color.red, 100))
            {
                level.ItemControlLevelDatas.Remove(selectedItemData);
                selectedCells[levelIndex] = new Vector2Int(-1, -1);
            }

            Draw.Space(10);

            selectedItemData.EventType = Draw.EnumField("Event Type", selectedItemData.EventType);

            if (selectedItemData.EventType != ItemEventType.Lock)
            {
                selectedItemData.KeepTheKey = Draw.ToggleField("Keep The Key", selectedItemData.KeepTheKey);
            }

            if (selectedItemData.EventType == ItemEventType.Lock || selectedItemData.KeepTheKey)
            {
                selectedItemData.LockId = Draw.IntPopupField("Lock Id", selectedItemData.LockId, GameConfig.Ins.LockDatas, "name", "id" , 200);
            }

            Draw.Space(10);
            EditorGUILayout.LabelField("ItemIds:");

            for (int i = 0; i < selectedItemData.ItemIds.Count; i++)
            {
                Draw.BeginHorizontal();
                selectedItemData.ItemIds[i] = Draw.IntPopupField(
                    $"ItemId:",
                    selectedItemData.ItemIds[i],
                    GameConfig.Ins.ItemDatas,
                    "name", "id", 200
                );

                if (Draw.Button("-", Color.red, 30))
                {
                    selectedItemData.ItemIds.RemoveAt(i);
                    break;
                }

                Draw.EndHorizontal();
            }

            if (Draw.Button("+", Color.green, 30))
                selectedItemData.ItemIds.Add(0);
        }
        else if (selectedLineSpawnData != null)
        {
            Draw.LabelBoldBox( $"Line Spawn: ({selectedLineSpawnData.CellLineSpawn.x}, {selectedLineSpawnData.CellLineSpawn.y})", Color.black);

            if (Draw.Button("Xóa Cell", Color.red, 100))
            {
                level.LineSpawnDatas.Remove(selectedLineSpawnData);
                selectedCells[levelIndex] = new Vector2Int(-1, -1);
                return;
            }

            Draw.Space(10);

            selectedLineSpawnData.CellSpawn = Draw.Vector2IntField("Cell Spawn", selectedLineSpawnData.CellSpawn);

            Draw.Space(10);
            EditorGUILayout.LabelField("ItemControls:", EditorStyles.boldLabel);

            if (!_lineSpawnFoldouts.ContainsKey(selectedLineSpawnData))
                _lineSpawnFoldouts[selectedLineSpawnData] = new List<bool>();

            var foldouts = _lineSpawnFoldouts[selectedLineSpawnData];

            for (int i = 0; i < selectedLineSpawnData.ItemControlLevelDatas.Count; i++)
            {
                var itemData = selectedLineSpawnData.ItemControlLevelDatas[i];
                if (i >= foldouts.Count)
                    foldouts.Add(false);

                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], $"Item {i + 1}", true);

                if (foldouts[i])
                {
                    Draw.BeginVertical("box");
                    Draw.Space(5);

                    // --- Item IDs ---
                    EditorGUILayout.LabelField("ItemIds:", EditorStyles.boldLabel);
                    for (int j = 0; j < itemData.ItemIds.Count; j++)
                    {
                        Draw.BeginHorizontal();
                        itemData.ItemIds[j] = Draw.IntPopupField(
                            $"ItemId:",
                            itemData.ItemIds[j],
                            GameConfig.Ins.ItemDatas,
                            "name", "id",
                            200
                        );
                        if (Draw.Button("-", Color.red, 25))
                        {
                            itemData.ItemIds.RemoveAt(j);
                            break;
                        }
                        if (Draw.Button("+", Color.green, 25))
                        {
                            itemData.ItemIds.Insert(j + 1, 0);
                            break;
                        }
                        Draw.EndHorizontal();
                    }

                    if (itemData.ItemIds.Count == 0 && Draw.Button("+ Add ItemId", Color.green, 120))
                        itemData.ItemIds.Add(0);

                    Draw.Space(5);

                    if (Draw.Button("Xóa Item", Color.red, 100))
                    {
                        selectedLineSpawnData.ItemControlLevelDatas.RemoveAt(i);
                        foldouts.RemoveAt(i);
                        Draw.EndVertical();
                        break;
                    }

                    Draw.EndVertical();
                    Draw.Space(5);
                }
            }

            // --- Thêm mới ---
            if (Draw.Button("+ Thêm ItemControl", Color.green, 200))
            {
                selectedLineSpawnData.ItemControlLevelDatas.Add(new ItemControlLevelData()
                {
                    Cell = selectedLineSpawnData.CellSpawn,
                    ItemIds = new List<int>() { 0, 0, 0, 0 },
                    EventType = ItemEventType.None,
                    LockId = 0
                });
                foldouts.Add(true); // Mở luôn item mới
            }

        }
        else
        {
            Draw.LabelBoldBox("Nhấp vào 1 ô trong grid để chỉnh ItemIds", Color.gray);
        }

        Draw.EndVertical();
        Draw.EndHorizontal();
    }



    //====================== LINE DATA ======================

    private void DrawLineList(LevelData level, int levelIndex)
    {
        if (!lineFoldouts.ContainsKey(levelIndex))
            lineFoldouts[levelIndex] = new List<bool>();

        // Nút thêm line
        if (Draw.Button("+ Add Line", Color.cyan, 120))
        {
            level.Lines.Add(new LineLevelData { Targets = new List<TargetLevelData>() });
            lineFoldouts[levelIndex].Add(true);
        }

        DrawItemSummary(level);

        // Vẽ từng line
        for (int l = 0; l < level.Lines.Count; l++)
        {
            var line = level.Lines[l];

            if (l >= lineFoldouts[levelIndex].Count)
                lineFoldouts[levelIndex].Add(false);

            Draw.BeginVertical("helpbox");
            lineFoldouts[levelIndex][l] = EditorGUILayout.Foldout(lineFoldouts[levelIndex][l],
                $"Line {l + 1}", true);

            if (lineFoldouts[levelIndex][l])
            {
                Draw.Space(5);
                Draw.BeginHorizontal();
                if (Draw.Button("+ Add Target", Color.green, 120))
                {
                    line.Targets.Add(new TargetLevelData());
                }
                if (Draw.Button("Xóa Line", Color.red, 120))
                {
                    level.Lines.RemoveAt(l);
                    lineFoldouts[levelIndex].RemoveAt(l);
                    Draw.EndHorizontal();
                    Draw.EndVertical();
                    break;
                }
                Draw.EndHorizontal();

                Draw.Space(5);

                var items = GameConfig.Ins.ItemDatas;

                List<int> itemLevelDataAll = new();
                foreach (var ItemControl in level.ItemControlLevelDatas)
                {
                    itemLevelDataAll.AddRange(ItemControl.ItemIds);
                }

                foreach (var lineSpawn in level.LineSpawnDatas)
                {
                    foreach (var itemControl in lineSpawn.ItemControlLevelDatas)
                    {
                        if (itemControl.ItemIds != null)
                            itemLevelDataAll.AddRange(itemControl.ItemIds);
                    }
                }

                int[] ids = itemLevelDataAll.Select(i => i).ToArray();
                string[] names = itemLevelDataAll
                    .Select(i =>
                    {
                        var info = GameConfig.Ins.GetItemData(i);
                        return info != null ? info.name : $"Bottle {i}";
                    })
                    .ToArray();

                // --- Hiển thị Targets trực tiếp ---
                for (int t = 0; t < line.Targets.Count; t++)
                {
                    var target = line.Targets[t];
                    Draw.BeginHorizontal();

                    target.ItemId = EditorGUILayout.IntPopup("Item:", target.ItemId, names, ids);
                    if (Draw.Button("X", Color.red, 25))
                    {
                        line.Targets.RemoveAt(t);
                        break;
                    }
                    Draw.EndHorizontal();
                }
            }

            Draw.EndVertical();
        }
    }


    private void DrawItemSummary(LevelData data)
    {
        Draw.BeginVertical("box");

        List<int> allItemIds = new();
        foreach (var itemControl in data.ItemControlLevelDatas)
        {
            if (itemControl.ItemIds != null)
                allItemIds.AddRange(itemControl.ItemIds);
        }

        foreach (var lineSpawn in data.LineSpawnDatas)
        {
            foreach (var itemControl in lineSpawn.ItemControlLevelDatas)
            {
                if (itemControl.ItemIds != null)
                    allItemIds.AddRange(itemControl.ItemIds);
            }
        }

        var placedGrouped = allItemIds
            .GroupBy(id => id)
            .ToDictionary(g => g.Key, g => g.Count());

        var targets = new List<TargetLevelData>();
        foreach (var line in data.Lines ?? new List<LineLevelData>())
            targets.AddRange(line.Targets ?? new List<TargetLevelData>());

        var targetGrouped = targets
            .GroupBy(t => t.ItemId)
            .ToDictionary(g => g.Key, g => g.Count());

        int totalPlaced = allItemIds.Count;
        int totalTarget = targets.Count;

        EditorGUILayout.LabelField("Item Usage Summary", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Total in ItemControl: {totalPlaced}");
        EditorGUILayout.LabelField($"Total used in Targets: {totalTarget}");
        EditorGUILayout.Space(3);

        var allItemIdsSet = new HashSet<int>(placedGrouped.Keys);
        foreach (var id in targetGrouped.Keys)
            allItemIdsSet.Add(id);

        foreach (int id in allItemIdsSet)
        {
            ColorFight.ItemData itemData = GameConfig.Ins.GetItemData(id);
            string name = itemData != null ? itemData.name : $"Item {id}";

            int placedCount = placedGrouped.ContainsKey(id) ? placedGrouped[id] : 0;
            int targetCount = targetGrouped.ContainsKey(id) ? targetGrouped[id] * 3 : 0;

            Color oldColor = GUI.color;
            GUI.color = placedCount == targetCount ? Color.green : new Color(1f, 0.6f, 0.6f);

            EditorGUILayout.LabelField($"{name} - Target: {targetCount} / Items: {placedCount}");
            GUI.color = oldColor;
        }

        Draw.EndVertical();
    }

}
