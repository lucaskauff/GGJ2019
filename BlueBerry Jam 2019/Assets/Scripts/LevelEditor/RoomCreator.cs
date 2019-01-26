using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomCreator : EditorWindow
{
    #region Rooms Values
    private List<GameObject> rooms = new List<GameObject>();
    private List<BoxCollider2D> roomColliders = new List<BoxCollider2D>();
    private Dictionary<BoxCollider2D, Color> roomColors = new Dictionary<BoxCollider2D, Color>();

    private GameObject selectedTilePrefab;
    private List<GameObject> tilePrefabs = new List<GameObject>();
    #endregion

    #region SelectedRoom Values
    private Vector2 roomGridSize;
    private Room selectedRoom;
    private GameObject selectedRoomObj;
    private Vector2 selectedRoomSize;
    #endregion

    #region General Values
    public enum Directions { up, right, down, left }
    private Directions direction;
    private string roomName;

    private bool edition = false;

    private Vector2 mousePosition;

    private LayerMask tileMask;
    private int tileMaskInt;
    private string[] options = new string[] { "Tiles" };
    #endregion

    //-------------------------------------------------------------

    #region Window Configuration
    [MenuItem("Tools/LevelEditor")]
    public static void ShowWindow()
    {
        GetWindow<RoomCreator>("Level Edition");
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        Setup();
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }


    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.CompareTag("Room"))
            {
                Debug.Log("New Room selected");
                selectedRoom = Selection.activeGameObject.GetComponent<Room>();

                RoomInitialization();
            }
        }
    }
    #endregion

    //-------------------------------------------------------------

    #region GUI Functionalities
    void OnGUI()
    {
        #region Basic Functionalities
        GUILayout.Label(" Tile selection", GUIStyle.none);
        selectedTilePrefab = (GameObject)EditorGUILayout.ObjectField(selectedTilePrefab, typeof(GameObject), true);

        GUILayout.BeginHorizontal();
        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            Texture prefabTexture = tilePrefabs[i].GetComponent<SpriteRenderer>().sprite.texture;

            if (GUILayout.Button(prefabTexture, GUILayout.MaxWidth(40), GUILayout.MaxHeight(40)))
            {
                selectedTilePrefab = tilePrefabs[i];
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label(" Room Grid parameters", GUIStyle.none);
        GUILayout.BeginHorizontal();
        selectedRoomSize = EditorGUILayout.Vector2Field("Grid Size", selectedRoomSize);
        if (GUILayout.Button("Update Grid", GUILayout.Height(30), GUILayout.Width(80)))
        {
            selectedRoom.roomSize = selectedRoomSize;
            RoomInitialization();
        }
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(10);

        #region Room Creation
        GUILayout.Label(" Room Creation", GUIStyle.none);
        roomName = EditorGUILayout.TextField(roomName);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create new Room") && selectedRoom != null)
        {
            Debug.Log("Created a new Room");

            Vector2 instantiatePos = Vector2.zero;
            switch (direction)
            {
                case Directions.right: instantiatePos = new Vector2(selectedRoomObj.transform.position.x + selectedRoom.roomSize.x, selectedRoomObj.transform.position.y);
                    break;
                case Directions.down:
                    instantiatePos = new Vector2(selectedRoomObj.transform.position.x, selectedRoomObj.transform.position.y - selectedRoom.roomSize.y);
                    break;
                case Directions.left:
                    instantiatePos = new Vector2(selectedRoomObj.transform.position.x - selectedRoom.roomSize.x, selectedRoomObj.transform.position.y);
                    break;
                case Directions.up:
                    instantiatePos = new Vector2(selectedRoomObj.transform.position.x, selectedRoomObj.transform.position.y + selectedRoom.roomSize.y);
                    break;
            }

            GameObject newRoom = Instantiate(RoomData.Instance.roomTemplatePrefab, instantiatePos, Quaternion.identity, RoomData.Instance.transform);

            if (roomName == string.Empty) newRoom.name = "New Room";
            else newRoom.name = roomName;
            newRoom.tag = "Room";

            Room newRoomScript = newRoom.GetComponent<Room>();
            newRoomScript.roomSize = selectedRoomSize;

            RoomInitialization();

            Selection.activeGameObject = newRoom;

            Setup();
        }
        direction = (Directions)EditorGUILayout.EnumFlagsField(direction);
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(10);

        #region Room Movement
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(RoomData.Instance.editorArrows[0].texture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)) && selectedRoom != null)
        {
            selectedRoomObj.transform.position = (Vector2)selectedRoomObj.transform.position + Vector2.up;
            selectedRoom.Initialize();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(RoomData.Instance.editorArrows[1].texture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)) && selectedRoom != null)
        {
            selectedRoomObj.transform.position = (Vector2)selectedRoomObj.transform.position + Vector2.left;
            selectedRoom.Initialize();
        }
        GUILayout.Space(50);
        if (GUILayout.Button(RoomData.Instance.editorArrows[2].texture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)) && selectedRoom != null)
        {
            selectedRoomObj.transform.position = (Vector2)selectedRoomObj.transform.position + Vector2.right;
            selectedRoom.Initialize();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(RoomData.Instance.editorArrows[3].texture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)) && selectedRoom != null)
        {
            selectedRoomObj.transform.position = (Vector2)selectedRoomObj.transform.position + Vector2.down;
            selectedRoom.Initialize();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(10);

        #region Tile Update Method
        if(GUILayout.Button("Update Tiles"))
        {
            selectedRoom.Initialize();

            selectedRoom.tiles.Clear();

            foreach(Bounds bounds in selectedRoom.tileBounds)
            {
                Collider2D[] result = Physics2D.OverlapBoxAll(bounds.center, bounds.size / 2, 0);

                for (int i = 0; i < result.Length; i++)
                {
                    if(result[i].gameObject != null & result[i].gameObject.CompareTag("Tile"))
                    {
                        selectedRoom.tiles.Add(bounds, result[i].gameObject);
                    }
                }
            }
        }
        #endregion
    }
    #endregion

    #region SceneHandle Functionalities
    void OnSceneGUI(SceneView sceneView)
    {
        #region Basic Functionalities
        mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        foreach (BoxCollider2D roomCollider in roomColliders)
        {
            if (roomCollider.gameObject.GetComponent<Room>() == selectedRoom && !edition)
            {
                Handles.DrawSolidRectangleWithOutline(new Rect((Vector2)roomCollider.gameObject.transform.position, roomCollider.size),
                    new Color(0, 1, 0, 0.25f), Color.clear);
            }
            else if (roomCollider.gameObject.GetComponent<Room>() != selectedRoom)
            {
                Handles.DrawSolidRectangleWithOutline(new Rect((Vector2)roomCollider.gameObject.transform.position, roomCollider.size),
                    roomColors[roomCollider], Color.clear);
            }
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E)
        {
            edition = !edition;

            if (edition)
            {
                Debug.Log("Entered Edit Mode");
                Tools.hidden = true;
            }
            else
            {
                Debug.Log("Exited Edit Mode");
                Tools.hidden = false;
            }
        }
        #endregion

        #region Edition Functionalities
        if (edition)
        {
            AddOrRemoveTile();

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
            {            
                foreach (Bounds bounds in selectedRoom.tiles.Keys)
                {
                    Debug.Log("C Pressed");
                    DestroyImmediate(selectedRoom.tiles[bounds]);
                }
                selectedRoom.tiles.Clear();
            }

            sceneView.Repaint();
        }
        else if(!edition)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
            {
                BoxCollider2D selectedRoomCollider = selectedRoom.GetComponent<BoxCollider2D>();

                roomColors.Remove(selectedRoomCollider);
                roomColliders.Remove(selectedRoomCollider);
                rooms.Remove(selectedRoomObj);

                Selection.activeGameObject = null;
                DestroyImmediate(selectedRoomObj);
            }

            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
            {
                foreach (BoxCollider2D roomCollider in roomColliders)
                {
                    if (roomCollider.gameObject.GetComponent<Room>() != selectedRoom)
                    {
                        if (roomCollider.bounds.Contains(mousePosition))
                        {
                            Selection.activeGameObject = roomCollider.gameObject;
                        }
                    }                     
                }
            }
        }
        #endregion
    }
    #endregion

    //-------------------------------------------------------------

    #region Private Methods
    private void Setup()
    {
        tilePrefabs = RoomData.Instance.tilePrefabs;
        if (tilePrefabs.Count > 0) selectedTilePrefab = tilePrefabs[0];

        rooms.Clear();
        roomColliders.Clear();
        roomColors.Clear();

        for (int i = 0; i < RoomData.Instance.gameObject.transform.childCount; i++)
        {
            rooms.Add(RoomData.Instance.gameObject.transform.GetChild(i).gameObject);
            roomColliders.Add(rooms[i].GetComponent<BoxCollider2D>());

            roomColors.Add(roomColliders[i], new Color(Random.value, 0, Random.value, 0.25f));
        }
    }

    private void RoomInitialization()
    {
        selectedRoom.Initialize();

        BoxCollider2D collider2D = selectedRoom.GetComponent<BoxCollider2D>();
        collider2D.isTrigger = true;
        collider2D.offset = selectedRoom.roomSize * 0.5f;
        collider2D.size = selectedRoom.roomSize;

        selectedRoomObj = selectedRoom.gameObject;
        selectedRoomSize = selectedRoom.roomSize;
    }

    private void AddOrRemoveTile()
    {
        if (selectedRoom != null)
        {

            foreach (Bounds bounds in selectedRoom.tileBounds)
            {
                if (bounds.Contains(mousePosition))
                {
                    Handles.color = Color.green;
                    Handles.DrawSolidRectangleWithOutline(new Rect((Vector2)bounds.center - (Vector2.one * 0.5f), bounds.size), Color.green, Color.clear);

                    if (Event.current.type == EventType.MouseDown)
                    {
                        if (Event.current.button == 0 && selectedTilePrefab != null)
                        {
                            RemoveTile(bounds);
                            GameObject tile = Instantiate(selectedTilePrefab, bounds.center, Quaternion.identity);

                            if (selectedTilePrefab.name.Contains("Ground")) tile.transform.parent = selectedRoomObj.transform.GetChild(0);
                            else tile.transform.parent = tile.transform.parent = selectedRoomObj.transform;

                            selectedRoom.tiles.Add(bounds, tile);
                        }

                        if (Event.current.button == 1)
                        {
                            RemoveTile(bounds);
                        }
                    }
                }
                else
                {
                    Handles.color = Color.blue;
                    Handles.RectangleHandleCap(0, bounds.center, Quaternion.identity, 0.5f, EventType.Repaint);
                }
            }
        }
    }

    private void RemoveTile(Bounds bounds)
    {
        if (selectedRoom.tiles.ContainsKey(bounds))
        {
            DestroyImmediate(selectedRoom.tiles[bounds]);
            selectedRoom.tiles.Remove(bounds);
        }
    }
    #endregion
}

