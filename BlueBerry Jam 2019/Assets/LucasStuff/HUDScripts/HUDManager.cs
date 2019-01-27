using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject hUD;

    private void Start()
    {
        //hUD.SetActive(false);
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
        hUD.SetActive(true);
    }

    public void HideHUD()
    {
        hUD.SetActive(false);
    }
}