using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPropierties : MonoBehaviour
{
    public bool isSpherical;
    public float Gravity;
    public float Mass;
    public float CalculateGravitationalForce(float massPlayer, float distance)
    {
        float G = 6.674f * Mathf.Pow(10, -11); // Constante gravitacional universal
        float force = G * (Mass * massPlayer) / Mathf.Pow(distance, 2);
        return force;
    }
}
