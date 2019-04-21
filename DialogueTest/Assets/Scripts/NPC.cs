using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour {

    public NPCData data;
    public GameObject SignalSprite;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        MissingReferenceErrorCheck();
    }

    public void ShowSignal()
    {
        SignalSprite.SetActive(true);
    }

    public void HideSignal()
    {
        SignalSprite.SetActive(false);
    }

    // talk event, must be triggered
    public void Talk()
    {
        dialogueManager.HandleSpeakerData(data);
    }

    private void MissingReferenceErrorCheck()
    {
        if (dialogueManager == null)
            throw new MissingReferenceException("Error: NPC Dialogue Manager is missing from scene");

        if (SignalSprite == null)
            throw new MissingReferenceException("Error: NPC Signal Sprite is missing");

        if (data == null)
            throw new MissingReferenceException("Error: NPC Data is missing");

    }
}
