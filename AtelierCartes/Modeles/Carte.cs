using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AtelierCartes.Modeles
{
    internal class Carte
    {
        #region Proprietes

        [JsonProperty("id")]
        public string Identifiant { get; set; } = string.Empty;

        [JsonProperty("brand")]
        public string Marque { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Modele { get; set; } = string.Empty;

        [JsonProperty("variant")]
        public string Variante { get; set; } = string.Empty;

        #endregion
    }
}
/// <summary>
/// Rôle de JsonProperty : Permet de lier automatiquement 
/// les clés JSON envoyées par l'API aux propriétés 
/// C# nommées en français.
/// </summary>
