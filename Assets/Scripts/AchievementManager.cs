using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    [Header("Ui 연결")]
    [SerializeField] private GameObject achievementPanel;

    [SerializeField] private TextMeshProUGUI titileText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float displayTime = 3.0f;

    private int totalColletecd;

    private bool firstitemUnlocked;
    private bool collectorUnlocked;
    private bool healUnlocked;
    private bool dropUnlocked;
    private bool QuestUnlocked;

    private Coroutine popupRoutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    public void itemCollected(int amount)
    {
        totalColletecd += amount;

        if (firstitemUnlocked == false)
        {
            firstitemUnlocked = true;
            Unlock("첫 수집" ,"첫 아이템 획득");
        }

        if(collectorUnlocked == false && totalColletecd >= 5)
        {
            collectorUnlocked = true;
            Unlock("초보 수집가", "아이템을 총 5개 획득");
        }
    }

    public void itemUsed(string itemName)
    {
        if (healUnlocked) return;

        if(itemName == "물약" || itemName == "회복약" || itemName == "구급키트")
        {
            healUnlocked = true;
            Unlock("건강해졌다.", "회복 아이템을 처음 사용 했습니다.");
        }
    }

    public void QuestCompleted()
    {
        if (QuestUnlocked) return;

        QuestUnlocked = true;

        Unlock("퀘스트 해결사","퀘스트를 처음 완료 했습니다.");
    }

    public void ItemDropped()
    {
        if (dropUnlocked) return;

        dropUnlocked = true;

        Unlock("과감한 정리", "아이템을 처음 벼렸습니다.");
    }

    private void Unlock(string title, string description)
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            achievementPanel.SetActive(false);
        }

        popupRoutine = StartCoroutine(ShowPopup(title, description));
    }





    private IEnumerator ShowPopup(string title, string description)
    {
        if (achievementPanel == null || titileText == null || descriptionText == null)
        {
            Debug.Log("ui 연결 안됌");
            yield break;
        }

        achievementPanel.SetActive(true);
        titileText.text = title;
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTime);

        achievementPanel.SetActive(false);
        popupRoutine = null;
    }
}
