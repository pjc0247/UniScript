using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

namespace ModPlayerSDK
{
    using Model;

    public class App
    {
        public async static Task CreateApp(string title, string description)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException(nameof(title));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("createApp")
                .CallAsync(new Dictionary<string, object>() {
                    ["title"] = title,
                    ["description"] = description
                });

            var json = JsonConvert.SerializeObject(resp.Data);
        }
        public async static Task<GetAppsResponse> GetMyApps()
        {
            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("getApps")
                .CallAsync(new Dictionary<string, object>() {
                });

            return Reinterpret<GetAppsResponse>(resp.Data);
        }
        public async static Task SetBuild(ModApp app, string sceneUrl, string scriptUrl)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (string.IsNullOrEmpty(sceneUrl))
                throw new ArgumentException(nameof(sceneUrl));
            if (string.IsNullOrEmpty(scriptUrl))
                throw new ArgumentException(nameof(scriptUrl));

            var func = ModPlayerFB.Functions;
            var resp = await func.GetHttpsCallable("setBuild")
                .CallAsync(new Dictionary<string, object>() {
                    ["app_id"] = app.id,
                    ["scene_url"] = sceneUrl,
                    ["script_url"] = scriptUrl
                });
        }

        private static T Reinterpret<T>(object input)
        {
            var json = JsonConvert.SerializeObject(input);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    namespace Model
    {
        public class GetAppsResponse
        {
            public ModApp[] apps;
        }
    }
}
