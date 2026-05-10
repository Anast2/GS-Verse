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

    void OnEnable()
    {
        if (triggerActionL != null) triggerActionL.action.started += OnTriggerLeft;
        if (triggerActionR != null) triggerActionR.action.started += OnTriggerRight;
    }

    void OnDisable()
    {
        if (triggerActionL != null) triggerActionL.action.started -= OnTriggerLeft;
        if (triggerActionR != null) triggerActionR.action.started -= OnTriggerRight;
    }

    private void OnTriggerLeft(InputAction.CallbackContext _) => TryPress(rayInteractorL);
    private void OnTriggerRight(InputAction.CallbackContext _) => TryPress(rayInteractorR);

    private void TryPress(XRRayInteractor interactor)
    {
        if (interactor == null) return;
        if (!interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) return;

        Key key = hit.collider.GetComponentInParent<Key>();
        if (key != null) key.Press();
    }
}
