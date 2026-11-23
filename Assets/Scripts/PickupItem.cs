using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("아이템 정보")]

    [SerializeField] private string itemName = "아이템";

    [SerializeField] private int amount = 1;

    [SerializeField] QuestManager questManager;

    [TextArea]
    [SerializeField] private string description = "아이템 설명이 없습니다.";

    [Header("화면 연출")]
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float floatingSpeed = 2f;

    private Vector3 startPosition;

    private SimpleInventory inventory;

    private AchievementManager achievementManager;
    public void Awake()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        inventory = FindFirstObjectByType<SimpleInventory>();
        achievementManager = FindFirstObjectByType<AchievementManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float y = Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    public void Collect()
    {
        if(inventory == null)
        {
            Debug.Log("SimpleInventory = null");
            return;
        }

        if(inventory.Additem(itemName,amount,description) == false)
        {
            return;
        }

        Debug.LogWarning(itemName + " " + amount);
        if(questManager != null)
        {
            questManager.AddQuestItem(itemName, amount);
        }

        if(achievementManager != null)
        {
            achievementManager.itemCollected(amount);
        }

        Destroy(gameObject);
    }

    public string GetItemName()
    {
        return itemName;
    }
}
