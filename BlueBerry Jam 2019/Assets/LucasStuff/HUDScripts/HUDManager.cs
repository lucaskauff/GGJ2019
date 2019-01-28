using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject hUD;
    public Transform mindMap;
    public Transform player;

    private void Start()
    {
        hUD.SetActive(false);
    }

    private void Update()
    {
        //Condition to open HUD
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowHUD();
        }

        //Condition to close HUD
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideHUD();
        }
    }

    public void ShowHUD()
    {
        //Center on player's position
        mindMap.position = mindMap.position - player.position;
        hUD.SetActive(true);
    }

    public void HideHUD()
    {
        hUD.SetActive(false);
    }
}