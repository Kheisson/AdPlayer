using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CrossPromo.Scripts
{
    public class GetCrossPromoRequest : CrossPromoRequest
    {
        public CrossPromosMetaDataDto DataDto { get; set; }
        public GetCrossPromoRequest(string url) : base(url)
        {
        }

        public GetCrossPromoRequest(string url, Action<string> onFailedRequest) : base(url, onFailedRequest)
        {
            
        }
        
        public override IEnumerator SendRequest()
        {
            using var uwr = UnityWebRequest.Get(Url);
            yield return uwr.SendWebRequest();
            var success = string.IsNullOrEmpty(uwr.error);
            if (success)
            {
                var json = uwr.downloadHandler.text;
                DataDto = JsonUtility.FromJson<CrossPromosMetaDataDto>(json);
            }
            else
            {
                OnFailedRequest(uwr.error);
            }
        }
    }
}