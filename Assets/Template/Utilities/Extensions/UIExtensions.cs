using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIExtensions
{
    public static bool IsOverUI()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}
}
