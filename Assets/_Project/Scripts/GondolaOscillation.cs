using UnityEngine;

public class GondolaOscillator : MonoBehaviour
{
  [Header("Bobbing Settings")]
  [SerializeField] private float bobFrequency = 1.0f;
  [SerializeField] private float bobAmplitude = 0.1f;

  [Header("Rolling Settings")]
  [SerializeField] private float rollFrequency = 0.5f;
  [SerializeField] private float rollAmplitude = 2.0f;

  [Header("Ground Settings")]
  [SerializeField] private LayerMask waterLayer;
  [SerializeField] private float raycastDistance = 2f;
  [SerializeField] private float heightOffset = 2f;

  private float timeOffset;
  private float baseHeight = 2f;
  private Transform gondolaTransform;
  private Rigidbody gondolaRigidbody;

  private void Awake()
  {
    gondolaTransform = transform.root;
    gondolaRigidbody = gondolaTransform.GetComponent<Rigidbody>();
  }

  private void Start()
  {
    timeOffset = Random.Range(0f, 2f * Mathf.PI);
    UpdateBaseHeight();
  }

  private void Update()
  {
    UpdateBaseHeight();
    ApplyOscillation();
  }

  private void UpdateBaseHeight()
  {
    if (Physics.Raycast(gondolaTransform.position, Vector3.down, out RaycastHit hit, raycastDistance, waterLayer))
    {
      baseHeight = hit.point.y + heightOffset;
    }
  }

  private void ApplyOscillation()
  {
    float time = Time.time + timeOffset;

    // Apply bobbing to parent
    float bobHeight = Mathf.Sin(time * bobFrequency) * bobAmplitude;
    Vector3 newPosition = gondolaTransform.position;
    newPosition.y = baseHeight + bobHeight;
    gondolaTransform.position = newPosition;

    // Apply roll effect locally to this object
    float roll = Mathf.Sin(time * rollFrequency) * rollAmplitude;
    gondolaRigidbody.MoveRotation(Quaternion.Euler(0, 0, roll));
  }
}