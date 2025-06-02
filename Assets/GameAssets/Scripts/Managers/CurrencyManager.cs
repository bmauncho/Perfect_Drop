using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [Header("Currency Settings")]
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private float cashAmount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cashAmount = 500;
        UpdateCurrencyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCash(float Amount )
    {
        cashAmount += Amount;
        UpdateCurrencyText();
    }


    public void RemoveCash ( float Amount )
    {
        if (cashAmount >= Amount)
        {
            cashAmount -= Amount;
            if(cashAmount<= 0)
            {
                cashAmount = 0;
            }
            UpdateCurrencyText();
        }
        else
        {
            Debug.LogWarning("Not enough cash to remove.");
        }
    }

    public void UpdateCurrencyText ()
    {
        if (currencyText != null)
        {
            string formattedCash = cashAmount % 1 == 0 ? cashAmount.ToString("N0") : cashAmount.ToString("N2");
            currencyText.text = formattedCash;
        }
        else
        {
            Debug.LogWarning("Currency Text is not assigned.");
        }
    }

    public float GetCashAmount ()
    {
        return cashAmount;
    }
}
