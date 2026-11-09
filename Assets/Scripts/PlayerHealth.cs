using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int startingHealth = 50;
    [SerializeField] private TextMeshProUGUI healthText;

    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(startingHealth, 0, maxHealth);
        UpdateHealthText();
    }
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)        //최대 체력 이상으로 수치가 올라갈 경우
        {
            currentHealth = maxHealth;       //최대로 설정
        }

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP " + currentHealth + "/" + maxHealth;
    }
}
