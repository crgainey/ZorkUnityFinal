using System;
using UnityEngine;
using TMPro;
using Zork;
using UnityEngine.UI;

public class UnityInputService : MonoBehaviour, IInputService
{
    [SerializeField]
    InputField InputField;

    public event EventHandler<string> InputRecieved;

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (string.IsNullOrWhiteSpace(InputField.text) == false)
            {
                string inputString = InputField.text.Trim().ToUpper();
                InputRecieved?.Invoke(this, inputString);
                InputField.Select();
                InputField.ActivateInputField();
            }

            InputField.text = string.Empty;

        }
    }

}
