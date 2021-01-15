using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector2Int position;

    public void Move(Vector2Int direction)
    {
        // Logic for moving   
        Vector2 start = transform.position;

        Vector2 end = start + direction;

        /*for the coordination of the obstacle (hole,enemy)
        if (end == )
        {
            
        }
        */
    } 
}
