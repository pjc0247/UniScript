using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Storage;
using Firebase.Functions;

namespace ModPlayerSDK.Internal
{
    public class ModPlayerFB
    {
        private static FirebaseApp _App;
        public static FirebaseApp App
        {
            get
            {
                if (_App == null)
                {
                    _App = FirebaseApp.Create(new AppOptions()
                    {
                        ApiKey = "AIzaSyBTB9nagH2-TSZcVswSVHeUHlkXfo1LYAs",
                        AppId = "modplayer-kr",
                        ProjectId = "modplayer-kr"
                    });
                }
                return _App;
            }
        }

        private static FirebaseAuth _Auth;
        public static FirebaseAuth Auth
        {
            get
            {
                if (_Auth == null)
                    _Auth = FirebaseAuth.GetAuth(App);
                return _Auth;
            }
        }

        private static FirebaseStorage _Storage;
        public static FirebaseStorage Storage
        {
            get
            {
                if (_Storage == null)
                    _Storage = FirebaseStorage.GetInstance(App, "gs://modplayer-kr.appspot.com/");
                return _Storage;
            }
        }

        private static FirebaseFunctions _Functions;
        public static FirebaseFunctions Functions
        {
            get
            {
                if (_Functions == null)
                    _Functions = FirebaseFunctions.GetInstance(App);
                return _Functions;
            }
        }
    }
}