using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
    public KeyCode DialogueKey;

    private NPC nearestNPC;
    private bool isSignalActive;

    // Start is called before the first frame update
    void Start()
    {
        nearestNPC = null;
        isSignalActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.isRunningDialogue)
        {
            if (isSignalActive)
            {
                if (Input.GetKeyDown(DialogueKey))
                {
                    nearestNPC.Talk();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>() != null)
        {
            if (nearestNPC != null) return;

            nearestNPC = collision.gameObject.GetComponent<NPC>();
            nearestNPC.ShowSignal();
            isSignalActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearestNPC != null)
        {
            nearestNPC.HideSignal();
            nearestNPC = null;
            isSignalActive = false;
        }
    }
}
