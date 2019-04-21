using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue GUI", order = 1)]
    public Canvas DialogueBox;
    public Text DialogueName;
    public Text DialogueText;

    [Header("Next Line Triggers", order = 2)]
    public KeyCode KeyboardTrigger;
    public MouseButtons MouseTrigger;

    // MouseTrigger --> _mTrig
    private int _mTrig;

    // currentset --> currentdialogue
    private Queue<string> CurrentDialogue;

    // conversation state for restricting actions in other scripts
    public static bool isRunningDialogue;

    // conversation state for internal processes
    private bool hasConversationFinished;

    [HideInInspector]
    private DialogueSet CurrentSet;

    [HideInInspector]
    private string CurrentName;

    [HideInInspector]
    public UnityEvent OnProgressClick;
    
	// Use this for initialization
	void Start ()
    {
        // set up dialogue manager initial state
        InitDialogueManager();
	}

    // get speaker's data from npc and commence conversation setup
    public void HandleSpeakerData(NPCData data)
    {
        // slot in data
        CurrentName = data.NPCName;
        CurrentSet = data.NPCDialogue;

        HandleDialogue();
    }

    private void Update()
    {
        // if conversation is over, clean up the manager
        if (hasConversationFinished)
        {
            EndDialogue();
        }
    }

    private void HandleDialogue()
    {
        // init dialogue, then display dialogue box
        InitDialogue();

        // proceed convo proper (needs a loop)
        StartCoroutine(BeginDialogue());
    }

    #region DIALOGUE MANAGER INITIALIZATIONS
    private void InitDialogueManager()
    {
        // init dialogue buffer
        CurrentDialogue = new Queue<string>();

        // turn off dialogue box asap
        DialogueBox.enabled = !DialogueBox.enabled;

        // convert mouse trigger to int
        _mTrig = (int)MouseTrigger;

        // set conversation state
        isRunningDialogue = false;
        hasConversationFinished = false;
    }
    #endregion

    #region DIALOGUE MANAGER IMPLEMENTATION
    private IEnumerator BeginDialogue()
    {
        // activate conversation sequence
        isRunningDialogue = true;
        hasConversationFinished = false;

        // load name
        DialogueName.text = CurrentName;

        // display first line
        DialogueText.text = CurrentDialogue.ToArray()[0];

        // run through dialogue
        while (CurrentDialogue.Count > 0)
        {
            if (Input.GetMouseButtonDown(_mTrig) || Input.GetKeyDown(KeyboardTrigger))
            {
                OnProgressClick.Invoke();
            }

            yield return new WaitForSeconds(0.0f);
        }

        // activate clean up
        isRunningDialogue = false;
        hasConversationFinished = true;
    }

    private void InitDialogue()
    {
        Debug.Log("initialized dialogue");

        // display dialogue box
        DialogueBox.enabled = !DialogueBox.enabled;

        CurrentDialogue = new Queue<string>();
        
        // load dialogue from data
        if (CurrentSet != null)
        {
            foreach (string sentence in CurrentSet.dialogue)
            {
                CurrentDialogue.Enqueue(sentence);
            }
        }
        
        // prepare for next line trigger listening
        OnProgressClick.AddListener(Progress);

    }

    private void EndDialogue()
    {
        Debug.Log("finished dialogue");

        // stop coroutine
        StopCoroutine(BeginDialogue());

        // clear dialogue box data
        DialogueName.text = "";
        DialogueText.text = "";

        // clear NPC dialogue set
        CurrentSet = null;

        // clear dialogue buffer
        CurrentDialogue.Clear();
        CurrentDialogue = new Queue<string>();

        // disable dialogue box
        DialogueBox.enabled = !DialogueBox.enabled;

        // remove invoke for security
        OnProgressClick.RemoveListener(Progress);

        // reset conversation state to initial state
        isRunningDialogue = false;
        hasConversationFinished = false;
    }

    private void Progress()
    {
        CurrentDialogue.Dequeue();

        // if there's any dialogue left, keep displaying
        if (CurrentDialogue.Count > 0)
        {
            DialogueText.text = CurrentDialogue.ToArray()[0];
        }
    }
    #endregion
}

// enums
public enum MouseButtons
{
    Left = 0,
    Right = 2,
    ScrollClick = 1
}