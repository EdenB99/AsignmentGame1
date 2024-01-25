using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Background : MonoBehaviour
{
    Player Player;
    Transform[] bgSlots;
    public float BackgroundWidth = 36.0f;
    private void Awake()
    { 
       
        Transform[] bgSlots;
        bgSlots = new Transform[transform.childCount];
        for (int i = 0; i < bgSlots.Length; i++)
        {
            bgSlots[i] = transform.GetChild(i);
        }

    }
    void Update()
    {
        for (int i = 0; i < bgSlots.Length; i++)
        {
            if (Player.transform.position.x > bgSlots[i].position.x + 18.0f)
            {
                bgSlots[i].Translate(BackgroundWidth * bgSlots.Length * transform.right);
            }
            

        }
    }
}
