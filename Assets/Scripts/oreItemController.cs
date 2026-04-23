using UnityEngine;
using UnityEngine.UI;

public class oreItemController : MonoBehaviour
{
    GameObject player;
    public GameObject SellText;
    Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        canvas = FindFirstObjectByType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsObjectAtPosition())
        {
            Destroy(gameObject);
        }

        if (transform.position.x > -1 && transform.position.x < 2 && transform.position.y > -1 && transform.position.y < 2)
        {
            GameObject ui = Instantiate(SellText, canvas.transform);
            ui.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            SoundManager.Instance.PlaySell();
            player.GetComponent<GameObjectPlacing>().coins += 20;
            Destroy(gameObject);
        }
    }

    bool IsObjectAtPosition()
    {
        GameObject[] gameObjects = player.GetComponent<GameObjectPlacing>().gameObjects;

        foreach (GameObject gameObject in gameObjects)
        { 
            if (gameObject != null && transform.position == gameObject.transform.position)
            {
                return true;
            }
        }
        return false;
    }
}
