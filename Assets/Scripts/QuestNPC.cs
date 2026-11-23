using UnityEngine;
using UnityEngine.InputSystem;

public class QuestNPC : MonoBehaviour
{
    [SerializeField]private QuestManager questManager;
    [SerializeField]private Quest quest;

    private bool playerInNear;


    // Update is called once per frame
    void Update()
    {
        if (playerInNear == false) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if(keyboard.eKey.wasPressedThisFrame)
        {
            questManager.TalkToNPC(quest);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInNear = true;
            Debug.Log("E 키 눌러 대화");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInNear = false;
        }
    }

}
