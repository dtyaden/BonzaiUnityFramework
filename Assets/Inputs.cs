using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs {
    private Dictionary<string, List<string>> inputMappings;
	// Use this for initialization
	public Inputs()
    {
        inputMappings = new Dictionary<string, List<string>>();
    }
	/// <summary>
    /// Add an additional input for the input axis to handle.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="unityInputName"></param>
	public void addMapping(string input, string unityInputName)
    {
        List<string> presentMappings;
        inputMappings.TryGetValue(input, out presentMappings);
        if(presentMappings == null)
        {
            presentMappings = new List<string>();
            presentMappings.Add(unityInputName);
            inputMappings.Add(input, presentMappings);
        }
        else if (!presentMappings.Contains(unityInputName))
        {
            presentMappings.Add(unityInputName);
        }
    }

    public float getAxis(string inputName)
    {
        List<string> unityInputValues;
        inputMappings.TryGetValue(inputName, out unityInputValues);
        float cumulativeAxisInput = 0;
        foreach(string axis in unityInputValues)
        {
            cumulativeAxisInput += Input.GetAxisRaw(axis);
        }
        return cumulativeAxisInput;
    }

    public bool getButton(string buttonName)
    {
        List<string> unityInputValues;
        inputMappings.TryGetValue(buttonName, out unityInputValues);
        foreach(string input in unityInputValues)
        {
            bool buttonDown = Input.GetButton(input);
            if (buttonDown)
            {
                return buttonDown;
            }
        }
        return false;
    }
}
