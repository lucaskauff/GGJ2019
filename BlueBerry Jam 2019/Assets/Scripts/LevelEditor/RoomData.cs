using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : Singleton<RoomData> {
    protected RoomData() { }

    #region Data
    public GameObject roomTemplatePrefab; 

    public List<GameObject> tilePrefabs = new List<GameObject>();
    public List<Sprite> editorArrows = new List<Sprite>();
    #endregion
}
