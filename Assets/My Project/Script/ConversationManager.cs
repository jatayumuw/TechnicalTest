using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    public GameObject chatPanel;
    public TextMeshProUGUI chatText;
    public Button nextButton;
    public GameObject chatButton;

    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatButton.SetActive(true);
            gameManager.hudPanel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatButton.SetActive(false);
            gameManager.hudPanel.SetActive(true);
        }
    }

    [TextArea] // Use TextArea attribute to make it easier to edit in the Inspector.
    public string[] chatEntries; // Use an array of strings for chat entries.

    private int currentEntryIndex = 0;

    private void Start()
    {
        chatButton.SetActive(false);
        chatPanel.SetActive(false);

        gameManager = FindObjectOfType<GameManager>();
    }
    public void StartConversation()
    {
        chatPanel.SetActive(true);
        currentEntryIndex = 0; // Reset the index to start from the beginning.
        DisplayNextChatEntry();
        if (!nextButton.IsInteractable())
        {
            nextButton.interactable = true; // Re-enable the button if it was disabled.
        }
        nextButton.onClick.AddListener(DisplayNextChatEntry);

        Debug.Log("Button Clicked");
    }



    private void DisplayNextChatEntry()
    {
        if (currentEntryIndex < chatEntries.Length)
        {
            chatText.text = chatEntries[currentEntryIndex];
            currentEntryIndex++;
        }
        else
        {
            chatPanel.SetActive(false); // Hide the chat panel when the conversation is over.
        }
    }

}
