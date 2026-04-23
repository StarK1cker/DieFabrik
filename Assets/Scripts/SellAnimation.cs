using TMPro;
using UnityEngine;

public class SellAnimation : MonoBehaviour
{
    Vector3 targetPosition;

    Camera cam;

    Vector3 lastMousePos;

    public float speed;

    public GameObject prefab;
    GameObject obj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        obj = Instantiate(prefab, cam.ScreenToWorldPoint(transform.position), Quaternion.identity);

        transform.localScale = 10 / cam.orthographicSize * Vector3.one;
        targetPosition = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.3f * Mathf.Log(cam.orthographicSize, 2f), -10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = 10 / cam.orthographicSize * Vector3.one;

        obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, Time.deltaTime * speed * Mathf.Log(cam.orthographicSize, 1.5f));

        if (Vector3.Distance(obj.transform.position, targetPosition) < 0.5f)
        {
            Destroy(gameObject);
        }

        transform.position = cam.WorldToScreenPoint(obj.transform.position);
    }
    
}
