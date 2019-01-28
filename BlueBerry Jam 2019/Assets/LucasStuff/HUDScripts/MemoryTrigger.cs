using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryTrigger : MonoBehaviour
{
    public Memory memory;
    public GameObject noyau;

    private void Update()
    {
        /*
        if (memory.discovered)
        {
            if (memory.associatedIcon == Icons.Boat)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Boat");
            }

            if (memory.associatedIcon == Icons.Heart)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Heart");
            }

            if (memory.associatedIcon == Icons.Home)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Home");
            }

            if (memory.associatedIcon == Icons.Leaf)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Leaf");
            }

            if (memory.associatedIcon == Icons.Star)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Star");
            }

            if (memory.associatedIcon == Icons.Web)
            {
                noyau.GetComponent<Image>().sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Web");
            }
        }
        */
    }

    public void TriggerMemory()
    {
        FindObjectOfType<MemoriesManager>().selectedMemory = gameObject;
        FindObjectOfType<MemoriesManager>().DisplayMemory(memory);
    }
}