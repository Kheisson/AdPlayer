using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CrossPromo.Scripts
{
    public class GetCrossPromoMetaDataRequest : CrossPromoMetaDataRequest
    {
        private string Endpoint { get; set; }
        public CrossPromosMetaData Data { get; set; }
        public GetCrossPromoMetaDataRequest(string endpoint) 
        {
            Endpoint = endpoint;
        }
        
        public override IEnumerator SendRequest()
        {
            using var uwr = UnityWebRequest.Get(Endpoint);
            yield return uwr.SendWebRequest();
            var success = string.IsNullOrEmpty(uwr.error);
            if (success)
            {
                var json = uwr.downloadHandler.text;
                Data = JsonUtility.FromJson<CrossPromosMetaData>(json);
            }
            else
            {
                throw new Exception(uwr.error);
            }
        }
    }
}