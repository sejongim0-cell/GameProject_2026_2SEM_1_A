using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private float openAngle = 90f;

    [SerializeField]private float openSpeed = 3f;

    private bool isOpen;

    [SerializeField]private Quaternion closedRotaiton;
    [SerializeField]private Quaternion openRoatation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRotaiton = transform.localRotation;
        openRoatation = closedRotaiton * Quaternion.Euler(0,openAngle, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation;

        if (isOpen)
        {
            targetRotation = openRoatation;
        }
        else
        {
            targetRotation = closedRotaiton;
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, openSpeed * Time.deltaTime);
    }

    public void interact()
    {
        isOpen = !isOpen;
    }

    public string GetGuideText()
    {
        if(isOpen)
        {
            return "[E] 문 닫기";
        }
        return "[E] 문 열기";
    }
}
