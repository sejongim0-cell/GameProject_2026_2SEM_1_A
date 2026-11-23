using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class InteractionUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guiddeText;

    [SerializeField] private GameObject DialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private float dialogueDuration = 3.0f;

    private Coroutine dialogueRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideGuide();
        DialoguePanel.SetActive(false);
    }

    public void ShowGuide(string Message)
    {
        guiddeText.gameObject.SetActive(true);
        guiddeText.text = Message;
    }

    public void HideGuide()
    {
        guiddeText.gameObject.SetActive(false);
    }

    private IEnumerator DialogueRoutine(string name,string Message)
    {
        DialoguePanel.SetActive(true);
        dialogueText.text = name + "\n" + Message;

        yield return new WaitForSeconds(dialogueDuration);

        DialoguePanel.SetActive(false);
        dialogueRoutine = null;
    }

    public void ShowDialogue(string Name,string Message)
    {
        if (dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine);
        }

        dialogueRoutine = StartCoroutine(DialogueRoutine(Name, Message));
    }

}
