﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    AudioSource eatingAudio;

    // Start is called before the first frame update
    void Start()
    {
        eatingAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        Edible food = other.gameObject.GetComponent<Edible>();
        if (food)
        {
            food.Eaten();
        }
    }

}
