using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevConsole : AutomatedSingleton<DevConsole>
{
    [SerializeField] CanvasGroup group;
    [SerializeField] int startPage;
    [SerializeField] DevPage[] devPages;
    [SerializeField] Color selectedColor;
    [SerializeField] Color unselectedColor;

    public bool Active => group.blocksRaycasts;

    private void Start()
    {
        SetPageActive(startPage);
    }

    // Update is called once per frame
    void Update()
    {
        bool toggle = (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F1);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle(false);
        }
        else if(toggle)
        {
            Toggle();
        }
    }

    public void Toggle(bool value)
    {
        group.alpha = value ? 1f : 0f;
        group.blocksRaycasts = value;

        //Enable if working at a game with no cursor input
        //var input = FindObjectOfType<UnityEngine.EventSystems.StandaloneInputModule>();
        //input.enabled = value;
    }
    public void Toggle()
    {
        Toggle(!group.blocksRaycasts);
    }
    public void SetPageActive(int index)
    {
        for (int i = 0; i < devPages.Length; i++)
        {
            devPages[i].obj.SetActive(i == index);
            devPages[i].buttonSprite.color = (i == index) ? selectedColor : unselectedColor;
        }
    }
    public void SetPageActive()
    {
        int index = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        SetPageActive(index);
    }

    [System.Serializable] struct DevPage
    {
        public Image buttonSprite;
        public GameObject obj;
    }
}
