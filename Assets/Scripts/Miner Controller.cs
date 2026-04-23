using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MinerController : MonoBehaviour
{

    GameObject ore;
    GameObject player;

    public GameObject oreItemPrefab;

    bool isOnCooldown = false;

    Vector3 inputOffset = Vector3.zero;
    float rot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        player.GetComponent<GameObjectPlacing>().gameObjects = GameObject.FindGameObjectsWithTag("GameObjects");

        rot = transform.eulerAngles.z;
        if (rot == 0) inputOffset = new Vector3(1, 0, 0);
        else if (rot == 90) inputOffset = new Vector3(0, 1, 0);
        else if (rot == 180) inputOffset = new Vector3(-1, 0, 0);
        else if (rot == 270) inputOffset = new Vector3(0, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        int level = GetComponent<SaveableObject>().level;
        isOnCooldown = true;
        yield return new WaitForSeconds(40.0f / (Mathf.Log(level + 1, 2)*2));
        Instantiate(oreItemPrefab, transform.position + inputOffset, Quaternion.identity);
        isOnCooldown = false;
    }

    public void Upgrade()
    {
        int level = GetComponent<SaveableObject>().level;
        if (player.GetComponent<GameObjectPlacing>().coins >= level * 100)
        {
            SoundManager.Instance.PlayFliessbandPlace();
            player.GetComponent<GameObjectPlacing>().coins -= level * 100;
            GetComponent<SaveableObject>().level++;
        }
    }

    public IEnumerator Load()
    {
        isOnCooldown = true;
        float seconds = UnityEngine.Random.Range(0f, 10f);
        print("Loading... " + seconds);
        yield return new WaitForSeconds(seconds);
        isOnCooldown = false;
    }
}
