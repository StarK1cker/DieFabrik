using System.Net.Http.Headers;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float targetZoom = 10f;

    public float speed;

    Vector3 lastMousePos;

    public GameObject OreGenerator;
    public GameObject HoverField;

    int worldSize;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldSize = OreGenerator.GetComponent<WorldGeneration>().worldSize;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        HoverField.transform.position = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            targetPosition = new Vector3(0, 0, -10);
            targetZoom = 10f;
        }
        if(targetPosition == new Vector3(0, 0, -10) && targetZoom == 10f)
        {
            speed = 5f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * speed);
        }

        Zoom();
        Movement(mousePos);
        KeepCameraInWorldBorders();
    }

    void Zoom()
    {
        if(new Vector2(0, 0) != Mouse.current.scroll.ReadValue())
        {
            Vector2 scroll = Mouse.current.scroll.ReadValue();
            cam.orthographicSize -= scroll.y * cam.orthographicSize / 30;
            targetZoom = cam.orthographicSize;
        }
    }

    void Movement(Vector2 mousePos)
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame)
        {
            lastMousePos = mousePos;
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector3 newMousePos = mousePos;
            transform.position += lastMousePos - newMousePos;
            targetPosition = transform.position;
        }
    }

    void KeepCameraInWorldBorders()
    {
        //zoom
        if (cam.orthographicSize > worldSize)
        {
            targetZoom = worldSize;
            cam.orthographicSize = targetZoom;
        }
        if (cam.orthographicSize < 3)
        {
            targetZoom = 3;
            cam.orthographicSize = targetZoom;
        }

        //up
        if (worldSize < transform.position.y + cam.orthographicSize && transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, worldSize - cam.orthographicSize, transform.position.z);
        }
        //down
        if (worldSize < -transform.position.y + cam.orthographicSize && transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, -worldSize + cam.orthographicSize, transform.position.z);
        }
        //right
        if (worldSize * 16 / 9 < transform.position.x + cam.orthographicSize * 16 / 9 && transform.position.x > 0)
        {
            transform.position = new Vector3(worldSize * 16 / 9 - cam.orthographicSize * 16 / 9, transform.position.y, transform.position.z);
        }
        //left
        if (worldSize * 16 / 9 < -transform.position.x + cam.orthographicSize * 16 / 9 &&   transform.position.x < 0)
        {
            transform.position = new Vector3(-worldSize * 16 / 9 + cam.orthographicSize * 16 / 9, transform.position.y, transform.position.z);
        }
    }
}
