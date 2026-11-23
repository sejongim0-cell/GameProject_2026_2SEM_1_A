using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    private Quest currentQuest;

    private SimpleInventory inventory;
    private AchievementManager achievementManager;

    private void Awake()
    {
        inventory = FindFirstObjectByType<SimpleInventory>();
        achievementManager = FindFirstObjectByType<AchievementManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questText.text = "진행 중 인 퀘스트가 없음";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TalkToNPC(Quest npcQuest)
    {
        if(npcQuest.state == QuestState.Completed)
        {
            Debug.Log("이미 완료한 퀘스트");
            return;
        }

        if(currentQuest == null || currentQuest.state == QuestState.Completed)
        {
            StartQuest(npcQuest);
        }

        if(currentQuest != npcQuest)
        {
            Debug.Log("이미 다른 쿼스트를 진행 중");
            return;
        }

        if(currentQuest.state == QuestState.InProgress)
        {
            Debug.Log("퀘스트 아이템이 부족");
        }
        else if(currentQuest.state == QuestState.ReadyToComplete)
        {
            CompleteQuest();
        }
    }


    private void UpdateQuestText()
    {
        if(currentQuest.state == QuestState.InProgress)
        {
            questText.text =
                currentQuest.questName + "\n" +
                currentQuest.targetItemName + " " +
                currentQuest.currentAmount + "/ " +
                currentQuest.targetAmount;
        }
        else if(currentQuest.state == QuestState.ReadyToComplete)
        {
            questText.text = "수집 완료! \nNPC에게 돌아가세요!";
        }
        else if(currentQuest.state == QuestState.Completed)
        {
            questText.text = currentQuest.questName + "\n쿼스트 완료!";
        }
    }

    void StartQuest(Quest newQuest)
    {
        currentQuest = newQuest;
        currentQuest.currentAmount = inventory.GetitemAmount(currentQuest.targetItemName);

        if(currentQuest.currentAmount >= currentQuest.targetAmount)
        {
            currentQuest.currentAmount = currentQuest.targetAmount;
            currentQuest.state = QuestState.ReadyToComplete;
        }
        else
        {
            currentQuest.state = QuestState.InProgress;
        }

        currentQuest.state = QuestState.InProgress;

        Debug.Log(currentQuest.questName + "퀘스트 시작함");
        UpdateQuestText();
    }

    public void AddQuestItem(string itemName, int amount)
    {
        if (currentQuest == null) return;
        if (currentQuest.state != QuestState.InProgress) return;
        if (currentQuest.targetItemName != itemName) return;

        currentQuest.currentAmount += amount;

        if(currentQuest.currentAmount >= currentQuest.targetAmount)
        {
            currentQuest.currentAmount = currentQuest.targetAmount;
            currentQuest.state = QuestState.ReadyToComplete;
        }

        UpdateQuestText();
    }

    public void CompleteQuest()
    {
        if(inventory.RemoveItem(currentQuest.targetItemName,currentQuest.targetAmount) == false)
        {
            Debug.Log("퀘스트 아이템이 부족합니다.");
            return;
        }
        currentQuest.state = QuestState.Completed;

        if(achievementManager != null)
        {
            achievementManager.QuestCompleted();
        }

        Debug.Log(currentQuest.questName + "퀘스트 완료!");
        UpdateQuestText();
    }
}
