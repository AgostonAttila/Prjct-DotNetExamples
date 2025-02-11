using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthTest.Models;

namespace AuthTest.Authentication
{
    public class AuthenticationService
    {
        private const string USER_SESSION_KEY = "app_user_session";

        public async Task<UserSession> GetUserSession()
        {
            UserSession? userSession = null;

            var userSessionJson = await SecureStorage.Default.GetAsync(USER_SESSION_KEY);

            if (!string.IsNullOrWhiteSpace(userSessionJson))
                userSession = JsonSerializer.Deserialize<UserSession>(userSessionJson);

            return userSession;

        }


        public async Task SaveUserSession(UserSession userSession)
        {
            var userSessionJson = JsonSerializer.Serialize(userSession);
            await SecureStorage.Default.SetAsync(USER_SESSION_KEY, userSessionJson);
        }

        public void RemoveUserSession()
        {
            SecureStorage.Remove(USER_SESSION_KEY);
        }

    }
}
