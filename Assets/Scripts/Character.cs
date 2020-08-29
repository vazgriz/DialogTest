using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    [SerializeField]
    Transform head;
    [SerializeField]
    float lookMinY;
    [SerializeField]
    float lookMaxY;

    [SerializeField]
    float speed;
    [SerializeField]
    float acceleration;

    [SerializeField]
    LayerMask interactMask;
    [SerializeField]
    float interactDistance;

    Vector2 look;
    Vector2 movementInput;
    new Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void AddLook(Vector2 delta) {
        look += delta;

        look.x = look.x % 360f;
        look.y = Mathf.Clamp(look.y, lookMinY, lookMaxY);

        head.localEulerAngles = new Vector3(-look.y, 0, 0);
        rigidbody.rotation = Quaternion.Euler(0, look.x, 0);
    }

    public void SetMovementInput(Vector2 input) {
        movementInput = Vector2.ClampMagnitude(input, 1);
    }

    public IInteractable TryGetInteractable() {
        RaycastHit hit;
        Ray ray = new Ray(head.position, head.forward);

        if (Physics.Raycast(ray, out hit, interactDistance, interactMask.value)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            return interactable;
        }

        return null;
    }

    void FixedUpdate() {
        Vector3 dir = rigidbody.rotation * new Vector3(movementInput.x, 0, movementInput.y);

        Vector3 targetVelocity = dir * speed;
        Vector3 correction = targetVelocity - rigidbody.velocity;
        correction = Vector3.ClampMagnitude(correction, Time.fixedDeltaTime * acceleration);

        rigidbody.AddForce(correction, ForceMode.VelocityChange);
    }
}
