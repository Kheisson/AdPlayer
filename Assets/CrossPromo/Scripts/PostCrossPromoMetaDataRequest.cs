using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CrossPromo.Scripts
{
    public class PostCrossPromoMetaDataRequest : CrossPromoMetaDataRequest
    {
        public string Body { get; private set; }

        public string URL { get; private set; }

        public PostCrossPromoMetaDataRequest(string url, string body)
        {
            URL = url;
            Body = body;
        }

        public override IEnumerator SendRequest()
        {
            using var uwr = UnityWebRequest.Post(URL, Body);
            yield return uwr.SendWebRequest();
            var success = string.IsNullOrEmpty(uwr.error);
            if (!success)
            {
                throw new Exception(uwr.error);
            }
        }
    }
}