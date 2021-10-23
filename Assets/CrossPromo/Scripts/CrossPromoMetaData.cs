using System.Collections.Generic;
using UnityEngine;

namespace CrossPromo.Scripts
{
    public class CrossPromoMetaData
    {
        public string Id { get; }

        public string TrackingUrl { get; }

        public string ClickUrl { get;  }

        public string VideoUrl { get;  }
        
        public CrossPromoMetaData(string id, string trackingUrl, string clickUrl, string videoUrl)
        {
            Id = id;
            TrackingUrl = trackingUrl;
            ClickUrl = clickUrl;
            VideoUrl = videoUrl;
        }
    }
}