using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CrossPromo.Scripts
{
    public class PostCrossPromoRequest : CrossPromoRequest
    {
        public PostCrossPromoRequest(string url) : base(url)
        {
        }

        public PostCrossPromoRequest(string url, Action<string> onFailedRequest) : base(url, onFailedRequest)
        {
            
        }

        public override IEnumerator SendRequest()
        {
            using var uwr = UnityWebRequest.Post(Url, string.Empty);
            yield return uwr.SendWebRequest();
            var success = string.IsNullOrEmpty(uwr.error);
            if (!success)
            {
                OnFailedRequest(uwr.error);
            }
        }
    }
}