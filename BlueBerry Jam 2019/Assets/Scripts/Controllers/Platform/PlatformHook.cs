using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PlatformHook : MonoBehaviour
{
    #region Private Values

    #region References

    #endregion

    #endregion

    #region Interface
    [BoxGroup("Setup")]
    public List<GameObject> hookPoints = new List<GameObject>();

    [BoxGroup("Setup")][SerializeField]
    [ReadOnly]
    private Quaternion rotation;

    [BoxGroup("Setup")][SerializeField]
    [Button]
    private void UpdateHooks()
    {
        Collider2D collider = this.GetComponent<Collider2D>();
        rotation = transform.rotation;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        List<float> xs = new List<float>();
        List<float> ys = new List<float>();

        xs.Add(collider.bounds.min.x);
        xs.Add(collider.bounds.max.x);
        ys.Add(collider.bounds.min.y);
        ys.Add(collider.bounds.max.y);

        hookPoints.Clear();        
        for (int i = 0; i < xs.Count; i++)
        {
            for (int ii = 0; ii < ys.Count; ii++)
            {
                hookPoints.Add(Instantiate(new GameObject(), new Vector2(xs[i], ys[ii]), Quaternion.identity, this.transform));
            }
        }

        transform.rotation = rotation;
    }
    #endregion

    #region Interface Functions

    #endregion

    #region MonoBehaviour Callbacks

    #endregion

    #region Public Methods
    public GameObject ReturnClosest(Vector2 point)
    {
        float minDistance = 100;
        GameObject closestPoint = null;
        foreach (GameObject hookPoint in hookPoints)
        {
            if (Vector2.Distance(point, hookPoint.transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(point, hookPoint.transform.position);
                closestPoint = hookPoint;
            }
        }
        return closestPoint;
    }

    #endregion
}
