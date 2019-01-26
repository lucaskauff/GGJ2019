using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoriesManager : MonoBehaviour
{
    [SerializeField]
    float letterSpeed = 0;

    public GameObject[] memories;
    public GameObject memoryText;

    GameObject selectedMemory;

    private void Start()
    {
        selectedMemory = memories[0];
    }

    private void Update()
    {
        foreach (GameObject item in memories)
        {
            if(item.GetComponent<MemoryTrigger>().memory.discovered)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void DisplayMemory(Memory memory)
    {
        memoryText.GetComponent<TextMeshProUGUI>().text = selectedMemory.GetComponent<MemoryTrigger>().memory.sentence;

        StopAllCoroutines();
        StartCoroutine(TypeMemory(selectedMemory.GetComponent<MemoryTrigger>().memory.sentence));
    }

    IEnumerator TypeMemory (string sentence)
    {
        memoryText.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            memoryText.GetComponent<TextMeshProUGUI>().text += letter;
            yield return new WaitForSeconds(letterSpeed);
        }
    }
}