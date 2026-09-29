using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelierCartes.Modeles
{
    internal class ErreurApi
    {
        #region Proprietes

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        
        #endregion
    }
}
