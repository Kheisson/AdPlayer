using System;
using System.Collections;

namespace CrossPromo.Scripts
{
    public abstract class CrossPromoRequest
    {
        
        public string Url { get;}
        
        public CrossPromoRequest(string url, Action<string> onFailedRequest) : this(url)
        {
            OnFailedRequest = onFailedRequest;
        }
        public CrossPromoRequest(string url)
        {
            Url = url;
        }
        public Action<string> OnFailedRequest = s => {};
        public abstract IEnumerator SendRequest();
    }
}