using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    Light _light;
    Vector2 boxSize;
    public LayerMask playerLayer;
    Vector3 player;
    float playerHP;
    float startingHp;
    public float waitTimeBeforeHeal = 1;

    [Header("Damage per sec")]
    public float damage;
    private bool isRayHittingTarget = false;
    private Coroutine coroutine;

    void Start()
    {
        _light = gameObject.GetComponent<Light>();
        boxSize = new Vector2(_light.areaSize.x, _light.areaSize.y);
        player = GameObject.Find("Player").transform.position;
        playerHP = GameObject.Find("Player").GetComponent<PlayerMovement>().playerHP;
        startingHp = GameObject.Find("Player").GetComponent<PlayerMovement>().startingHp;
    }

    void Update()
    {
        var ray = Physics.BoxCast(_light.transform.position, boxSize / 2f, -_light.transform.forward, out RaycastHit hit, Quaternion.identity, Mathf.Infinity, _light.cullingMask);
        DebugDrawBox();
        if (hit.collider.CompareTag("Player"))
        {
            Vector3 lightPos = transform.position;
            Vector3 result = ClosestPointFinder.FindClosestPoint(lightPos, boxSize, player);
            Vector3 difference_vector = player - lightPos;
            Vector3 direction_vector = difference_vector / difference_vector.magnitude;

            if (Physics.Raycast(result, direction_vector, difference_vector.magnitude + 5f))
            {
                isRayHittingTarget = true;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                playerHP -= damage * Time.deltaTime;
            }
            else
            {
                if (isRayHittingTarget)
                {
                    coroutine = StartCoroutine(TriggerCoroutine());
                    isRayHittingTarget = false;
                }
            }

        }
        
    }

    IEnumerator TriggerCoroutine()
    {
        yield return new WaitForSeconds(waitTimeBeforeHeal);
        
        while (playerHP < startingHp)
        {
            playerHP *= Time.deltaTime;
        }
    }

    void DebugDrawBox()
    {
        Vector3 dir = _light.transform.forward;
        Vector3 lightPosition = transform.position;
        Quaternion lightRotation = transform.rotation;

        Vector3 corner1 = lightPosition + lightRotation * new Vector3(-boxSize.x * 0.5f, boxSize.y * 0.5f, 0);
        Vector3 corner2 = lightPosition + lightRotation * new Vector3(boxSize.x * 0.5f, boxSize.y * 0.5f, 0);
        Vector3 corner3 = lightPosition + lightRotation * new Vector3(-boxSize.x * 0.5f, -boxSize.y * 0.5f, 0);
        Vector3 corner4 = lightPosition + lightRotation * new Vector3(boxSize.x * 0.5f, -boxSize.y * 0.5f, 0);

        float rayLength = 10000;
        Vector3 corner1End = corner1 + dir.normalized * rayLength;
        Vector3 corner2End = corner2 + dir.normalized * rayLength;
        Vector3 corner3End = corner3 + dir.normalized * rayLength;
        Vector3 corner4End = corner4 + dir.normalized * rayLength;

        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner4, Color.green);
        Debug.DrawLine(corner4, corner3, Color.green);
        Debug.DrawLine(corner3, corner1, Color.green);
        Debug.DrawLine(corner1, corner1End, Color.red);
        Debug.DrawLine(corner2, corner2End, Color.red);
        Debug.DrawLine(corner4, corner3End, Color.red);
        Debug.DrawLine(corner3, corner4End, Color.red);

    }
}
