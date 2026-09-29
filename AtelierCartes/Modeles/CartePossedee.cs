using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace AtelierCartes.Modeles
{
    internal class CartePossedee
    {
        #region Proprietes

        [JsonProperty("id")]
        public string Identifiant { get; set; } = string.Empty;

        [JsonProperty("card")]
        public Carte Carte { get; set; } = new Carte();

        [JsonProperty("currentValue")]
        public decimal ValeurActuelle { get; set; } = 0;

        #endregion
    }
}
