using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoPopupBehaviour : MonoBehaviour
{
    public static DemoPopupBehaviour instance;

    public Text textContainer;
    public GameObject container;

    private void Awake()
    {
        instance = this;
    }
    public void Show(string message)
    {
        textContainer.text = message;
        container.SetActive(true);
    }
    public void OnClickClose()
    {
        container.SetActive(false);
    }
}
