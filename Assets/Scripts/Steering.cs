using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Steering
{
    public float angular; // Угловое ускорение
    public Vector3 linear; // Линейное ускорение

    public Steering()
    {
        angular = 0.0f;
        linear = new Vector3();
    }
}
