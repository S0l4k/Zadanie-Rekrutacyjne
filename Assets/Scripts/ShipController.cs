using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Label _opennessLabel;
    [SerializeField] private Label _rotationLabel;
    [SerializeField] private UIDocument _uiDocument;

    [SerializeField, Range(0f, 100f)] private float _sailOpenness = 0f;
    [SerializeField, Range(-45f, 45f)] private float _sailRotation = 0f;

    [SerializeField] private Transform _sailTransform;

    [SerializeField] private float _opennessSpeed = 20f;
    [SerializeField] private float _rotationSpeed = 45f;

    private float _opennessInput;
    private float _rotationInput;

    private void Awake()
    {
        InitializeUI();
    }


    public void OnSailOpen(InputAction.CallbackContext context)
    {
        _opennessInput = context.ReadValue<float>();
    }

    public void OnSailRotate(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<float>();
    }

    private void InitializeUI()
    {
        var root = _uiDocument.rootVisualElement;
        _opennessLabel = root.Q<Label>("OpennessLabel");
        _rotationLabel = root.Q<Label>("RotationLabel");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSailValues();
        UpdateUserInterface();
        RotateSail();
    }

    private void UpdateSailValues()
    {
        _sailOpenness += _opennessInput * _opennessSpeed * Time.deltaTime;

        _sailOpenness = Mathf.Clamp(_sailOpenness, 0f, 100f);

        _sailRotation += _rotationInput * _rotationSpeed * Time.deltaTime;

        _sailRotation = Mathf.Clamp(_sailRotation, -45f, 45f);
    }


    private void UpdateUserInterface()
    {
        _opennessLabel.text = $"Sail Openness: {_sailOpenness:F0}%";
        _rotationLabel.text = $"Sail Rotation: {_sailRotation:F0}°";
    }

    private void RotateSail()
    {   
            _sailTransform.localRotation = Quaternion.Euler(0f, _sailRotation, 0f);
    }
}
