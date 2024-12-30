using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketFillManager : MonoBehaviour
{
    // The selected colour in which the drawing will be filled.
    private BucketFillInk ink;

    [SerializeField] private Transform selectionObject;
    public float smoothness = 2f;

    [SerializeField] private Transform outlineCursor;

    [SerializeField] private List<BucketFillInk> inkSelection = new List<BucketFillInk>();

    [SerializeField] private Button homeButton;
    private BucketFillEntryPoint _entryPoint;

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
        // Handle both touch and mouse inputs
        HandleInput();
    }

    private void HandleInput()
    {
        Vector3 inputPosition = Vector3.zero;
        bool inputDetected = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPosition = Camera.main.ScreenToWorldPoint(touch.position);
            inputPosition.z = 0;
            inputDetected = touch.phase == TouchPhase.Began;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosition.z = 0;
            inputDetected = true;
        }

        if (inputDetected && ink != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);
            if (hit.collider != null)
            {
                BucketFillArea areaObject = hit.collider.gameObject.GetComponent<BucketFillArea>();

                if (areaObject)
                {
                    areaObject.ColourArea(ink);
                }
            }
        }
    }

    public void ClearInk(BucketFillInk fillInk)
    {
        inkSelection.Remove(fillInk);

        if (inkSelection.Count <= 0)
        {
            Debug.Log("Game Over!");
            SetFinishForPackage();
        }
    }

    public void SetEntryPoint(BucketFillEntryPoint entryPoint)
    {
        _entryPoint = entryPoint;
    }

    private void SetFinishForPackage()
    {
        StartCoroutine(FinishAfterFireworks());
    }

    private IEnumerator FinishAfterFireworks()
    {
        yield return new WaitForSeconds(5f);
        _entryPoint.InvokeGameFinished();
    }
}
