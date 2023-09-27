using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    private bool conversationDone;

    public GameObject continueButton;

    void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    //void Start()
    //{
    //    StartCoroutine(Type());
    //}

    public void StartConversation()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        if (index < sentences.Length -1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
            Debug.Log("Conversation has ended");
            conversationDone = true;
        }
        continueButton.SetActive(false);
    }

    public bool getConversationStatus()
    {
        return conversationDone;
    }


}
