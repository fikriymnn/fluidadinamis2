using UnityEngine;
using UnityEngine.UI;

public class TorricelliSimulation : MonoBehaviour
{
    public InputField h1Input;
    public InputField h2Input;
    public InputField vInput;
    public InputField xInput;
    public Text resultText;

    private const float g = 9.81f; // Gravity constant

    public void CalculateMissingValue()
    {
        // Parse inputs
        float h1 = ParseInput(h1Input.text);
        float h2 = ParseInput(h2Input.text);
        float v = ParseInput(vInput.text);
        float x = ParseInput(xInput.text);

        // Check for which value is missing (-1 indicates missing value)
        if (h1 == -1)
        {
            if (v != -1)
                h1 = Mathf.Pow(v, 2) / (2 * g);
            else if (x != -1 && h2 != -1)
                h1 = Mathf.Pow(x, 2) / (4 * h2);
            resultText.text = $"h1 = {h1:F2} meters";
        }
        else if (h2 == -1)
        {
            if (x != -1 && h1 != -1)
                h2 = Mathf.Pow(x, 2) / (4 * h1);
            resultText.text = $"h2 = {h2:F2} meters";
        }
        else if (v == -1)
        {
            if (h1 != -1)
                v = Mathf.Sqrt(2 * g * h1);
            resultText.text = $"v = {v:F2} m/s";
        }
        else if (x == -1)
        {
            if (h1 != -1 && h2 != -1)
                x = Mathf.Sqrt(4 * h1 * h2);
            resultText.text = $"x = {x:F2} meters";
        }
        else
        {
            resultText.text = "Please leave one field empty for calculation.";
        }
    }

    private float ParseInput(string input)
    {
        if (float.TryParse(input, out float value))
            return value;
        return -1; // Use -1 as a flag for missing value
    }
}
