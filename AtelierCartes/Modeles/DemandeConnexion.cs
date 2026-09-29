using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelierCartes.Modeles
{
    internal class DemandeConnexion
    {
        #region Proprietes

        [JsonProperty("username")]
        public string Identifiant { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string MotDePasse { get; set; } = string.Empty;

        #endregion
    }
}
