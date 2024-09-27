public interface IInteractable
{
    void Interact();              // Defines what happens when the object is interacted with (e.g., opening a door or picking up an item)
    string GetPromptText();        // Returns the text to be displayed in the UI prompt (e.g., "Press E to pick up")
}
