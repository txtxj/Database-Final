using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Texture2D defaultTexture;
	public Texture2D enteredTexture;
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		Cursor.SetCursor(enteredTexture, Vector2.one, CursorMode.ForceSoftware);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		Cursor.SetCursor(defaultTexture, Vector2.one, CursorMode.ForceSoftware);
	}
}
