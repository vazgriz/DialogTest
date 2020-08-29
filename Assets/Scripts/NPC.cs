using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable {
    [SerializeField]
    string name;
    [SerializeField]
    YarnProgram dialog;
    [SerializeField]
    Yarn.Unity.DialogueRunner dialogRunner;

    bool dead;

    new Rigidbody rigidbody;

    void Start() {
        dialogRunner.Dialogue.SetProgram(dialog.GetProgram());
        dialogRunner.AddStringTable(dialog);

        dialogRunner.AddCommandHandler("die", Die);

        rigidbody = GetComponent<Rigidbody>();
    }

    void Die(string[] arguments) {
        if (dead) return;
        dead = true;
        name = "[DEAD] " + name;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        rigidbody.AddTorque(Random.onUnitSphere * 0.25f, ForceMode.Impulse);
    }

    public void Interact() {
        if (!dead) {
            dialogRunner.StartDialogue();
        }
    }

    public string GetName() {
        return name;
    }
}
