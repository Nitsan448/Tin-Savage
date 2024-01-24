using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(_dialogue);
    }
}