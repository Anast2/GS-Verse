using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeyPressInput : MonoBehaviour
{
    [Header("XR Setup")]
    [SerializeField] private XRRayInteractor rayInteractorL;
    [SerializeField] private XRRayInteractor rayInteractorR;

    [Header("Input")]
    [SerializeField] private InputActionReference triggerActionL;
    [SerializeField] private InputActionReference triggerActionR;

    private Key leftHeldKey;
    private Key rightHeldKey;

    void OnEnable()
    {
        if (triggerActionL != null)
        {
            triggerActionL.action.started += OnTriggerLeftStarted;
            triggerActionL.action.canceled += OnTriggerLeftCanceled;
        }
        if (triggerActionR != null)
        {
            triggerActionR.action.started += OnTriggerRightStarted;
            triggerActionR.action.canceled += OnTriggerRightCanceled;
        }
    }

    void OnDisable()
    {
        if (triggerActionL != null)
        {
            triggerActionL.action.started -= OnTriggerLeftStarted;
            triggerActionL.action.canceled -= OnTriggerLeftCanceled;
        }
        if (triggerActionR != null)
        {
            triggerActionR.action.started -= OnTriggerRightStarted;
            triggerActionR.action.canceled -= OnTriggerRightCanceled;
        }
    }


    private void OnTriggerLeftStarted(InputAction.CallbackContext _)
    {
        HandlePress(rayInteractorL, ref leftHeldKey);
    }

    private void OnTriggerRightStarted(InputAction.CallbackContext _)
    {
        HandlePress(rayInteractorR, ref rightHeldKey);
    }


    private void OnTriggerLeftCanceled(InputAction.CallbackContext _)
    {
        if (leftHeldKey != null)
        {
            leftHeldKey.StopPress();
            leftHeldKey = null;
        }
    }

    private void OnTriggerRightCanceled(InputAction.CallbackContext _)
    {
        if (rightHeldKey != null)
        {
            rightHeldKey.StopPress();
            rightHeldKey = null;
        }
    }



    private void HandlePress(XRRayInteractor interactor, ref Key heldKey)
    {
        if (interactor == null)
            return;

        if (!interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            return;

        Key key = hit.collider.GetComponentInParent<Key>();
        if (key != null)
        {
            heldKey = key;
            key.StartPress();
            return;
        }

        SafeDigit digit = hit.collider.GetComponentInParent<SafeDigit>();
        if (digit != null)
        {
            digit.Increment();
        }

        Safe safe = hit.collider.GetComponentInParent<Safe>();
        XRSimpleInteractable safeHandle = hit.collider.GetComponent<XRSimpleInteractable>();
        if (safe != null && safeHandle != null)
        {
            safe.TryOpen();
            return;
        }
    }
}
