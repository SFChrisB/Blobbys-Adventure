using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //public members
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    //private members
    Vector3 startingPos;    //must be stored for absolute movement
    float movementFactor;   // 0 for not moved, 1 for moved


    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }   //used instead of 0, as we are comparing two float values. Epsilon is smallest value.
        SetMovementFactor();
    }

    private void SetMovementFactor()
    {
        //protect against 'period' being divided by zero - NaN
        float cycles = Time.time / period;  //grows continuosly from 0

        const float tau = Mathf.PI * 2;     //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;    //0.5 needed to give us a value between 0 and 1
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
