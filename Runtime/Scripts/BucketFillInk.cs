using System.Collections.Generic;
using UnityEngine;

public class BucketFillInk : MonoBehaviour
{
    public Color inkColour = Color.white; // The colour of the ink.
    [SerializeField] BucketFillManager manager;   // Reference to the game manager.

    [SerializeField] List<BucketFillArea> areaList = new List<BucketFillArea>();

    [SerializeField] Animator animator;

    public void SetColour() { manager.SetColour(this, transform); }
    public void DestroyInk() { Destroy(gameObject); }

    public void AddArea(BucketFillArea area)
    {
        if (!areaList.Contains(area)) { areaList.Add(area); }
        else
        {
            areaList.Remove(area);

            if (areaList.Count <= 0)
            {
                manager.ClearInk(this);

                animator.SetTrigger("Clear");
            
                transform.GetChild(0).transform.position = new Vector3(100,100,100);
                transform.GetChild(0).SetParent(null);
            }
        }
    }
}