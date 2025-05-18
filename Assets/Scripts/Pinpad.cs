using UnityEngine;
using TMPro;

public class Pinpad : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FirstDigit;
    [SerializeField] TextMeshProUGUI SecondDigit;
    [SerializeField] TextMeshProUGUI ThirdDigit;

    private string password = "   ";
    public string answer;
    public bool unlocked;

    void Start()
    {
        if (answer.Length != 3 || !int.TryParse(answer, out _))
        {
            Debug.LogError("A senha deve ter exatamente 3 dígitos.");
        }
        gameObject.SetActive(false);
    }

    public int GetPressedNumber()
    {
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()) | Input.GetKeyDown(KeyCode.Keypad0 + number))
            {
                return number;
            }
        }
        return -1;
    }

    public void CheckPassword()
    {
        string attempt = password[^3..];
        HidePinpad();
        if (unlocked)
        {
            ThoughtManager.ShowThought("A porta já está aberta, por que eu coloquei a senha de novo?...");   
        }
        else if (attempt == answer)
        {
            unlocked = true;
            ThoughtManager.ShowThought("A porta se abriu!");
        }
        else
        {
            ThoughtManager.ShowThought("A porta continua trancada, acho que essa não é a senha correta...");
        }
    }

    public void UpdateDisplay()
    {
        FirstDigit.text = password[^1].ToString();
        SecondDigit.text = password[^2].ToString();
        ThirdDigit.text = password[^3].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int number = GetPressedNumber();
        if (number != -1 && password.Length < 6)
        {
            password += number.ToString();
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && password.Length > 3)
        {
            password = password.Remove(password.Length - 1);
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPassword();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            HidePinpad();
        }
    }

    public void PressButton(string value)
    {
        if (value == "Erese" && password.Length > 3)
        {
            password = password.Remove(password.Length - 1);
            UpdateDisplay();
        }
        else if (value == "Confirm")
        {
            CheckPassword();
        }
        else if (int.TryParse(value, out _) && value.Length == 1)
        {
            if (password.Length < 6)
            {
                password += value;
                UpdateDisplay();
            }
        }
        else
        {
            Debug.LogError("Valor inválido para o botão do PinPad: " + value);
        }
    }

    public void DisplayPinpad()
    {
        PlayerPrefs.SetString("CurrentUI", "Pinpad");
        gameObject.SetActive(true);
        password = "   ";
        UpdateDisplay();
    }

    public void HidePinpad()
    {
        PlayerPrefs.SetString("CurrentUI", "None");
        gameObject.SetActive(false);
        password = "   ";
        UpdateDisplay();
    }
}
