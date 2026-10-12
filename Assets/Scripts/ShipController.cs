using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShipController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Transform _sailTransform;

    [SerializeField, Range(0f, 100f)] private float _sailOpenness;
    [SerializeField, Range(-45f, 45f)] private float _sailRotation;

    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField, Range(0.90f, 0.999f)] private float _activeDrag = 0.98f;
    [SerializeField, Range(0.95f, 0.9999f)] private float _coastingDrag = 0.999f;
    [SerializeField] private float _hullTurnSpeed = 2f;

    private Vector3 _velocity;
    private Vector3 _originalSailScale;
    private ShipControls _inputActions;

    private Label _opennessLabel;
    private Label _rotationLabel;

    private void Awake()
    {
        InitializeUI();
        _inputActions = new ShipControls();

        _originalSailScale = _sailTransform.localScale;
    }

    private void OnEnable() => _inputActions.Gameplay.Enable();
    private void OnDisable() => _inputActions.Gameplay.Disable();

    private void Update()
    {
        ReadInput();
        UpdateSailState();
        UpdateMovement();
        UpdateUserInterface();
    }

    private void ReadInput()
    {
        float opennessInput = _inputActions.Gameplay.SailOpen.ReadValue<float>();
        float rotationInput = _inputActions.Gameplay.SailRotate.ReadValue<float>();

        _sailOpenness = Mathf.Clamp(_sailOpenness + opennessInput * 30f * Time.deltaTime, 0f, 100f);
        _sailRotation = Mathf.Clamp(_sailRotation + rotationInput * 60f * Time.deltaTime, -45f, 45f);
    }

    private void UpdateSailState()
    {
        if (_sailTransform == null) return;

        _sailTransform.localRotation = Quaternion.Euler(0f, _sailRotation, 0f);

        float opennessRatio = _sailOpenness / 100f;
        float targetScaleY = Mathf.Lerp(0.1f, _originalSailScale.y, opennessRatio);

        Vector3 newScale = _originalSailScale;
        newScale.y = targetScaleY;
        _sailTransform.localScale = newScale;
    }

    private void UpdateMovement()
    {
        if (_sailTransform == null) return;

        Vector3 windDirection = _sailTransform.forward;
        float windForce = (_sailOpenness / 100f) * _acceleration;

        _velocity += windDirection * windForce * Time.deltaTime;

        float currentDrag = _sailOpenness > 5f ? _activeDrag : _coastingDrag;
        _velocity *= currentDrag;

        if (_velocity.magnitude > _maxSpeed)
            _velocity = _velocity.normalized * _maxSpeed;

        transform.position += _velocity * Time.deltaTime;

        if (_velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_velocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _hullTurnSpeed * Time.deltaTime);
        }
    }

    private void InitializeUI()
    {
        var root = _uiDocument.rootVisualElement;
        _opennessLabel = root.Q<Label>("OpennessLabel");
        _rotationLabel = root.Q<Label>("RotationLabel");
    }

    private void UpdateUserInterface()
    {
        _opennessLabel.text = $"Sail Openness: {_sailOpenness:F0}%";

        _rotationLabel.text = $"Sail Rotation: {_sailRotation:F0}°";
    }
}