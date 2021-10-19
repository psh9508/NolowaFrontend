﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NolowaFrontend.Configurations;
using System.Security.Cryptography;
using NolowaFrontend.Extensions;

namespace NolowaFrontend.Core.SNSLogin
{
    public class GoogleLoginProvider : SNSLoginBase<GoogleLoginConfiguration>
    {
        public GoogleLoginProvider() : base(configPath: "SNSLoginGroup/GoogleLogin")
        {
        }

        public override void ShowLoginPage()
        {
            var authorizationRequestBuilder = new StringBuilder();

            authorizationRequestBuilder.Append(_configuration.AuthorizationEndpoint);
            authorizationRequestBuilder.Append("?");
            authorizationRequestBuilder.Append("response_type=code");
            authorizationRequestBuilder.Append("&");
            authorizationRequestBuilder.Append("scope=email%20profile");
            authorizationRequestBuilder.Append("&");
            authorizationRequestBuilder.Append($"redirect_uri={_configuration.RedirectURI}");
            authorizationRequestBuilder.Append("&");
            authorizationRequestBuilder.Append($"client_id={_configuration.GoogleClientID}");
            //authorizationRequestBuilder.Append("&");
            //authorizationRequestBuilder.Append($"code_challenge={GetCodeChallenge()}");
            //authorizationRequestBuilder.Append("&");
            //authorizationRequestBuilder.Append($"code_challenge_method=S256");

            Process.Start(new ProcessStartInfo(authorizationRequestBuilder.ToString()) { UseShellExecute = true });
        }

        private string GetCodeChallenge()
        {
            string codeVerifier = RandomDataBase64url(32);

            return codeVerifier.ToSha256().Base64UrlEncodeNoPadding();
        }
    }
}
