using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    Character character;
    bool locked = false;

    [SerializeField]
    float mouseSensitivity;
    [SerializeField]
    UnityEngine.UI.Text interactionLabel;

    IInteractable interactionTarget;

    bool inDialog;

    void Start() {
        character = GetComponent<Character>();
    }

    void Update() {
        interactionTarget = character.TryGetInteractable();

        if (inDialog || interactionTarget == null) {
            interactionLabel.gameObject.SetActive(false);
            interactionLabel.text = "";
        } else {
            interactionLabel.gameObject.SetActive(true);
            interactionLabel.text = interactionTarget.GetName();
        }
    }

    public void SetMouseDelta(InputAction.CallbackContext context) {
        if (locked) {
            character.AddLook(context.ReadValue<Vector2>() * mouseSensitivity);
        }
    }

    public void Lock() {
        if (inDialog) return;

        locked = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Unlock() {
        locked = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetInDialog(bool value) {
        inDialog = value;

        if (inDialog) {
            Unlock();
        } else {
            Lock();
        }
    }

    public void Interact(InputAction.CallbackContext context) {
        if (context.performed && interactionTarget != null) {
            interactionTarget.Interact();
        }
    }

    public void SetMovementInput(InputAction.CallbackContext context) {
        character.SetMovementInput(context.ReadValue<Vector2>());
    }
}
