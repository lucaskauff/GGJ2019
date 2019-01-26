using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public Memory memory;

    public void TriggerMemory()
    {
        FindObjectOfType<MemoriesManager>().DisplayMemory(memory);
    }
}