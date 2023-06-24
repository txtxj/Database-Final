using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorHandler : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D buttonCursor;
    public Texture2D textCursor;

    private void Start()
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in buttons)
        {
            PointerHandler handler = button.gameObject.AddComponent<PointerHandler>();
            handler.defaultTexture = defaultCursor;
            handler.enteredTexture = buttonCursor;
        }
        
        TMP_Dropdown[] dropdowns = Resources.FindObjectsOfTypeAll<TMP_Dropdown>();
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            PointerHandler handler = dropdown.gameObject.AddComponent<PointerHandler>();
            handler.defaultTexture = defaultCursor;
            handler.enteredTexture = buttonCursor;
        }
        
        TMP_InputField[] inputs = Resources.FindObjectsOfTypeAll<TMP_InputField>();
        foreach (TMP_InputField input in inputs)
        {
            PointerHandler handler = input.gameObject.AddComponent<PointerHandler>();
            handler.defaultTexture = defaultCursor;
            handler.enteredTexture = textCursor;
        }
    }
}
