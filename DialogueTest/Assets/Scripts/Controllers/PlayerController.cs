using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MoveSpeed;

    private float xMove;
    private float yMove;

    private bool conversationPossible;

    private NPC currentSpeaker;

    public void Start()
    {
        conversationPossible = false;

        StartCoroutine(CheckConversationStatus());
    }

    public void Update()
    {   
        DoMotion();
    }

    private void DoMotion()
    {
        if (!DialogueManager.isRunningDialogue)
        {
            xMove = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            yMove = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
            transform.Translate(new Vector3(xMove, yMove, 0));
        }
    }

    private IEnumerator CheckConversationStatus()
    {
        while (true)
        {
            if (!DialogueManager.isRunningDialogue)
            {
                if (conversationPossible)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        currentSpeaker.Talk();
                    }
                }
            }

            yield return new WaitForSeconds(0.0f);
        }
    }

    // show interact signal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            currentSpeaker = collision.gameObject.GetComponent<NPC>();
            currentSpeaker.ShowSignal();
            conversationPossible = true;
            
        }
    }

    // hide interact signal
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            currentSpeaker.HideSignal();
            currentSpeaker = null;       
            conversationPossible = false;
        }
    }
}
