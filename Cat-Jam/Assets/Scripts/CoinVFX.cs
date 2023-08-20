using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVFX : MonoBehaviour
{
    Material material;

    public float rotationSpeed = 30f;

    [Header("Glow Animation")]
    public Gradient gradient;
    public float intensity = 1;
    public float oscillationSpeed = 1f;
    private float time;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        material.EnableKeyword("_EMISSION");
        time += Time.deltaTime * oscillationSpeed;
        float t = Mathf.PingPong(time, 1f);
        Color emissionColor = gradient.Evaluate(t) * t * intensity;
        material.SetColor("_EmissionColor", emissionColor);
    }
}
