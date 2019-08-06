using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            print("Space pressed");
        }

        if (Input.GetKey(KeyCode.A)) {
            print("Rotating left");
        } else if (Input.GetKey(KeyCode.D)){
            print("Rotating right");
        }
    }
}
