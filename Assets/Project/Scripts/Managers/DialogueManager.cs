using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public interface IDialogueManager {
    void StartDialogue(Dialogue dialogue);
}

public class DialogueManager : MonoBehaviourSingleton<DialogueManager>, IDialogueManager {
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    private List<string> _sentences;
    public GameObject exitButton, shopButton;

    private bool _inDialogue;

    public bool ButtonsOff {
        get => _buttonsOff;
        set {
            if (!value) {
                exitButton.SetActive(true);
                shopButton.SetActive(true);
            }
            else {
                exitButton.SetActive(false);
                shopButton.SetActive(false);
            }

            _buttonsOff = value;
        }
    }

    private bool _buttonsOff;

    public Animator animator;
    private static readonly int Show = Animator.StringToHash("show");
    private static readonly int Hide = Animator.StringToHash("hide");

    private void Start() {
        _sentences = new List<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        nameText.text = dialogue.name;

        _sentences.Clear();
        _inDialogue = true;
        GameManager.Instance.OnDialogue(true);

        foreach (string s in dialogue.sentences) _sentences.Add(s);

        animator.SetTrigger(Show);
        DisplaySentence();
    }

    private void DisplaySentence() {
        string s = _sentences[Random.Range(0, _sentences.Count - 1)];

        StopAllCoroutines();
        StartCoroutine(Type(s));
    }

    private IEnumerator Type(string s, bool goodbye = false) {
        ButtonsOff = goodbye;
        _inDialogue = true;
        GameManager.Instance.OnDialogue(true);
        dialogueText.text = "";
        foreach (char l in s) {
            dialogueText.text += l;
            yield return new WaitForSeconds(0.05f);
        }

        if (goodbye) {
            yield return new WaitForSeconds(1.5f);
            animator.SetTrigger(Hide);
            _inDialogue = false;
            GameManager.Instance.OnDialogue(false);
            _buttonsOff = false;
        }
    }

    public void EndDialogue(bool accept, Dialogue goodbyeDialogue) {
        if (accept) {
            animator.SetTrigger(Hide);
            UIManager.Instance.OnShop(true);
            GameManager.Instance.OnDialogue(false);
        }
        else {
            if (!GameManager.Instance.GetInDialogue()) animator.SetTrigger(Show);
            string s = goodbyeDialogue.sentences[Random.Range(0, goodbyeDialogue.sentences.Count - 1)];
            StopAllCoroutines();
            StartCoroutine(Type(s, true));
        }
    }

    public void OnShopButton() {
        EndDialogue(true, GameManager.Instance.GetGoodbyeNpcDialogue());
    }

    public void OnEndDialogueButton() {
        EndDialogue(false, GameManager.Instance.GetGoodbyeNpcDialogue());
    }
}