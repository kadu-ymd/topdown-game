using UnityEngine;
using TMPro;

public class Password : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FirstDigit;
    [SerializeField] TextMeshProUGUI SecondDigit;
    [SerializeField] TextMeshProUGUI ThirdDigit;
    private int position;
    private string answer;
    private bool correct;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     answer = "123";   
    }

    public int GetPressedNumber() 
    {
        for (int number = 0; number <= 9; number++) 
        {
            if (Input.GetKeyDown(number.ToString()) | Input.GetKeyDown(KeyCode.Keypad0+number))
            {
                return number;
            }
        }
        return -1;
    }

    public void testPassword()
    {
        string test =  ThirdDigit.text + SecondDigit.text + FirstDigit.text;
        if (test == answer) {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }

    public void UpdatePassword(int position, string value)
    {
        if (position == 0) {
            FirstDigit.text = value;
        }
        else if (position == 1) {
            SecondDigit.text = value;
        }
        else {
            ThirdDigit.text = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int number = GetPressedNumber();
        if (number != -1 && position < 3)
        {
            UpdatePassword(position, number.ToString());
            position++;
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && position > 0)
        {
            position--;
            UpdatePassword(position, "");
        }
        else if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            testPassword();
        }
    }
}
