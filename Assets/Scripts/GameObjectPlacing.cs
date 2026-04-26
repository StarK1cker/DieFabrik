using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameObjectPlacing : MonoBehaviour
{
    public GameObject saveSystem;

    public GameObject minerPrefab;
    public GameObject fliessbandPrefab;

    public Image minerButton;
    public Image fliessbandButton;
    public Image upgradeButton;
    public Image hammerButton;
    public GameObject rotationArrow;
    public GameObject upgradeUI;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI upgradeSelectedObjectsText;
    public GameObject selectionField;
    public GameObject selectedObjectsUpgradeButton;
    int rotation = 90;
    int i = 0;
    int upgradeCosts; 

    Vector2 startMousePos;

    bool upgrade;

    public int coins = 0;
    public TextMeshProUGUI coinText;

    Color selectedColor = new Color(1f, 1f, 1f, 0.2f);

    public GameObject[] gameObjects;
    private List<GameObject> selectedObjects = new List<GameObject>();
    private List<GameObject> calculatedObjects = new List<GameObject>();
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

        UpgradeSystem(mousePos);

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
            SelectUpgrade();
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
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
            if (Mouse.current.leftButton.wasPressedThisFrame && i == 4)
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
        upgradeButton.color = new Color(1f, 1f, 1f, 1f);
        hammerButton.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SelectFliessband()
    {
        SoundManager.Instance.PlayUI();
        i = 2;
        minerButton.color = new Color(1f, 1f, 1f, 1f);
        upgradeButton.color = new Color(1f, 1f, 1f, 1f);
        fliessbandButton.color = selectedColor;
        hammerButton.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SelectUpgrade()
    {
        SoundManager.Instance.PlayUI();
        i = 3;
        minerButton.color = new Color(1f, 1f, 1f, 1f);
        fliessbandButton.color = new Color(1f, 1f, 1f, 1f);
        upgradeButton.color = selectedColor;  
        hammerButton.color = new Color(1f, 1f, 1f, 1f);
    }

    public void SelectHammer()
    {
        SoundManager.Instance.PlayUI();
        i = 4;
        minerButton.color = new Color(1f, 1f, 1f, 1f);
        fliessbandButton.color = new Color(1f, 1f, 1f, 1f);
        upgradeButton.color = new Color(1f, 1f, 1f, 1f);
        hammerButton.color = selectedColor;
    }

    void UpgradeSystem(Vector2 mousePos)
    {
        if (i == 3)
        {
            selectedObjectsUpgradeButton.SetActive(true);
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
                    upgradeUI.transform.position = new Vector3(Mouse.current.position.ReadValue().x + 10, Mouse.current.position.ReadValue().y + 20, 0);

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
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startMousePos = mousePos;
                selectionField.SetActive(true);
                selectionField.transform.localScale = Vector3.zero;
                selectionField.transform.position = startMousePos;
            } else if (Mouse.current.leftButton.isPressed)
            {
                Vector2 currentMousePos = mousePos;
                Vector2 center = (startMousePos + currentMousePos) / 2;
                selectionField.transform.position = center;
                Vector3 size = new Vector3(Mathf.Abs(currentMousePos.x - startMousePos.x), Mathf.Abs(currentMousePos.y - startMousePos.y), 0);
                selectionField.transform.localScale = size;
            }
            if (Mouse.current.leftButton.wasReleasedThisFrame) 
            {
                foreach (GameObject obj in gameObjects)
                {
                    if(obj != null && obj.transform.position.x > selectionField.transform.position.x - selectionField.transform.localScale.x / 2 &&
                       obj.transform.position.x < selectionField.transform.position.x + selectionField.transform.localScale.x / 2 &&
                       obj.transform.position.y > selectionField.transform.position.y - selectionField.transform.localScale.y / 2 &&
                       obj.transform.position.y < selectionField.transform.position.y + selectionField.transform.localScale.y / 2)
                    {
                        if (!selectedObjects.Contains(obj))
                        {
                            selectedObjects.Add(obj);
                        }
                        
                    }
                }
                selectionField.SetActive(false);
            }
            foreach (GameObject obj in selectedObjects)
            {
                if(obj != null && !calculatedObjects.Contains(obj))
                {
                    obj.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.8f, 1f, 1f);
                    if (obj.name.Contains("Miner"))
                    {
                        upgradeCosts += obj.GetComponent<SaveableObject>().level * 100;
                        calculatedObjects.Add(obj);
                    } else if (obj.name.Contains("Fliessband"))
                    {
                        upgradeCosts += obj.GetComponent<SaveableObject>().level * 10;
                        calculatedObjects.Add(obj);
                    }
                    upgradeSelectedObjectsText.text = upgradeCosts.ToString();
                }
            }
            if(Keyboard.current.enterKey.wasPressedThisFrame)
            {
                UpgradeSelectedObjects();
            }
        } else // i != 3
        {
            selectedObjectsUpgradeButton.SetActive(false);
            upgradeUI.SetActive(false);
            upgrade = false;
        }

        if (Mouse.current.middleButton.wasPressedThisFrame || i != 3)
        {
            foreach (GameObject obj in selectedObjects)
            {
                if(obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            selectedObjects.Clear();
            calculatedObjects.Clear();
            upgradeCosts = 0;
            upgradeSelectedObjectsText.text = upgradeCosts.ToString();
        }
    }
    

    public void UpgradeSelectedObjects()
    {
        if(upgradeCosts > coins)
        {
            return;
        }
        foreach (GameObject obj in calculatedObjects)
        {
            if (obj != null)
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
        calculatedObjects.Clear();
        upgradeCosts = 0;
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

