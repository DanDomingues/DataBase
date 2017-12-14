using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {

    public string levelName ;
    public Text optionsText ;

    public UnityAction[] actions;
    

    protected virtual void   StartGame(PointerEventData data)
    {
         SceneManager.LoadScene(levelName);

    }

    protected void Options(PointerEventData data)
    {
       Debug.Log("Coming Soon") ;
    }

    protected virtual void Quit(PointerEventData data)
    {
        SceneManager.LoadScene(0);
    }


    // Use this for initialization
    protected void Awake ()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length;i++)
        {
            EventTrigger trigger = buttons[i].gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            switch(i)
            {
                case 0: entry.callback.AddListener((data) => { StartGame((PointerEventData)data); }); break;
                case 1: entry.callback.AddListener((data) => { Options  ((PointerEventData)data); }); break;
                case 2: entry.callback.AddListener((data) => { Quit     ((PointerEventData)data); }); break;

            }

            trigger.triggers.Add(entry);


        }
    }


}
