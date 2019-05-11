using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniScript;
using ModPlayerSDK;
using ModPlayerSDK.Model;

public class ModListItem : MonoBehaviour,
    IPointerClickHandler
{
    public RawImage thumbnail;
    public Text title, lastBuildDate;
    public GameObject noBuilds;

    private ModApp app;

    public void SetApp(ModApp app)
    {
        this.app = app;
        title.text = app.name;

        if (app.has_build == false)
        {
            lastBuildDate.gameObject.SetActive(false);
            noBuilds.SetActive(true);
        }
        else
            lastBuildDate.text = app.last_build.GetCreatedAt().ToLongDateString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (app.has_build == false)
            return;

        UniAssetBundle.LoadUniScriptScene(
            app.last_build.scene_url,
            app.last_build.script_url);
    }
}
