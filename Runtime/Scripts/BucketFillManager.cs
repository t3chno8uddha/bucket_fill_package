using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketFillManager : MonoBehaviour
{
    // The selected colour in which the drawing will be filled.
    BucketFillInk ink;

    [SerializeField] Transform selectionObject;
    public float smoothness = 2f;

    [SerializeField] Transform outlineCursor;

    [SerializeField] List<BucketFillInk> inkSelection = new List<BucketFillInk>();

    [SerializeField] Button homeButton;
    BucketFillEntryPoint _entryPoint;

    void Awake()
    {
        homeButton.onClick.AddListener(FinishOnButton);
    }

    public void FinishOnButton()
    {
        _entryPoint.InvokeGameFinished();
    }

    // Methods for setting and returning the selected colour.
    public void SetColour(BucketFillInk colour, Transform parent)
    {
        ink = colour;

        outlineCursor.SetParent(parent);
        outlineCursor.localPosition = Vector3.zero;
    }

    public Color GetColour() { return ink.inkColour; }

    void Update()
    {
        // Check if a touch event is happening.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch input.

            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0; // Set z to 0 to align with 2D.

            if (touch.phase == TouchPhase.Began)
            {
                if (ink)
                {
                    // Raycast to check if the touch is on this piece.
                    RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                    if (hit.collider != null)
                    {
                        BucketFillArea areaObject = hit.collider.gameObject.GetComponent<BucketFillArea>();

                        if (areaObject) { areaObject.ColourArea(ink); }
                    }
                }
            }
        }
    }

    public void ClearInk(BucketFillInk fillInk)
    {
        inkSelection.Remove(fillInk);

        if (inkSelection.Count <= 0)
        {
            print("Game Over!");
        }
    }

    public void SetEntryPoint(BucketFillEntryPoint entryPoint)
    {
        _entryPoint = entryPoint;
    }

    void SetFinishForPackage()
    {
        StartCoroutine (FinishAfterFireworks());
    }

    IEnumerator FinishAfterFireworks()
    {
        yield return new WaitForSeconds(5f);

        _entryPoint.InvokeGameFinished();
    }
}