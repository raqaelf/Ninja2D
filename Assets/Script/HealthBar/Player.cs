using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Stat health;

    private void Awake()
    {
        health.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            health.CurrentVal -= 10;
        }
        if (Input.GetKey(KeyCode.E))
        {
            health.CurrentVal += 10;
        }
    }
}
