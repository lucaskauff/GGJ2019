using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapMov : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    public bool hasToMove;
    //public Transform previousPos;
    public Transform whereToGo;

    private void Start()
    {
        //previousPos = transform;
        hasToMove = false;
    }

    private void Update()
    {
        if (hasToMove)
        {
            //Debug.Log("Tipar");
            transform.position = Vector2.MoveTowards(transform.position, whereToGo.position, moveSpeed * Time.deltaTime);
        }

        if (transform.position == whereToGo.position)
        {
            Debug.Log("Arrived");
            //previousPos = whereToGo;
            whereToGo = null;
            hasToMove = false;
        }
        else if (whereToGo != null)
        {
            hasToMove = true;
        }
    }

    /*
    public void Move(Transform target)
    {
        whereToGo = target;
        transform.position = Vector2.MoveTowards(previousPos.position, target.position, moveSpeed * Time.deltaTime);
    }
    */
}