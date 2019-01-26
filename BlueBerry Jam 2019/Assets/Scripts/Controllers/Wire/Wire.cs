using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using XInputDotNetPure;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    #region Private Values

    #region References
    private LineRenderer lineRenderer;

    #endregion

    #region Interface

    [BoxGroup("Setup")]
    public float distanceMax;

    [BoxGroup("Setup")]
    public LayerMask layerMask;

    [BoxGroup("Setup")]
    public GameObject wireStart;


    [BoxGroup("Debug")]
    [SerializeField]
    private List<GameObject> wirePoints = new List<GameObject>();

    [BoxGroup("Debug")]
    [SerializeField]
    private float distance;

    [BoxGroup("Debug")]
    [SerializeField]
    private float distanceLock;

    [BoxGroup("Debug")]
    [SerializeField]
    private GameObject previousPoint;

    [BoxGroup("Debug")]
    [SerializeField]
    private GameObject lastPoint;

    [BoxGroup("Debug")]
    [SerializeField]
    private GameObject lastHit;

    #endregion

    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        wirePoints.Add(wireStart);
        lastPoint = wirePoints[0];
    }

    private void Update()
    {
        CalculateLine();
        DrawLine();

        CalculateDistance();
    }
    #endregion

    #region Private Values
    private void CalculateLine()
    {
        float maxRange = Vector2.Distance(lastPoint.transform.position, this.transform.position);
        RaycastHit2D lastHit = Physics2D.Raycast(this.transform.position, (Vector2)lastPoint.transform.position - (Vector2)this.transform.position, maxRange, layerMask);

        if (lastHit.collider != null)
        {
            if (lastHit.collider.GetComponent<PlatformHook>())
            {
                PlatformHook platformHooks = lastHit.collider.GetComponent<PlatformHook>();
                GameObject closestPoint = platformHooks.ReturnClosest(lastHit.point);
                if ((Vector2)closestPoint.transform.position != (Vector2)lastPoint.transform.position)
                {
                    wirePoints.Add(closestPoint);
                    previousPoint = lastPoint;
                    lastPoint = closestPoint;
                }
            }
        }
        else if ((Vector2)previousPoint.transform.position != Vector2.zero)
        {
            maxRange = Vector2.Distance((Vector2)previousPoint.transform.position, this.transform.position);
            RaycastHit2D prevHit = Physics2D.Raycast(this.transform.position, (Vector2)previousPoint.transform.position - (Vector2)this.transform.position, maxRange, layerMask);

            if (Vector2.Distance(prevHit.point, (Vector2)previousPoint.transform.position) < 0.15f)
            {
                if (wirePoints.Count < 2) return;
                wirePoints.RemoveAt(wirePoints.Count - 1);
                lastPoint = wirePoints[wirePoints.Count - 1];
                previousPoint = wirePoints.Count == 1 ? null : wirePoints[wirePoints.IndexOf(lastPoint) - 1];
            }
        }
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = wirePoints.Count + 1;

        for (int i = 0; i < wirePoints.Count; i++)
        {
            lineRenderer.SetPosition(i, wirePoints[i].transform.position);
        }
        lineRenderer.SetPosition(wirePoints.Count, transform.position);
    }

    private void CalculateDistance()
    {
        float newDistance = 0;
        for (int i = 0; i < wirePoints.Count; i++)
        {
            if (i != 0)
            {
                newDistance += Vector2.Distance(wirePoints[i].transform.position, wirePoints[i - 1].transform.position);
            }
        }
        distance = newDistance + Vector2.Distance(this.transform.position, wirePoints[wirePoints.Count - 1].transform.position);
    }

    private Vector2 ClosestPoint(Vector2 point, Collider2D collider)
    {
        Vector2 colliderMin = collider.bounds.min;
        Vector2 colliderMax = collider.bounds.max;
        Vector2 rectangleMin;
        Vector2 rectangleMax;

        List<Vector2> rectanglePoints = new List<Vector2>();

        float W = collider.bounds.extents.x * 2;
        float H = collider.bounds.extents.y * 2;
        float aS = Mathf.Abs(Mathf.Sin(collider.transform.rotation.z));
        float cS = Mathf.Abs(Mathf.Cos(collider.transform.rotation.z));

        float h = (H * cS - W * aS) / (Mathf.Pow(cS, 2) - Mathf.Pow(aS, 2));
        float w = -(H * aS - W * cS) / (Mathf.Pow(cS, 2) - Mathf.Pow(aS, 2));

        rectangleMin = new Vector2(h * aS, h * cS);
        rectangleMax = new Vector2(w * cS, w * aS);

        rectanglePoints.Add(new Vector2(colliderMin.x, rectangleMax.y));
        rectanglePoints.Add(new Vector2(rectangleMax.x, colliderMax.y));
        rectanglePoints.Add(new Vector2(colliderMax.x, rectangleMin.y));
        rectanglePoints.Add(new Vector2(rectangleMin.x, colliderMin.y));

        float minDistance = 100;
        Vector2 closestPoint = Vector2.zero;

        for (int i = 0; i < rectanglePoints.Count; i++)
        {
            if (i != 0) Debug.DrawLine(rectanglePoints[i - 1], rectanglePoints[i]);

            if(Vector2.Distance(point, rectanglePoints[i]) < minDistance)
            {
                minDistance = Vector2.Distance(point, rectanglePoints[i]);
                closestPoint = rectanglePoints[i];
            }
        }

        /*
        List<float> xs = new List<float>();
        List<float> ys = new List<float>();
        List<Vector2> colliderPoints = new List<Vector2>();
        xs.Add(collider.bounds.min.x);
        xs.Add(collider.bounds.max.x);
        ys.Add(collider.bounds.min.y);
        ys.Add(collider.bounds.max.y);

        for (int i = 0; i < xs.Count; i++)
        {
            for (int ii = 0; ii < ys.Count; ii++)
            {
                colliderPoints.Add(new Vector2(xs[i], ys[ii]));
            }
        }

        xs.Add(collider.bounds.min.x);
        xs.Add(collider.bounds.max.x);
        ys.Add(collider.bounds.min.y);
        ys.Add(collider.bounds.max.y);
         */


        return closestPoint;
    }

    #endregion
}