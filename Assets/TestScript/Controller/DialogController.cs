using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionType { Normal, Shop, Quest }

[System.Serializable]
public class CharacterSentence
{
    [System.Serializable]
    public class Selection
    {
        public string title;
        public SelectionType type;
        public bool enable = true;
        public string[] selectionSentece;
        public System.Action callback;
    }

    public string[] firstSantence;
    public Selection[] selections;
}

public class DialogController : MonoBehaviour {

    public Text characterNameText;
    public Text dialogText;
    public Button[] selectionButtons = new Button[3];

    public CharacterSentence characterSentence;
    public float typingSpeed = 0.2f;

    int currentLine = 0;
    int currentChar = 0;
    int selectorIndex = 0;
    string showingString = "";
    string[] currentSentece;

    bool isTyping = false;
    bool stopTyping = false;
    bool isStartSelection = false;
    bool isWaitingForSelect = false;
    bool isStartSelectionSentence = false;
    bool isBegin = false;

    public System.Action actionCallback;

    public void Init(string characterName,CharacterSentence sentence, System.Action callback = null)
    {
        if (isBegin)
            return;
        characterNameText.text = characterName;
        characterSentence = sentence;
        showingString = "";
        currentLine = 0;
        currentChar = 0;
        currentSentece = sentence.firstSantence;
        actionCallback = callback;

        isTyping = false;
        stopTyping = false;
        isStartSelection = false;
        isStartSelectionSentence = false;
        isWaitingForSelect = false;
        isBegin = true;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!stopTyping)
            {
                stopTyping = true;
                isTyping = false;
                dialogText.text = currentSentece[currentLine];
            }
            else if (stopTyping)
            {
                if (isWaitingForSelect)
                    return;

                if(currentSentece.Length - 1 > currentLine && (!isStartSelection || isStartSelectionSentence) )
                {
                    currentLine++;
                    currentChar = 0;
                    showingString = "";
                    stopTyping = false;
                }
                else
                {
                    OnSentenceOver();
                    return;
                }

                if (currentLine == currentSentece.Length - 1 && !isStartSelection)
                {
                    EnableSlector();
                }
                
            }
        }

        StartCoroutine(TypingText(currentSentece));

    }

    public void OnSentenceOver()
    {
        isBegin = false;

        for (int i = 0; i < characterSentence.selections.Length; i++)
        {
            selectionButtons[i].gameObject.SetActive(false);
        }

        if (actionCallback != null)
            actionCallback();

        gameObject.SetActive(false);
    }

    public void OnInteruptClose()
    {
        isBegin = false;
        gameObject.SetActive(false);
    }

    void EnableSlector()
    {
        if(characterSentence.selections.Length == 0)
        {
            return;
        }

        isStartSelection = true;
        isWaitingForSelect = true;
        for (int i = 0; i < characterSentence.selections.Length; i++)
        {
            selectionButtons[i].GetComponentInChildren<Text>().text = characterSentence.selections[i].title;
            selectionButtons[i].gameObject.SetActive(true);
            selectionButtons[i].interactable = characterSentence.selections[i].enable;
        }
    }

    public void DisableSelector(int buttonIndex)
    {
        selectorIndex = buttonIndex;
        currentChar = 0;
        currentLine = 0;
        currentSentece = characterSentence.selections[selectorIndex].selectionSentece;
        actionCallback = characterSentence.selections[selectorIndex].callback;

        isWaitingForSelect = false;
        isStartSelectionSentence = true;
        stopTyping = false;
        showingString = "";

        for (int i = 0; i < characterSentence.selections.Length; i++)
        {
            selectionButtons[i].gameObject.SetActive(false);
        }
    }

    IEnumerator TypingText(string[] sentences)
    {
        if (!isTyping && !stopTyping)
        {
            isTyping = true;
            showingString += sentences[currentLine][currentChar];
            currentChar++;
            dialogText.text = showingString;

            if (currentChar >= sentences[currentLine].Length - 1)
            {
                stopTyping = true;
                isTyping = false;
                yield break;
            }

            yield return new WaitForSeconds(typingSpeed);
            isTyping = false;
        }
    }
}
