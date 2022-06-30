using System;
using ColizeumSDK.Auth;
using ColizeumSDK.Utils;
using TMPro;
using UnityEngine;
using Debug = ColizeumSDK.Utils.Debug;

namespace ColizeumSDK.Examples.Login.Scripts
{
    public class AuthController : MonoBehaviour
    {
        public TMP_Text user;
        public TMP_Text energy;

        private void Awake()
        {
            if (!Colizeum.IsLoggedIn) return;

            Colizeum.Auth.GetUser(_ =>
            {
                user.text = $"Hello {Colizeum.User.username}";

                GetEnergy();
            });
        }

        public void Login()
        {
            Colizeum.Auth.Login(response =>
            {
                this.user.text = $"Hello {response.username}";

                GetEnergy();
            });
        }

        public void Logout()
        {
            Colizeum.Auth.Logout();

            user.text = "Waiting...";
        }

        private void GetEnergy()
        {
            Colizeum.API.GetUserEnergy(response => { energy.text = "Energy: " + response.item.total_energy; });
        }
    }
}