using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShipController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Transform _sailTransform;

    [SerializeField, Range(0f, 100f)] private float _sailOpenness;
    [SerializeField, Range(-45f, 45f)] private float _sailRotation;

    [SerializeField] private float _opennessSpeed = 20f;
    [SerializeField] private float _rotationSpeed = 45f;

    private Label _opennessLabel;
    private Label _rotationLabel;
    private ShipControls _inputActions;

    private void Awake()
    {
        InitializeUI();
        _inputActions = new ShipControls();
    }

    private void OnEnable() => _inputActions.Gameplay.Enable();
    private void OnDisable() => _inputActions.Gameplay.Disable();

    private void Update()
    {
        float opennessInput = _inputActions.Gameplay.SailOpen.ReadValue<float>();
        float rotationInput = _inputActions.Gameplay.SailRotate.ReadValue<float>();

        UpdateSailValues(opennessInput, rotationInput);
        UpdateUserInterface();
        RotateSail();
    }

    private void InitializeUI()
    {
        var root = _uiDocument.rootVisualElement;
        _opennessLabel = root.Q<Label>("OpennessLabel");
        _rotationLabel = root.Q<Label>("RotationLabel");
    }

    private void UpdateSailValues(float opennessInput, float rotationInput)
    {
        _sailOpenness = Mathf.Clamp(_sailOpenness + opennessInput * _opennessSpeed * Time.deltaTime, 0f, 100f);
        _sailRotation = Mathf.Clamp(_sailRotation + rotationInput * _rotationSpeed * Time.deltaTime, -45f, 45f);
    }

    private void UpdateUserInterface()
    {
        if (_opennessLabel != null)
            _opennessLabel.text = $"Sail Openness: {_sailOpenness:F0}%";

        if (_rotationLabel != null)
            _rotationLabel.text = $"Sail Rotation: {_sailRotation:F0}°";
    }

    private void RotateSail()
    {
        if (_sailTransform != null)
            _sailTransform.localRotation = Quaternion.Euler(0f, _sailRotation, 0f);
    }
}