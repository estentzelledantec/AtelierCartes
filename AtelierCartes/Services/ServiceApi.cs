using AtelierCartes.Modeles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text;


namespace AtelierCartes.Services
{
    internal class ServiceApi
    {
        #region Attributs
        private readonly HttpClient _client;

        private static readonly ServiceApi instance = new ServiceApi();

        #endregion

        #region Getters et Setters
        public static ServiceApi Instance
        {
            get { return instance; } 
        }
        #endregion

        #region Constructeurs
        private ServiceApi()
        {
            // TROU : choisir l'adresse selon la plateforme
            string adresseApi = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5088/api/"
                : "https://api-cartes-g2.fldsio.com";

            // TROU : créer le client HTTP
            _client = new HttpClient();

            // TROU : donner l'adresse de l'API au client HTTP
            _client.BaseAddress = new Uri(adresseApi);

            _client.Timeout = TimeSpan.FromSeconds(15);
        }
        #endregion

        #region Proprietes

        #endregion

        #region Methodes

        private async Task<string> LireReponseAsync(HttpResponseMessage reponse)
        {
            string json = await reponse.Content.ReadAsStringAsync();

            if (reponse.IsSuccessStatusCode)
            {
                return json;
            }

            ErreurApi? erreur = null;
            try
            {
                erreur = JsonConvert.DeserializeObject<ErreurApi>(json);
            }
            catch (JsonException)
            {
                erreur = null;
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Veuillez vérifier votre connexion ou vous reconnecter.");
            }

            if (erreur != null && !string.IsNullOrWhiteSpace(erreur.Message))
            {
                throw new Exception(erreur.Message);
            }

            throw new Exception("Une erreur est survenue lors de la communication avec le serveur.");
        }

        #endregion
    }
}