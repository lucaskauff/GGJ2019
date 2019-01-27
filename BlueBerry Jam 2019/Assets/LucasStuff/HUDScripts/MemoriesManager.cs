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
    public PlayerMapMov playerMapMov;

    public GameObject selectedMemory;

    private void Start()
    {
        selectedMemory = memories[0];

        foreach (GameObject item in memories)
        {
            item.GetComponent<Image>().enabled = false;
        }
    }

    private void Update()
    {
        foreach (GameObject item in memories)
        {
            if (item.GetComponent<MemoryTrigger>().memory.discovered  && !item.GetComponent<MemoryTrigger>().memory.yeppa)
            {
                item.GetComponent<MemoryTrigger>().memory.yeppa = true;
                NeuroneApparition(item);
            }
        }
    }

    public void DisplayMemory(Memory memory)
    {
        memoryText.GetComponent<TextMeshProUGUI>().text = selectedMemory.GetComponent<MemoryTrigger>().memory.sentence;

        StopAllCoroutines();
        StartCoroutine(TypeMemory(selectedMemory.GetComponent<MemoryTrigger>().memory.sentence));
    }

    public void NeuroneApparition(GameObject neurone)
    {
        Debug.Log(neurone);

        //ANIM APPARITION (OUPAS)
        neurone.GetComponent<Image>().enabled = true;

        //playerMapMov.SendMessage("Move", neurone.transform);
        playerMapMov.GetComponent<PlayerMapMov>().whereToGo = neurone.transform;
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