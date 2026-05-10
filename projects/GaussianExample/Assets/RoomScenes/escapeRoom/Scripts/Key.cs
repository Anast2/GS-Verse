using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour
{
    [SerializeField] private string _character;
    public string character => _character;

    private Keyboard _keyboard;

    void Awake()
    {
        _keyboard = GetComponentInParent<Keyboard>();
        if (_keyboard == null)
            Debug.LogError($"Key '{name}' has no Keyboard parent in hierarchy.");
    }

    public void Press()
    {
        if (_keyboard != null)
            _keyboard.RegisterKeyPress(_character);
    }
}
