using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueManager : ASingleton<DialogueManager>
{
    private readonly Queue<string> _sentences = new();
    [SerializeField] private Animator _dialogueBoxAnimator;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private CancellationTokenSource _cancellationTokenSource;

    public void StartDialogue(Dialogue dialogue)
    {
        _dialogueBoxAnimator.SetBool("IsOpen", true);
        _nameText.text = dialogue.Name;

        _sentences.Clear();

        foreach (string sentence in dialogue.Sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }


        string sentence = _sentences.Dequeue();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        TypeSentence(sentence, _cancellationTokenSource.Token).Forget();
    }

    private void EndDialogue()
    {
        _dialogueBoxAnimator.SetBool("IsOpen", false);
    }

    private async UniTask TypeSentence(string sentence, CancellationToken cancellationToken)
    {
        _dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _dialogueText.text += letter;
            await UniTask.Yield();
        }
    }
}