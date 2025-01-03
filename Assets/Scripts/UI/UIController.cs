﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UI_Outline = UnityEngine.UI.Outline;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Action OnRoadPlacement, OnHousePlacement, OnSpecialPlacement, OnEdit;
    [SerializeField] private Button placeRoadButton, placeHouseButton, placeSpecialButton, editButton;

    public Color outlineColor;
    List<Button> buttonList;
    public static StructureSoundEmitter editTarget;
    public InfoPanel infoPanel;
    public BuildingPanel buildingPanel;
    [SerializeField] private GameObject advancedPanel;
    [SerializeField] private GameObject ConfirmationPanel;
    private void Awake() {
        if (Instance != null) {
            Destroy (gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, editButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();

        });
        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();

        });
        /*
        placeSpecialButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeSpecialButton);
            OnSpecialPlacement?.Invoke();

        });*/
        editButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(editButton);
            OnEdit?.Invoke();

        });
    }

    private void ModifyOutline(Button button)
    {
        UI_Outline outline = button.GetComponent<UI_Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<UI_Outline>().enabled = false;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) advancedPanel.SetActive(!advancedPanel.activeSelf);
        
        // Function number hotkeys
        if (Input.GetKeyDown(KeyCode.F1)) placeRoadButton.onClick.Invoke();
        else if (Input.GetKeyDown(KeyCode.F2)) placeHouseButton.onClick.Invoke();
        else if (Input.GetKeyDown(KeyCode.F3)) editButton.onClick.Invoke();
    }
    public void OpenConfirmationPanel(string message) {
        ConfirmationPanel.SetActive(true);
        ConfirmationPanel.transform.Find("Message").GetComponent<TMP_Text>().SetText(message);
    }
}
