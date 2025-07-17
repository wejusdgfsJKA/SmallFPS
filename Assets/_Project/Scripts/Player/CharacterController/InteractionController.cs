using KBCore.Refs;
using TMPro;
using UnityEngine;

public class InteractionController : ValidatedMonoBehaviour
{
    #region Fields
    [SerializeField, Anywhere] InputReader inputReader;

    /// <summary>
    /// The interactable we are currently able to interact with. DO NOT CHANGE!!!
    /// </summary>
    protected Transform currentInteractable = null;
    protected Transform currentInteractableProperty
    {
        get
        {
            return currentInteractable;
        }
        set
        {
            if (currentInteractable != value)
            {
                if (value == null)
                {
                    if (interactionPrompt != null)
                    {
                        interactionPrompt.enabled = false;
                    }
                }
                else
                {
                    if (interactionPrompt != null)
                    {
                        interactionPrompt.enabled = true;
                    }
                }
                currentInteractable = value;
            }
        }
    }
    protected RaycastHit hit;
    [SerializeField, Anywhere] TextMeshProUGUI interactionPrompt;

    [SerializeField, Anywhere] GameObject settingsScreen;
    #endregion
    private void OnEnable()
    {
        inputReader.Interact += OnInteract;
        inputReader.Escape += OnEscape;
        UpdateInteractionPrompt();
    }
    private void Update()
    {
        InteractionCheck();
    }
    private void OnDisable()
    {
        inputReader.Interact -= OnInteract;
        inputReader.Escape -= OnEscape;
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    /// <summary>
    /// Updates the interaction prompt. Should only fire if the interact key changes.
    /// </summary>
    protected void UpdateInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.text = $"[{inputReader.InteractKey}] to interact.";
        }
    }
    /// <summary>
    /// Check if we are able to interact with anything. Updates currentInteractableProperty accordingly.
    /// </summary>
    protected void InteractionCheck()
    {
        if (Physics.SphereCast(transform.position, transform.localScale.x / 2,
            transform.forward, out hit, GlobalPlayerConfig.InteractionDistance, 1 << 5))
        {
            if (InteractableManager.Instance.IsInteractable(hit.transform))
            {
                currentInteractableProperty = hit.transform;
                return;
            }
        }
        currentInteractableProperty = null;
    }
    /// <summary>
    /// Fires when the interact key is pressed.
    /// </summary>
    public void OnInteract()
    {
        if (currentInteractable != null)
        {
            //attempt to interact with something in front of us
            InteractableManager.Instance.Interact(currentInteractable, transform);
        }
    }
    /// <summary>
    /// Fires when the escape key is pressed in game.
    /// </summary>
    public void OnEscape()
    {
        if (settingsScreen.activeSelf)
        {
            GameManager.Instance.EnableMouse();
            Time.timeScale = 1;
        }
        else
        {
            GameManager.Instance.DisableMouse();
            Time.timeScale = 0;
        }
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }
    /// <summary>
    /// Fires when the MainMenu button is pressed in game.
    /// </summary>
    public void OnMainMenuButton()
    {
        GameManager.Instance.GoToMainMenu();
    }
    /// <summary>
    /// Fires when the Restart button is pressed in game.
    /// </summary>
    public void OnRestart()
    {
        GameManager.Instance.RestartLevel();
    }
}
