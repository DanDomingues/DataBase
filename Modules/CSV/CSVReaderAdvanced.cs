using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReaderAdvanced : CSVReader
{
    [SerializeField] int startX;
    [SerializeField] int startY;
    [SerializeField] int xLength;
    [SerializeField] int yLength;

    public void ProcessValues(string serialized)
    {
        var grid = SplitCsvGrid(serialized);
        var values = new string[xLength, yLength];

        for (int j = 0; j < yLength; j++)
        {
            for (int i = 0; i < xLength; i++)
            {
                if(startX + i >= grid.GetLength(0) || startY + j >= grid.GetLength(1))
                {
                    Debug.Log(string.Format("[ERROR: {0}]Attempted to access {1} at a {2} grid", name,
                        new Vector2Int(startX + i, startY + j), new Vector2Int(grid.GetLength(0), grid.GetLength(1))));
                    continue;
                }
                values[i, j] = grid[startX + i, startY + j];
            }
        }

        ProcessValues(values);
    }
    public void ProcessFile()
    {
        ProcessValues(csvFile.text);
    }

    protected virtual void ProcessValues(string[,] input)
    {

    }
    public virtual void ApplyChanges(bool keepChanges)
    {

    }
    public int GetColumnIndex(string[,] source, string key, int row)
    {
        for (int i = 0; i < source.GetLength(0); i++)
        {
            //Debug.Log($"Checking {key} to {source[i, row]}");
            if (source[i, row].ToLower() == key.ToLower())
            {
                return i;
            }
        }

        return -1;
    }

    [ContextMenu("Set bounds to max")]
    public void SetBoundsToMax()
    {
        var grid = SplitCsvGrid(csvFile.text);
        xLength = grid.GetLength(0);
        yLength = grid.GetLength(1);
        startX = Mathf.Clamp(startX, 0, xLength - 1);
        startY = Mathf.Clamp(startY, 0, yLength - 1);
    }
    [ContextMenu("Process File", false, 900)]
    public void ProcessFile_Editor()
    {
        ProcessFile();
        KeepChanges(this);
    }

    protected void KeepChanges(ScriptableObject scriptableObject)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(scriptableObject);
#endif
    }
}
