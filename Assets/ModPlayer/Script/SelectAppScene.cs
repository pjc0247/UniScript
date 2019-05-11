using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ModPlayerSDK;
using ModPlayerSDK.Model;

public class SelectAppScene : MonoBehaviour
{
    public RectTransform appsContainer;

    private ModApp[] apps;

    async void Start()
    {
        apps = (await App.GetApps()).apps;

        foreach (var app in apps)
        {
            var item = Instantiate(
                Resources.Load<GameObject>("ModListItem"),
                appsContainer);

            item.GetComponent<ModListItem>()
                .SetApp(app);
        }
    }
}
