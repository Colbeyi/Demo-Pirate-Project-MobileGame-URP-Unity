using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveTypes
{
    public static Vector3 GerstnerWave(Vector3 position, Vector2 direction, float steepness, float wavelenght, float speed, float timeSinceStart){

    float k = 2 * Mathf.PI / wavelenght;

    Vector2 normalizedDirection = direction.normalized;

    float f = k * Vector2.Dot(normalizedDirection, new Vector3(position.x, position.z)) - (speed * timeSinceStart);
    float a = steepness / k;

    return new Vector3(normalizedDirection.x * a * Mathf.Cos(f), a * Mathf.Sin(f), normalizedDirection.y * a * Mathf.Cos(f));

   }
}
