using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class toricelli : MonoBehaviour
{
    public TMP_InputField h1Input;
    public TMP_InputField h2Input;
    public TMP_InputField vInput;
    public TMP_InputField xInput;
    public TextMeshProUGUI resultText;

    [Header("Input Numbers")]
    public float valueA = 5f; // First number
    public float valueB = 5f; // Second number (total = valueA + valueB)

    [Header("Block Settings")]
    public Transform block; // Reference to the block
    public Transform objectToMove; // Reference to the object to move

    [Header("Particle System")]
    public ParticleSystem waterParticleSystem; // Particle system to simulate water

    private float blockHeight;
    private const float g = 9.81f;

    // public void CalculateMissingValue()
    // {
    //     // Parse inputs
    //     float h1 = ParseInput(h1Input.text);
    //     float h2 = ParseInput(h2Input.text);
    //     float v = ParseInput(vInput.text);
    //     float x = ParseInput(xInput.text);
    //     valueA = h2;
    //     valueB = h1;

    //     // Check for which value is missing (-1 indicates missing value)
    //     if (h1 == -1)
    //     {
    //         if (v != -1)
    //             h1 = Mathf.Pow(v, 2) / (2 * g);
    //         else if (x != -1 && h2 != -1)
    //             h1 = Mathf.Pow(x, 2) / (4 * h2);
    //         resultText.text = $"h1 = {h1:F2} meters";
    //     }
    //     else if (h2 == -1)
    //     {
    //         if (x != -1 && h1 != -1)
    //             h2 = Mathf.Pow(x, 2) / (4 * h1);
    //         resultText.text = $"h2 = {h2:F2} meters";
    //     }
    //     else if (v == -1)
    //     {
    //         if (h1 != -1)
    //             v = Mathf.Sqrt(2 * g * h1);
    //         resultText.text = $"v = {v:F2} m/s";
    //     }
    //     else if (x == -1)
    //     {
    //         if (h1 != -1 && h2 != -1)
    //             x = Mathf.Sqrt(4 * h1 * h2);
    //         resultText.text = $"x = {x:F2} meters";
    //     }
    //     else
    //     {
    //         resultText.text = "Please leave one field empty for calculation.";
    //     }

    //     SimulateWaterParticles(h1, h2, x);
    //     MoveObject();
    // }
    public void CalculateMissingValue()
    {
        // Parse inputs
        float h1 = ParseInput(h1Input.text);
        float h2 = ParseInput(h2Input.text);
        float v = ParseInput(vInput.text);
        float x = ParseInput(xInput.text);
        valueA = h2;
        valueB = h1;

        string output = ""; // Collect output text here

        // Check if any value is missing
        bool isAnyValueMissing = h1 == -1 || h2 == -1 || v == -1 || x == -1;

        if (h1 == -1)
        {
            if (v != -1)
                h1 = Mathf.Pow(v, 2) / (2 * g);
            else if (x != -1 && h2 != -1)
                h1 = Mathf.Pow(x, 2) / (4 * h2);

            output += $"h1 = {h1:F2} meters\n";
        }
        if (h2 == -1)
        {
            if (x != -1 && h1 != -1)
                h2 = Mathf.Pow(x, 2) / (4 * h1);

            float h2WithCrane = h2 + blockHeight;
            output += $"h2 = {h2:F2} meters\n";
            output += $"h2 with crane = {h2WithCrane:F2} meters\n";
        }
        if (v == -1)
        {
            if (h1 != -1)
                v = Mathf.Sqrt(2 * g * h1);

            output += $"v = {v:F2} m/s\n";
        }
        if (x == -1)
        {
            if (h1 != -1 && h2 != -1)
                x = Mathf.Sqrt(4 * h1 * h2);

            output += $"x = {x:F2} meters\n";
        }

        // Recalculate x with crane height if h1 and h2 are valid
        if (h1 != -1 && h2 != -1)
        {
            float h2WithCrane = h2 + blockHeight;
            float xWithCrane = Mathf.Sqrt(4 * h1 * h2WithCrane);
            output += $"x with crane = {xWithCrane:F2} meters\n";
        }

        // If no value is missing, ensure v and x are calculated
        if (!isAnyValueMissing)
        {
            v = Mathf.Sqrt(2 * g * h1);
            x = Mathf.Sqrt(4 * h1 * h2);
            output += $"v = {v:F2} m/s\n";
            output += $"x = {x:F2} meters\n";
        }

        if (string.IsNullOrEmpty(output))
        {
            output = "Please leave one field empty for calculation.";
        }

        resultText.text = output; // Set output text

        Debug.Log($"Output Text: {output}"); // Debug the output
        SimulateWaterParticles(h1, h2, x);
        MoveObject();
    }



    private float ParseInput(string input)
    {
        if (float.TryParse(input, out float value))
            return value;
        return -1; // Use -1 as a flag for missing value
    }

    void Start()
    {
        // Calculate the height of the block (assuming it's a 3D object with a collider)
        if (block.TryGetComponent(out Renderer renderer))
        {
            blockHeight = renderer.bounds.size.y;
        }
        else
        {
            Debug.LogError("Block does not have a renderer to calculate height!");
            return;
        }

        MoveObject();
    }

    void MoveObject()
    {
        if (block == null || objectToMove == null)
        {
            Debug.LogError("Block or Object to Move is not assigned!");
            return;
        }

        // Calculate the percentage
        float total = valueA + valueB;
        if (total == 0)
        {
            Debug.LogError("Total cannot be zero!");
            return;
        }

        float percentage = valueA / total; // 50% = 0.5

        // Calculate the new Y position
        Vector3 blockPosition = block.position;
        Vector3 newPosition = objectToMove.position;

        newPosition.y = blockPosition.y + (blockHeight * percentage); // Move up by percentage of block height

        // Apply the new position
        objectToMove.position = newPosition;

        Debug.Log($"Object moved to {percentage * 100}% of block height.");

        // Adjust particle force based on percentage
        AdjustParticleForce(percentage);
    }

    private void SimulateWaterParticles(float h1, float h2, float x)
    {
        if (waterParticleSystem == null) return;

        // Get the Force over Lifetime module
        var forceOverLifetime = waterParticleSystem.forceOverLifetime;
        forceOverLifetime.enabled = true;

        // Set the Y force (this will be adjusted based on percentage in MoveObject)
        forceOverLifetime.x = new ParticleSystem.MinMaxCurve(0); // Keep X force constant (no horizontal movement)
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(0); // We'll update this value in AdjustParticleForce
    }

    private void AdjustParticleForce(float percentage)
    {
        // Map percentage (0 to 1) to force range (4 to -30) for Y force
        float forceY = Mathf.Lerp(4f, -30f, percentage);

        // Map percentage (0 to 1) to speed range (10 to 3) for start speed
        float startSpeed = Mathf.Lerp(10f, 3f, percentage);

        // Get the Force over Lifetime module
        var forceOverLifetime = waterParticleSystem.forceOverLifetime;
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(forceY);

        // Set the start speed of the particle system
        var mainModule = waterParticleSystem.main;
        mainModule.startSpeed = startSpeed;

        Debug.Log($"Particle Y force set to: {forceY}, Start speed set to: {startSpeed}");
    }

}
