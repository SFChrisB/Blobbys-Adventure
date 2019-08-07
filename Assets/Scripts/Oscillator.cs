using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //public members
    [SerializeField] Vector3 movementVector;
    // todo remove from inspector later
    [SerializeField][Range(0,1)] float movementFactor; // 0 for not moved, 1 for moved

    //private members
    Vector3 startingPos;    //must be stored for absolute movement

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
