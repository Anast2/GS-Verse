using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour
{
    [SerializeField] private string _character;
    public string character => _character;

    private Keyboard _keyboard;

    private bool _isPressed = false;

    void Awake()
    {
        _keyboard = GetComponentInParent<Keyboard>();
        if (_keyboard == null)
            Debug.LogError($"Key '{name}' has no Keyboard parent in hierarchy.");
    }


    void Update()
    {
        if (_isPressed)
        {
            _keyboard.deformer.AddPressForce(transform.position, transform.forward);
        }
    }

    public void StartPress()
    {
        if (_isPressed) return;

        _isPressed = true;

        _keyboard.RegisterKeyPress(_character);
    }

    public void StopPress()
    {
        _isPressed = false;
    }
}
