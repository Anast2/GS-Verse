using GaussianSplatting.Shared;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private string targetSequence = "233";
    [SerializeField] private bool caseSensitive = false;

    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;

    public GameObject ObjectRequiredToBeActiveForCodeToWork = null;

    //[SerializeField]
    public IDeformable deformer;// needs to be accessible for keys

    //public event Action OnSequenceMatched;
    [SerializeField] private UnityEvent onCorrectCode;

    private readonly StringBuilder _buffer = new StringBuilder();
    private string _normalizedTarget;

    void Awake()
    {
        _normalizedTarget = caseSensitive ? targetSequence : targetSequence.ToLowerInvariant();

        deformer = GetComponentInChildren<IDeformable>();
    }

    public void RegisterKeyPress(string ch)
    {
        if (string.IsNullOrEmpty(ch) || string.IsNullOrEmpty(_normalizedTarget) || (ObjectRequiredToBeActiveForCodeToWork != null && !ObjectRequiredToBeActiveForCodeToWork.activeSelf)) return;

        string normalized = caseSensitive ? ch : ch.ToLowerInvariant();
        _buffer.Append(normalized);

        if (_buffer.Length > _normalizedTarget.Length)
            _buffer.Remove(0, _buffer.Length - _normalizedTarget.Length);

        if (_buffer.Length == _normalizedTarget.Length && _buffer.ToString() == _normalizedTarget)
        {
            HandleSequenceMatched();
            _buffer.Clear();
        }
    }

    private void HandleSequenceMatched()
    {
        Debug.Log($"[Keyboard] Sequence '{_normalizedTarget}' entered!");
        redLight.SetActive(false);
        greenLight.SetActive(true);
        onCorrectCode?.Invoke();
    }
}
