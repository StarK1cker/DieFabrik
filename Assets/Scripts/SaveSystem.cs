using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    public List<SaveableObject> saveables = new List<SaveableObject>();
    public List<GameObject> prefabs; // alle Prefabs im Inspector setzen

    private Dictionary<string, GameObject> prefabMap;

    public GameObject player;

    void Awake()
    {
        instance = this;

        prefabMap = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            prefabMap[prefab.name] = prefab;
        }
    }

    [System.Obsolete]
    public void Save()
    {
        PlayerPrefs.SetInt("coins", player.GetComponent<GameObjectPlacing>().coins);
        PlayerPrefs.Save();

        SaveData data = new SaveData();

        SaveableObject[] objs = FindObjectsOfType<SaveableObject>();

        foreach (SaveableObject obj in objs)
        {
            data.objects.Add(obj.Save());
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);

        SoundManager.Instance.PlaySave();
    }

    public void Load()
    {
        SoundManager.Instance.PlayUI();
        string path = Application.persistentDataPath + "/save.json";
        if (!File.Exists(path)) return;

        // Alte löschen
        foreach (SaveableObject obj in FindObjectsOfType<SaveableObject>())
        {
            Destroy(obj.gameObject);

        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (ObjectData objData in data.objects)
        {
            if (prefabMap.ContainsKey(objData.id))
            {
                GameObject obj = Instantiate(
                    prefabMap[objData.id],
                    objData.position,
                    objData.rotation
                );

                obj.GetComponent<SaveableObject>().Load(objData);
                if (obj.name.Contains("Miner"))
                {
                    StartCoroutine(obj.GetComponent<MinerController>().Load());
                }
                    
            }
        }
        player.GetComponent<GameObjectPlacing>().coins = PlayerPrefs.GetInt("coins", 0);
    }
}