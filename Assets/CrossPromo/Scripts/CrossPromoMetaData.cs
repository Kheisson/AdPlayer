using System;
using System.Collections.Generic;

namespace CrossPromo.Scripts
{
    /// <summary>
    /// Data class to store JSON results -> Of type CrossPromoMetaData
    /// </summary>
    public class CrossPromosMetaData
    {
        public List<CrossPromoMetaData> results;
    }


    [Serializable]
    public class CrossPromoMetaData
    {
        public string id;

        public string tracking_url;

        public string click_url;

        public string video_url;
    }
}