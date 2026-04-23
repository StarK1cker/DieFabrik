using System.Collections.Generic;
using UnityEngine;
using static SaveSystem;

[System.Serializable]
public class ObjectData
{
    public string id;
    public Vector3 position;
    public Quaternion rotation;
    public int level;
}

[System.Serializable]
public class SaveData
{
    public List<ObjectData> objects = new List<ObjectData>();
}

public class SaveableObject : MonoBehaviour
{
    public string objectID; // z.B. "Fliessband", "Ofen"
    public int level = 1;

    public virtual ObjectData Save()
    {
        return new ObjectData
        {
            id = objectID,
            position = transform.position,
            rotation = transform.rotation,
            level = level
        };
    }

    public virtual void Load(ObjectData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
        level = data.level;
    }
}