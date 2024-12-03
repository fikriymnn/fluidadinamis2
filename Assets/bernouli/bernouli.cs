using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bernouli : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField area1Input;  // Input field for A1
    public TMP_InputField area2Input;  // Input field for A2
    public TMP_InputField height1Input;  // Input field for H1
    public TMP_InputField height2Input;  // Input field for H2
    public TMP_InputField initialVelocityInput;  // Input field for initial velocity (v1)

    [Header("Output Fields")]
    public TMP_Text velocity1Output;  // Display for velocity1
    public TMP_Text velocity2Output;  // Display for velocity2

    [Header("Constants")]
    public float fluidDensity = 1000f;  // Fluid density (water = 1000 kg/m^3)
    public float gravity = 9.81f;  // Gravitational acceleration

    [Header("Objects to Scale")]
    public Transform object1;  // Object affected by area1Input
    public Transform object2;
    public Transform l1;
    public Transform l2;  // Object affected by area2Input

    public LineRenderer line; // Reference to the LineRenderer component

    void Start()
    {
        if (line == null)
        {
            Debug.LogError("LineRenderer reference is missing! Please assign it in the Inspector.");
            return;
        }

        line.positionCount = 2;
    }

    void Update()
    {
        if (line != null && object1 != null && object2 != null)
        {
            line.SetPosition(0, l1.position);
            line.SetPosition(1, l2.position);
        }
    }

    void UpdateLineWidth()
    {
        float scale1Value = 1f;
        float scale2Value = 1f;

        if (float.TryParse(area1Input.text, out float parsedScale1))
        {
            scale1Value = parsedScale1 * 0.03280194f; // Convert scale1 to the new scale
        }
        if (float.TryParse(area2Input.text, out float parsedScale2))
        {
            scale2Value = parsedScale2 * 0.03280194f; // Convert scale2 to the new scale
        }

        // Update the LineRenderer width using an AnimationCurve
        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0f, scale1Value); // Start width
        widthCurve.AddKey(1f, scale2Value); // End width

        line.widthCurve = widthCurve;
    }


    private void UpdateScales()
    {
        if (float.TryParse(area1Input.text, out float scale1Value))
        {
            if (object1 != null)
            {
                Vector3 scale1 = object1.localScale;
                scale1.x = scale1Value;
                scale1.z = scale1Value;
                object1.localScale = scale1;
            }
        }

        if (float.TryParse(area2Input.text, out float scale2Value))
        {
            if (object2 != null)
            {
                Vector3 scale2 = object2.localScale;
                scale2.x = scale2Value;
                scale2.z = scale2Value;
                object2.localScale = scale2;
            }
        }
    }

    void UpdatePositions()
    {
        float height1Value = 0f, height2Value = 0f;

        if (float.TryParse(height1Input.text, out float parsedHeight1))
        {
            height1Value = parsedHeight1;
        }

        if (float.TryParse(height2Input.text, out float parsedHeight2))
        {
            height2Value = parsedHeight2;
        }

        // Determine which object is lower
        float minHeight = Mathf.Min(height1Value, height2Value);
        float heightDifference = Mathf.Abs(height1Value - height2Value);

        if (object1 != null && object2 != null)
        {
            if (height1Value < height2Value)
            {
                object1.localPosition = new Vector3(object1.localPosition.x, 0, object1.localPosition.z);
                object2.localPosition = new Vector3(object2.localPosition.x, heightDifference, object2.localPosition.z);
            }
            else
            {
                object2.localPosition = new Vector3(object2.localPosition.x, 0, object2.localPosition.z);
                object1.localPosition = new Vector3(object1.localPosition.x, heightDifference, object1.localPosition.z);
            }
        }
    }


    public void CalculateVelocities()
    {
        if (!float.TryParse(area1Input.text, out float area1) || area1 <= 0)
        {
            velocity1Output.text = "Invalid A1";
            velocity2Output.text = "Invalid A1";
            return;
        }
        if (!float.TryParse(area2Input.text, out float area2) || area2 <= 0)
        {
            velocity1Output.text = "Invalid A2";
            velocity2Output.text = "Invalid A2";
            return;
        }
        if (!float.TryParse(height1Input.text, out float height1))
        {
            velocity1Output.text = "Invalid H1";
            velocity2Output.text = "Invalid H1";
            return;
        }
        if (!float.TryParse(height2Input.text, out float height2))
        {
            velocity1Output.text = "Invalid H2";
            velocity2Output.text = "Invalid H2";
            return;
        }
        if (!float.TryParse(initialVelocityInput.text, out float initialVelocity))
        {
            initialVelocity = 0;
        }

        float heightDifference = height1 - height2;
        float v1 = initialVelocity;

        float v2Squared = v1 * v1 + 2 * gravity * heightDifference;

        if (v2Squared < 0)
        {
            velocity1Output.text = "Error: Unrealistic inputs.";
            velocity2Output.text = "Error: Unrealistic inputs.";
            return;
        }

        float velocity1 = v1;
        float velocity2 = Mathf.Sqrt(v2Squared);

        velocity1Output.text = $"v1: {velocity1:F2} m/s";
        velocity2Output.text = $"v2: {velocity2:F2} m/s";

        UpdateScales();
        UpdateLineWidth();
        UpdatePositions();
    }
}
