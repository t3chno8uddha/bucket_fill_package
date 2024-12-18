using UnityEngine;

public class BucketFillArea : MonoBehaviour
{
    [SerializeField] BucketFillManager manager;   // Reference to the game manager.

    [SerializeField] BucketFillInk ink;

    // Gameplay components.
    SpriteRenderer areaRenderer;

    MaterialPropertyBlock matPropBlock;

    bool isFilled = false;
    bool isFilling = false;

    void Start()
    {
        matPropBlock = new MaterialPropertyBlock();

        // Get the necessary components.
        areaRenderer = GetComponent<SpriteRenderer>();

        ink.AddArea(this);
    }

    void Update()
    {
        // Check when to stop filling the object.
        if (isFilled)
        {
            if (!isFilling) isFilling = true;

            // Get the property block.
            areaRenderer.GetPropertyBlock(matPropBlock);

            // Fetch scale and lerp it up.
            float scale = matPropBlock.GetFloat("_Scale");
            matPropBlock.SetFloat("_Scale", Mathf.Lerp(scale, 1, manager.smoothness * Time.deltaTime));

            // Check if we're done.
            if (scale >= 0.5f)
            {
                matPropBlock.SetFloat("_Scale", 1);

                isFilling = false;
            }

            // Apply the block.
            areaRenderer.SetPropertyBlock(matPropBlock);
        }
    }

    public void ColourArea(BucketFillInk newInk)
    {
        if (ink == newInk)
        {
            if (!isFilled)
            {
                ink.AddArea(this);

                // Toggle the flag.
                isFilled = true;

                // Again, get the property block.
                areaRenderer.GetPropertyBlock(matPropBlock);

                // Set the scale, and input position.
                matPropBlock.SetFloat("_Scale", 0);
                matPropBlock.SetVector("_Position", Camera.main.ScreenToViewportPoint(Input.mousePosition));

                // Set the new colour.
                matPropBlock.SetColor("_Fill_Colour", ink.inkColour);

                // Apply the property block.
                areaRenderer.SetPropertyBlock(matPropBlock);
            }
        }
    }
}