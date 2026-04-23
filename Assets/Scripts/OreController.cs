using System;
using System.Collections;
using UnityEngine;

public class OreController : MonoBehaviour
{
    float cooldownTime = 5f;

    public GameObject oreItemPrefab;

    bool isOnCooldown = false;
    public Boolean isMining = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMining && !isOnCooldown)
        {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        Instantiate(oreItemPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        isOnCooldown = false;
    }
}
