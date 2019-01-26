using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid
{
    #region Private Fields
    private readonly Vector2 roomSize;
    private readonly Vector2Int startPoint;
    #endregion

    #region Constructor
    public RoomGrid(Vector2 _roomSize, Vector2Int _startPoint)
    {
        roomSize = _roomSize;
        startPoint = _startPoint;
    }
    #endregion

    #region Public Methods
    public List<Bounds> CreateGrid()
    {
        List<Bounds> tilesBounds = new List<Bounds>();

        for (int x = startPoint.x; x < roomSize.x + startPoint.x; x++)
        {
            for (int y = startPoint.y; y < roomSize.y + startPoint.y; y++)
            {
                Vector2 tileCenter = new Vector2(x + 0.5f, y + 0.5f);
                Bounds tileBounds = new Bounds(tileCenter, Vector2.one);

                tilesBounds.Add(tileBounds);
            }
        }

        return tilesBounds;
    }
    #endregion
}
