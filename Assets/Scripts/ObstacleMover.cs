using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] Vector3 movementVector; // this is the total distance / direction that the object is to move
    [SerializeField] float period = 2f;
    Vector3 startingPosition;
    float movementFactor; // this is the range between the starting position and finishing position

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) {return;}
        float cycles = Time.time / period; // number of cycles that have elapsed

        const float tau = Mathf.PI * 2; // tau is the total angle in a circle
        float rawSinWave = Mathf.Sin(cycles * tau); // value going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // recalculating Sin value to between 0 and 1

        Vector3 offset = movementVector * movementFactor; // this shows the distance travelled from the starting position
        transform.position = startingPosition + offset;
    }
}
