using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class SimpleInventory : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI[] slotText;

    [SerializeField] private TextMeshProUGUI selecteditemText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private int HealAmount = 25;

    private List<inventoryitem> items = new List<inventoryitem>();

    private PlayerContoller playerContoller;
    private PlayerPickup playerPickup;
    private ThirdPersonCamera thirdPersonCamera;
    private bool isOpen;

    [SerializeField] private int selectedIndex = -1;
    //-1 : 선택 없음 , 0 : 첫번째 슬롯 1, 두 번째 슬롯 -> 3

    private AchievementManager achievementManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerContoller = GetComponent<PlayerContoller>();
        playerPickup = GetComponent<PlayerPickup>();
        achievementManager = FindAnyObjectByType < AchievementManager>();

        if(Camera.main != null)
        {
            thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();
        }

        inventoryPanel.SetActive(false);
    }

    void Start()
    {
        UpdateUi();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null) return;

        if(keyboard.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if(isOpen == false && playerContoller.IsNormalState() == false)
        {
            return;
        }

        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if(isOpen)
        {
            playerContoller.ChangeState(PlayerState.inventory);
            playerPickup.enabled = false;

            if(thirdPersonCamera != null)
            {
                thirdPersonCamera.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerContoller.ChangeState(PlayerState.Normal);
            playerPickup.enabled = true;

            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.enabled = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    private void UpdateUi()
    {
        for(int i = 0; i < slotText.Length; i++)
        {
            if(i < items.Count)
            {
                slotText[i].text = items[i].itemName + "\n" + items[i].amount + "개";
            }
            else
            {
                slotText[i].text = "비어 있음";
            }
        }

        if(selectedIndex >= 0 && selectedIndex < items.Count)
        {
            selecteditemText.text = "선택 : " + items[selectedIndex].itemName;
            descriptionText.text = items[selectedIndex].description;

            useButton.interactable = CanUseItem(items[selectedIndex].itemName);
            dropButton.interactable = true;
        }
        else
        {
            selecteditemText.text = "아이템을 선택하세요";
            descriptionText.text = "슬롯을 클릭하면 설명이 표시 됩니다.";


            useButton.interactable = false;
            dropButton.interactable = false;
        }
    }

    public bool Additem(string itemName, int amount, string description)
    {
        foreach(inventoryitem item in items)
        {
            if(item.itemName == itemName)
            {
                item.amount += amount;
                UpdateUi();
                return true;
            }
        }

        if(items.Count >= slotText.Length)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
            return false;
        }

        inventoryitem newitem = new inventoryitem();
        newitem.itemName = itemName;
        newitem.amount = amount;
        newitem.description = description;

        items.Add(newitem);
        UpdateUi();

        return true;

    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= items.Count) return;

        selectedIndex = index;

        UpdateUi();
    }

    public void SelectSlot0() { SelectSlot(0); }
    public void SelectSlot1() { SelectSlot(1); }
    public void SelectSlot2() { SelectSlot(2); }
    public void SelectSlot3() { SelectSlot(3); }

    private bool CanUseItem(string itemName)
    {
        return itemName == "물약" || itemName == "회복약" || itemName == "구급키트";
    }

    public bool RemoveItem(string itemName, int Amount)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName != itemName) continue;
            if (items[i].amount < Amount) return false;

            items[i].amount -= Amount;

            if (items[i].amount <= 0)
            {
                items.RemoveAt(i);
                selectedIndex = -1;
            }

            UpdateUi();
            return true;
        }

        return false;
    }

    public void UseSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count) return;

        inventoryitem item = items[selectedIndex];

        if(CanUseItem(item.itemName) == false)
        {
            selecteditemText.text = item.itemName + "은 사용 할 수 없습니다."; return;
        }

        playerHealth.Heal(HealAmount);

        if(achievementManager != null)
        {
            achievementManager.itemUsed(item.itemName);
        }
        RemoveItem(item.itemName, 1);
    }

    public void DropSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count) return;

        string itemName = items[selectedIndex].itemName;
        if(RemoveItem(itemName, 1))
        {
            if(achievementManager != null)
            {
                achievementManager.ItemDropped();
            }
        }
    }

    public int GetitemAmount(string itemName)
    {
        foreach(inventoryitem item in items)
        {
            if(item.itemName == itemName)
            {
                return item.amount;
            }
        }

        return 0;
    }

}
