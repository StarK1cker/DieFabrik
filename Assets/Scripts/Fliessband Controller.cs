using System;
using System.Collections;
using NUnit.Framework.Internal.Execution;
using Unity.VisualScripting;
using UnityEngine;

public class FliessbandController : MonoBehaviour
{
    GameObject player;

    Boolean isTransporting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        player.GetComponent<GameObjectPlacing>().gameObjects = GameObject.FindGameObjectsWithTag("GameObjects");
    }
    // Update is called once per frame
    void Update()
    {
        if (!isTransporting)
        {
            TransportOreItem();
        }
    }

    IEnumerator Cooldown(GameObject ore, Vector3 newOrePosition)
    {
        int level = GetComponent<SaveableObject>().level;
        isTransporting = true;
        yield return new WaitForSeconds(Mathf.Log(1.1f, level + 1) * 5f - 0.15f);
        ore.transform.position = newOrePosition;
        isTransporting = false;
    }

    void TransportOreItem()
    {
        GameObject[] oreItems = GameObject.FindGameObjectsWithTag("OreItem");

        Vector3 inputOffset = Vector3.zero;
        float rot = transform.eulerAngles.z;

        if (rot == 0) inputOffset = new Vector3(1, 0, 0);
        else if (rot == 90) inputOffset = new Vector3(0, 1, 0);
        else if (rot == 180) inputOffset = new Vector3(-1, 0, 0);
        else if (rot == 270) inputOffset = new Vector3(0, -1, 0);

        Vector3 newOrePosition = transform.position + inputOffset;

        foreach (GameObject ore in oreItems)
        {
            if (ore.transform.position == transform.position)
            {
                StartCoroutine(Cooldown(ore, newOrePosition)); 
            }
        }
    }

    public void Upgrade()
    {
        int level = GetComponent<SaveableObject>().level;
        print("Upgrade");
        if (player.GetComponent<GameObjectPlacing>().coins >= level * 10)
        {
            SoundManager.Instance.PlayFliessbandPlace();
            player.GetComponent<GameObjectPlacing>().coins -= level * 10;
            GetComponent<SaveableObject>().level++;
        }
    }

    
}
