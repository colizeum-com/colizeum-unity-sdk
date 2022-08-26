using System;

namespace ColizeumSDK.Models
{
    /// <summary>
    /// Holds information related to users secondary currency
    /// </summary>
    [Serializable]
    public class SecondaryCurrency
    {
        /// <summary>
        /// Total amount of Secondary Currency the User has
        /// </summary>
        public float total;

        /// <summary>
        /// Fetches latest information from the API and updates the model
        /// </summary>
        /// <param name="onSuccess"></param>
        public void Refresh(Action onSuccess = null)
        {
            Colizeum.API.GetSecondaryCurrency(response =>
            {
                total = response.item.total;

                onSuccess?.Invoke();
            });
        }
    }
}