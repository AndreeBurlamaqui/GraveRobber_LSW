using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input Reader")]
public class InputReader : ScriptableObject, CharacterControls.IGameplayActions
{

	//Gameplay
	public event UnityAction<Vector2, bool> onMoveEvent;
	public event UnityAction onInteractEvent;
	public event UnityAction<bool> onDigEvent;


	private CharacterControls characterControls;
	private PointerEventData clickData = new PointerEventData(EventSystem.current);
	List<RaycastResult> clickRayResults = new List<RaycastResult>();
	public GraphicRaycaster uiRaycast { get; set; }
	private Vector2 targetMousePosTest;

    #region Enable and Disable Inputs
    public void OnEnable()
	{
		if (characterControls == null)
		{
			characterControls = new CharacterControls();
			characterControls.Gameplay.SetCallbacks(this);
		}
		EnableGameplayInput();
	}

	public void OnDisable()
	{
		DisableAllInput();
	}


	public void EnableGameplayInput()
	{
		characterControls.Gameplay.Enable();
	}

	public void DisableAllInput()
	{
		characterControls.Gameplay.Disable();
	}
    #endregion

    public void OnInteract(InputAction.CallbackContext context)
    {
		if (onInteractEvent != null && context.performed)
			onInteractEvent.Invoke();
	}

    public void OnMovement(InputAction.CallbackContext context)
    {
		if (onMoveEvent != null)
		{
			onMoveEvent.Invoke(context.ReadValue<Vector2>(), context.performed? true : false);
		}
	}

	

	private List<RaycastResult> OnMouseCheck(GameObject canvasToCheck)
	{
		clickData.position = Mouse.current.position.ReadValue();
		clickRayResults.Clear();

		if (uiRaycast == null)
			uiRaycast = canvasToCheck.GetComponent<GraphicRaycaster>();

		uiRaycast.Raycast(clickData, clickRayResults);

		return clickRayResults;
	}

	
	public int OnDigTest(GameObject canvasToDig)
	{
		if (Mouse.current.leftButton.IsPressed()) //click mouse down (holding)
		{
			List<RaycastResult> newRaycastResult = new List<RaycastResult>();
			newRaycastResult = OnMouseCheck(canvasToDig);

			foreach(RaycastResult rr in newRaycastResult)
			{
				if (rr.gameObject.TryGetComponent(out DigStage toDigTest))
				{
					return toDigTest.digTestPosition;
				}
			}

		}
		else //click mouse up (not holding)
		{
			return 0;
		}

		return 0;
	}

	public void SetDigMousePosition(Vector2 multiplyPos)
	{
		Vector2 currentMousePos = Mouse.current.position.ReadValue();
		targetMousePosTest = currentMousePos + multiplyPos;
	}

	public Vector2 GetCurrentMousePosition()
	{
		return Mouse.current.position.ReadValue();
	}

	public Vector2 GetTargetMousePosition()
	{
		return targetMousePosTest;
	}

	public void OnDig(InputAction.CallbackContext context)
	{
		if (onDigEvent != null)
			onDigEvent.Invoke(context.performed? true : false);
	}

	public void HideCursor()
	{
		Cursor.visible = false;
	}

	public void ShowCursor()
	{
		Cursor.visible = true;
	}
}
