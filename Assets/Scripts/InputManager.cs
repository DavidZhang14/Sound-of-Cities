using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
	private Vector2 cameraMovementVector;

	[SerializeField]
	Camera mainCamera;

	public LayerMask groundMask;

	public Vector2 CameraMovementVector
	{
		get { return cameraMovementVector; }
	}

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	private void Update()
	{
		// Mouse
		CheckClickDownEvent();
		CheckClickUpEvent();
		CheckClickHoldEvent();
		CheckRightClickDownEvent();

		// Arrow keys
		CheckArrowInput();

		// Ctrl
		CheckCommand();
	}

	private Vector3Int? RaycastGround()
	{
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
		{
			Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
			return positionInt;
		}
		return null;
	}

	private void CheckArrowInput()
	{
		cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void CheckClickHoldEvent()
	{
		if(Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			if (position != null)
				OnMouseHold?.Invoke(position.Value);
		}
	}

	private void CheckClickUpEvent()
	{
		if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			OnMouseUp?.Invoke();
		}
	}

	private void CheckClickDownEvent()
	{
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			if (position != null)
				OnMouseClick?.Invoke(position.Value);
		}
	}

	private void CheckRightClickDownEvent() {
		if(Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject() == false) {
			var position = RaycastGround();
			if (position != null) {
				Vector3Int deletePosition = position.Value;
				PlacementManager.instance.TryDeleteObject(deletePosition);
			}
		}
	}

	private void CheckCommand() {
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) 
		{
			if (Input.GetKeyDown(KeyCode.S)) GameManager.OpenSavePanel();
		}

		// Pitch and Rhythm Shortcuts
		if (UIController.editTarget) {
			//if hold down shift key
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
			{
				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					short newTargetGrid = (short)(1 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					short newTargetGrid = (short)(9 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3)) {
					short newTargetGrid = (short)(17 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4)) {
					short newTargetGrid = (short)(25 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5)) {
					short newTargetGrid = (short)(33 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6)) {
					short newTargetGrid = (short)(41 + (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
			}
			//if not hold down shift key
			else 
			{
				if (Input.GetKeyDown(KeyCode.BackQuote)) {
					short newTargetGrid = (short)(UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha1)) {
					short newTargetGrid = (short)(1 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					short newTargetGrid = (short)(2 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3)) {
					short newTargetGrid = (short)(3 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4)) {
					short newTargetGrid = (short)(4 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5)) {
					short newTargetGrid = (short)(5 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6)) {
					short newTargetGrid = (short)(6 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha7)) {
					short newTargetGrid = (short)(7 + UIController.editTarget.targetGrid - (--UIController.editTarget.targetGrid) % 8);
					UIController.editTarget.targetGrid = newTargetGrid;
					UIController.Instance.UpdateInfoPanel();
				}
			}
		}
	}
	public void MIDI_PitchControl(int pitch) {
		Debug.Log("MIDI triggered at pitch " + pitch);
		if (UIController.editTarget) {
			UIController.editTarget.pitch = (short)pitch;
			UIController.editTarget.UpdateSound();
			UIController.Instance.UpdateInfoPanel();
		}
	}
}
