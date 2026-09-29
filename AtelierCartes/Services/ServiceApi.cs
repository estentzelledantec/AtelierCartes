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
        // GET :
        private async Task<string> LireReponseAsync(HttpResponseMessage reponse)
        {
            // TROU : lire le JSON de la réponse avec ReadAsStringAsync
            string json = await reponse.Content.ReadAsStringAsync();

            // TROU : vérifier si la réponse indique un succès
            if (reponse.IsSuccessStatusCode)
            {
                return json;
            }

            // TROU : préparer une variable pour récupérer l'erreur API
            ErreurApi? erreur = null;

            try
            {
                // TROU : transformer le JSON d'erreur en ErreurApi
                erreur = JsonConvert.DeserializeObject<ErreurApi>(json);
            }
            catch (JsonException)
            {
                // En cas d'erreur de format JSON, conserve erreur à null
                erreur = null;
            }

            // TROU : gérer le cas Unauthorized (401)
            if (reponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Veuillez vérifier votre connexion ou vous reconnecter.");
            }

            // TROU : lever une exception avec le message d'erreur s'il est disponible
            if (erreur != null && !string.IsNullOrWhiteSpace(erreur.Message))
            {
                throw new Exception(erreur.Message);
            }

            // TROU : lever une exception générique sinon
            throw new Exception("Une erreur est survenue lors de la communication avec le serveur.");
        }
        // POST :
        public async Task SeConnecterAsync(string identifiant, string motDePasse)
        {
            DemandeConnexion demande = new DemandeConnexion
            {
                Identifiant = identifiant,
                MotDePasse = motDePasse
            };

            string json = JsonConvert.SerializeObject(demande);

            using StringContent contenu = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage reponse = await _client.PostAsync("auth/login", contenu);

            string jsonResultat = await LireReponseAsync(reponse);

            ReponseConnexion? resultat = JsonConvert.DeserializeObject<ReponseConnexion>(jsonResultat);

            if (resultat == null || string.IsNullOrWhiteSpace(resultat.JetonAcces))
            {
                throw new Exception("Jeton d'accès absent ou invalide.");
            }

            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", resultat.JetonAcces);
        }
        public async Task<ObservableCollection<CartePossedee>> RecupererCartesPossedeesAsync()
        {
            using HttpResponseMessage reponse = await _client.GetAsync("cartes");
            string jsonResultat = await LireReponseAsync(reponse);
            ObservableCollection<CartePossedee>? cartes = JsonConvert.DeserializeObject<ObservableCollection<CartePossedee>>(jsonResultat);
            if (cartes == null)
            {
                throw new Exception("Impossible de récupérer les cartes possédées.");
            }
            return cartes;
        }

        #endregion
    }
}