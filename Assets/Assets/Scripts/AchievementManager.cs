using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementManager : MonoBehaviour
{

    [Header("UI 연결")]
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private TextMeshProUGUI titileText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float displayTime = 3.0f;

    private int totalCollected;                  //아이템 획득 횟수
    private bool firstItemUnlocked;              //아이템 획득 첫번째
    private bool collectorUnlocked;              //아이템 5개 이상 획득시
    private bool healUnlocked;                   //힐링을 했을때 업적
    private bool dropUnlocked;                   //아이템을 버렸을때 업적
    private bool QeustUnlocked;                  //퀘스트 완료시 업적

    private Coroutine popupRoutine;              //업적창 연출을 위한 코루틴

    public void ItemCollected(int amount)
    {
        totalCollected += amount;

        if (firstItemUnlocked == false)
        {
            firstItemUnlocked = true;
            Unlock("첫 수집", "처음으로 아이템을 획득 했습니다. ");
        }

        if (collectorUnlocked == false && totalCollected >= 5)
        {
            collectorUnlocked = true;
            Unlock("초보 수집가", "아이템을 총 5개 획득했습니다.");
        }

    }
        public void ItemUsed(string itemName)
    {
        if (healUnlocked) return;

        if (itemName == "물약" || itemName == "회복약" || itemName == "구급키트")
        {
            healUnlocked = true;
            Unlock("건강이 최고", "회복 아이템을 처음 사용 했습니다.");
        }
    }

    public void ItemDropped()
    {
        if (dropUnlocked) return;

        dropUnlocked = true;

        Unlock("과감한 정리", "아이템을 처음 버렸습니다.");
    }

    public void QuestCompleted()
    {
        if (QeustUnlocked) return;

        QeustUnlocked = true;

        Unlock("퀘스트 해결사", "퀘스트를 처음 완료 했습니다.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (achievementPanel != null) achievementPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
            Debug.LogWarning("AchievemntManager의 UI가 연결 되어있지 않습니다.");
            yield break;
        }

        achievementPanel.SetActive(true);
        titileText.text = "업적 달성!\n" + title;
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTime);

        achievementPanel.SetActive(false);
        popupRoutine = null;
    }



}
