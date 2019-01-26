using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region Public Fields
    public Vector2 roomSize;

    [HideInInspector]
    public List<Bounds> tileBounds = new List<Bounds>();
    [HideInInspector]
    [SerializeField]
    public Dictionary<Bounds, GameObject> tiles = new Dictionary<Bounds, GameObject>();

    [HideInInspector]
    public Vector2Int intPos;
    #endregion

    #region Private Fields
    [SerializeField]
    private int startRopeAmount;

    private RoomGrid roomGrid;
    private BoxCollider2D boxCollider2D;
    #endregion

    #region StartUp Callback
    public void Initialize()
    {
        intPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        roomGrid = new RoomGrid(roomSize, intPos);

        tileBounds = roomGrid.CreateGrid();
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerTest>().ropeAmount = startRopeAmount;
            Camera.main.transform.position = boxCollider2D.bounds.center + new Vector3(0,0,-10);
        }
    }
    #endregion
}
