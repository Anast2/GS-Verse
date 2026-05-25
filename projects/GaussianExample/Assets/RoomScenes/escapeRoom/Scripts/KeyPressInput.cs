using UnityEngine;
using UnityEngine.InputSystem;
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


    //private void OnTriggerLeft(InputAction.CallbackContext _) => TryPress(rayInteractorL);
    //private void OnTriggerRight(InputAction.CallbackContext _) => TryPress(rayInteractorR);

    //private void TryPress(XRRayInteractor interactor)
    //{
    //    if (interactor == null) return;
    //    if (!interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) return;

    //    Key key = hit.collider.GetComponentInParent<Key>();
    //    if (key != null) key.Press();
    //}


    private void OnTriggerLeftStarted(InputAction.CallbackContext _)
    {
        leftHeldKey = TryGetKey(rayInteractorL);

        if (leftHeldKey != null)
            leftHeldKey.StartPress();
    }

    private void OnTriggerLeftCanceled(InputAction.CallbackContext _)
    {
        if (leftHeldKey != null)
        {
            leftHeldKey.StopPress();
            leftHeldKey = null;
        }
    }


    private void OnTriggerRightStarted(InputAction.CallbackContext _)
    {
        rightHeldKey = TryGetKey(rayInteractorR);

        if (rightHeldKey != null)
            rightHeldKey.StartPress();
    }

    private void OnTriggerRightCanceled(InputAction.CallbackContext _)
    {
        if (rightHeldKey != null)
        {
            rightHeldKey.StopPress();
            rightHeldKey = null;
        }
    }


    private Key TryGetKey(XRRayInteractor interactor)
    {
        if (interactor == null)
            return null;

        if (!interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            return null;

        return hit.collider.GetComponentInParent<Key>();
    }
}
