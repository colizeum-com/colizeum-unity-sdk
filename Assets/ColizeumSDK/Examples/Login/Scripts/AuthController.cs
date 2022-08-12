using TMPro;
using UnityEngine;

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
                energy.text = $"Energy: {Colizeum.User.energy.current}";
            });
        }

        public void Login()
        {
            Colizeum.Auth.Login(response =>
            {
                user.text = $"Hello {response.username}";
                energy.text = $"Energy: {response.energy.current}";
            });
        }

        public void Logout()
        {
            Colizeum.Auth.Logout();

            user.text = "Waiting...";
        }
    }
}