using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;

namespace ModPlayerSDK
{
    using Internal;
    using Model;

    public class App
    {
        public async static Task<CreateAppResponse> CreateApp(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("createApp")
                .CallAsync(new Dictionary<string, object>() {
                    ["name"] = name
                });

            return Reinterpret<CreateAppResponse>(resp.Data);
        }
        public async static Task SetThumbnail(ModApp app, string url)
        {
            if (app == null)
                throw new ArgumentException(nameof(app));
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(nameof(url));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("setThumbnail")
                .CallAsync(new Dictionary<string, object>()
                {
                    ["app_id"] = app.id,
                    ["thumbnail_url"] = url
                });
        }

        public async static Task<GetAppsResponse> GetApps()
        {
#if UNITY_WEBGL
            var www = UnityWebRequest.Get("https://us-central1-modplayer-kr.cloudfunctions.net/getAppsPublic");
            www.SendWebRequest();
            while (www.isDone == false) ;
            return JsonConvert.DeserializeObject<GetAppsResponse>(www.downloadHandler.text);
#else
            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("getApps")
                .CallAsync(new Dictionary<string, object>() {
                });

            return Reinterpret<GetAppsResponse>(resp.Data);
#endif
        }
        public async static Task<GetAppsResponse> GetApps(string owner)
        {
            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("getApps")
                .CallAsync(new Dictionary<string, object>()
                {
                    ["owner"] = owner
                });

            return Reinterpret<GetAppsResponse>(resp.Data);
        }
        public static Task<GetAppsResponse> GetMyApps()
        {
            return GetApps(ModPlayerFB.Auth.CurrentUser.UserId);
        }
        public async static Task AddBuild(ModApp app, ModBuild build)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (build == null)
                throw new ArgumentNullException(nameof(build));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("addBuild")
                .CallAsync(new Dictionary<string, object>() {
                    ["app_id"] = app.id,
                    ["scene_url"] = build.scene_url,
                    ["script_url"] = build.script_url,
                    ["title"] = build.title,
                    ["version"] = build.version,
                    ["description"] = build.description
                });
        }

        public async static Task<GetAppsResponse> CreatePlayHistory(ModApp app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("createPlayHistory")
                .CallAsync(new Dictionary<string, object>()
                {
                    ["app_id"] = app.id
                });

            return Reinterpret<GetAppsResponse>(resp.Data);
        }
        public async static Task<GetAppsResponse> Like(ModApp app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("like")
                .CallAsync(new Dictionary<string, object>()
                {
                    ["app_id"] = app.id
                });

            return Reinterpret<GetAppsResponse>(resp.Data);
        }

        private static T Reinterpret<T>(object input)
        {
            var json = JsonConvert.SerializeObject(input);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    namespace Model
    {
        public class CreateAppResponse
        {
            public ModApp app;
        }
        public class GetAppsResponse
        {
            public ModApp[] apps;
        }
    }
}
