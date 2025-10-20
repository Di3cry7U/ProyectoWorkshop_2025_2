using System.Collections;
using UnityEngine;

public class CleanTool : MonoBehaviour
{
    public float cleanAmount = .34f;
    bool canClean = true;
    public float cleanCooldownTime = .5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!canClean) return;

        var cleanable = other.GetComponent<CleanableObject>();
        if(cleanable != null)
        {
            cleanable.Clean(cleanAmount);
            StartCoroutine(CleanCooldown());
        }
    }

    IEnumerator CleanCooldown()
    {
        canClean = false;
        yield return new WaitForSeconds(cleanCooldownTime);
        canClean = true;
    }
}
