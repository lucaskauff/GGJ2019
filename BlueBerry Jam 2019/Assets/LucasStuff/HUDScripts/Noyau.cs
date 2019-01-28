using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noyau : MonoBehaviour
{
    public MemoryTrigger origin;
    Image myImage;

    private void Start()
    {
        myImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(origin.memory.discovered)
        {
            if(origin.memory.associatedIcon == Icons.Boat)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Boat");
            }

            if (origin.memory.associatedIcon == Icons.Heart)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Heart");
            }

            if (origin.memory.associatedIcon == Icons.Home)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Home");
            }

            if (origin.memory.associatedIcon == Icons.Leaf)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Leaf");
            }

            if (origin.memory.associatedIcon == Icons.Star)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Star");
            }

            if (origin.memory.associatedIcon == Icons.Web)
            {
                myImage.sprite = Resources.Load<Sprite>("LucasStuff/Sprites/Finaux/Web");
            }
        }
    }
}