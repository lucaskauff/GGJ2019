using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointVisibility : MonoBehaviour
{
    public GameObject startMemory;
    public GameObject endMemory;
    Image myImage;
    bool isVisible;

    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = !myImage.enabled;
        isVisible = false;
    }

    private void Update()
    {
        if (startMemory.GetComponent<MemoryTrigger>().memory.discovered && endMemory.GetComponent<MemoryTrigger>().memory.discovered && !isVisible)
        {
            myImage.enabled = !myImage.enabled;
            isVisible = true;
        }
    }
}