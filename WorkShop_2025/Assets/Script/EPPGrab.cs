using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.Hands;

public class EPPGrab : MonoBehaviour
{
    public enum EPPType {Head, Chest, Hands, Foots };
    public EPPType eppType;

    [Header("Settings")]

    public float distance = 0.2f;

    bool isEquipped = false;
    XRGrabInteractable interactable;
    Transform bodySlot;

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        FindBodySlot();
    }

    private void Update()
    {
        if (isEquipped || interactable.isSelected == false) return;
        {
            float dist = Vector3.Distance(transform.position, bodySlot.position);
            if(dist < distance)
            {
                EquipEPP();
            }
        }
    }

    public void FindBodySlot()
    {
        string slotName = "Slot_" + eppType.ToString();
        GameObject slotObj = GameObject.Find(slotName);

        if (slotObj != null)
        {
            bodySlot = slotObj.transform;
        }
        else
        {
            Debug.LogWarning("No Hay Slot Para Equiparse " + eppType);
        }
    }

    public void EquipEPP()
    {
        isEquipped = true;
        interactable.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.SetParent(bodySlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
