using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    Yarn.Unity.DialogueUI dialogUI;
    [SerializeField]
    Text nameLabel;
    [SerializeField]
    Text dialogLabel;

    struct Line {
        public string speaker;
        public string dialog;
    }

    public void StartDialog() {
        playerController.SetInDialog(true);
    }

    public void EndDialog() {
        playerController.SetInDialog(false);
    }

    public void ShowLine(string text) {
        Line line = ParseLine(text);
        nameLabel.text = line.speaker;
        dialogLabel.text = line.dialog;
    }

    Line ParseLine(string line) {
        //YarnSpinner only returns the entire line of dialog in the form "[Name]: [Line]"
        //we need to separate this into two strings to display nicely
        int index = line.IndexOf(':');

        return new Line {
            speaker = line.Substring(0, index),
            dialog = line.Length >= index + 2 ? line.Substring(index + 2) : null
        };
    }

    public void FinishLine() {
        dialogUI.MarkLineComplete();
    }
}
