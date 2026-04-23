using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameObjectPlacing : MonoBehaviour
{
    public GameObject saveSystem;

    public GameObject minerPrefab;
    public GameObject fliessbandPrefab;

    public Image minerButton;
    public Image fliessbandButton;
    public Image hammerButton;
    public GameObject rotationArrow;
    public GameObject upgradeUI;
    public TextMeshProUGUI upgradeText;

    int rotation = 90;
    int i = 0;

    bool upgrade;

    public int coins = 0;
    public TextMeshProUGUI coinText;

    Color selectedColor = new Color(255, 255, 255, 0.2f);

    public GameObject[] gameObjects;

    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("GameObjects");
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coins.ToString();

        SetPlacingRotation();
        rotationArrow.transform.rotation = Quaternion.Euler(0, 0, rotation);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (i != 3)
        {
            
            Vector3 roundedMousePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
            upgradeUI.SetActive(false);
            upgrade = false;
            foreach (GameObject obj in gameObjects)
            {
                if (obj != null && roundedMousePos == obj.transform.position)
                {
                    upgrade = true;
                    upgradeUI.SetActive(true);
                    print("Upgrade-Symbol");
                    upgradeUI.transform.position = new Vector3(mousePos.x + 10, mousePos.y + 20, 0);

                    if (obj.name.Contains("Miner"))
                    {
                        upgradeText.text = (obj.GetComponent<SaveableObject>().level * 100).ToString();
                    }
                    if (obj.name.Contains("Fliessband"))
                    {
                        upgradeText.text = (obj.GetComponent<SaveableObject>().level * 10).ToString();
                    }
                }
            }
            if (upgrade)
            {

                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    print(upgrade);
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        Upgrade(mousePos);
                    }
                }
            }
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectMiner();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelectFliessband();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelectHammer();
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && i == 1 && coins >= 100 && !upgrade)
            {
                PlaceMiner(mousePos);            
            }
            if (Mouse.current.leftButton.wasPressedThisFrame && i == 2 && coins >= 10 && !upgrade)
            {
                PlaceFliessband(mousePos);
            }
            if (Mouse.current.leftButton.wasPressedThisFrame && i == 3)
            {
                DestroyGameObject(mousePos);
            }
        }

    }

    void Upgrade(Vector2 mousePos)
    {
        print("Upgrade");
        Vector3 roundedMousePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null && roundedMousePos == obj.transform.position)
            {
                if (obj.name.Contains("Miner"))
                {
                    obj.GetComponent<MinerController>().Upgrade();
                }
                if (obj.name.Contains("Fliessband"))
                {
                    obj.GetComponent<FliessbandController>().Upgrade();
                }
            }
        }
    }

    public void SelectMiner()
    {
        SoundManager.Instance.PlayUI();
        i = 1;
        minerButton.color = selectedColor;
        fliessbandButton.color = new Color(1f, 1f, 1f, 1f);
        hammerButton.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SelectFliessband()
    {
        SoundManager.Instance.PlayUI();
        i = 2;
        minerButton.color = new Color(1f, 1f, 1f, 1f);
        fliessbandButton.color = selectedColor;
        hammerButton.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SelectHammer()
    {
        SoundManager.Instance.PlayUI();
        i = 3;
        minerButton.color = new Color(1f, 1f, 1f, 1f);
        fliessbandButton.color = new Color(1f, 1f, 1f, 1f);
        hammerButton.color = selectedColor;
    }

    void PlaceMiner(Vector2 mousePos)
    {
        Vector3 roundedMousePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null && gameObject.transform.position == roundedMousePos)
            {
                return;
            }
        }

        GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");

        foreach (GameObject ore in ores)
        {
            if (ore.transform.position == roundedMousePos)
            {
                Quaternion rotationQuat = Quaternion.Euler(0, 0, rotation);
                SoundManager.Instance.PlayMinerPlace();
                Instantiate(minerPrefab, roundedMousePos, rotationQuat);
                coins -= 100;
            }
        }
    }

    void PlaceFliessband(Vector2 mousePos)
    {
        Vector3 roundedMousePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null && gameObject.transform.position == roundedMousePos)
            {
                return;
            }
        }

        if (roundedMousePos.x > -1 && roundedMousePos.x < 2 && roundedMousePos.y > -1 && roundedMousePos.y < 2)
        {
            return;
        }

        Quaternion rotationQuat = Quaternion.Euler(0, 0, rotation);
        SoundManager.Instance.PlayFliessbandPlace();
        Instantiate(fliessbandPrefab, roundedMousePos, rotationQuat);
        coins -= 10;
    }

    void DestroyGameObject(Vector2 mousePos)
    {
        Vector3 roundedMousePos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameObjects");
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null && gameObject.transform.position == roundedMousePos)
            {
                Destroy(gameObject);
                break;
            }
        }
        GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
        foreach (GameObject ore in ores)
        {
            if (ore.transform.position == roundedMousePos)
            {
                SoundManager.Instance.PlaySell();
                Destroy(ore);
                coins += 20;
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("GameObjects");
    }

    void SetPlacingRotation()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            rotation = 90;
            SoundManager.Instance.PlayUI();
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            rotation = -90;
            SoundManager.Instance.PlayUI();
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            rotation = 180;
            SoundManager.Instance.PlayUI();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            rotation = 0;
            SoundManager.Instance.PlayUI();
        }
    }

    public void SetToNextRotation()
    {
        rotation -= 90;
        SoundManager.Instance.PlayUI();
    }
}
