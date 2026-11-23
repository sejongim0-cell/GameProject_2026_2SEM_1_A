using UnityEngine;
using UnityEngine.Audio;

public class NPC : MonoBehaviour
{
    [SerializeField] private string npcName = "마을 주민";

    [TextArea]

    [SerializeField] private string message = "안녕";

    public string GetNPCName()
    {
        return npcName;
    }

    public string GetMessage()
    {
        return message;
    }

    public string GetGuideText()
    {
        return "[E]" + npcName + "와 대화 하기";
    }
}
