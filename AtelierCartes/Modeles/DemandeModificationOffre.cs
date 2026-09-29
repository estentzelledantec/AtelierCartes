using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelierCartes.Modeles
{
    internal class DemandeModificationOffre
    {
        #region Proprietes
        [JsonProperty("price")]
        public decimal Prix { get; set; } = 0;
        #endregion
    }
}
