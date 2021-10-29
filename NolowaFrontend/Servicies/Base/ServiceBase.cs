﻿using NolowaFrontend.Extensions;
using NolowaFrontend.Models;
using NolowaFrontend.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NolowaFrontend.Servicies.Base
{
    public class ServiceBase
    {
        protected static string _jwtToken = string.Empty;
        protected static readonly HttpClient _httpClient = new HttpClient();

        public ServiceBase()
        {
            if(_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("http://localhost:8080/");
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        protected async Task<ResponseBaseEntity<TResult>> DoGet<TResult>(string uri) where TResult : new()
        {
            if (uri.StartsWith("/"))
                uri = uri.Remove(0, 1);

            try
            {
                SetJWTToken();

                var result = await _httpClient.GetAsync(uri);

                var debug = await result.Content.ReadAsStringAsync();

                return new ResponseBaseEntity<TResult>()
                {
                    IsSuccess = result.IsSuccessStatusCode,
                    ResponseData = await result.Content.ReadFromJsonAsync<TResult>(),
                };
            }
            catch (Exception ex)
            {
                return new ResponseBaseEntity<TResult>()
                {
                    IsSuccess = false,
                    ResponseData = new TResult(),
                    Message = ex.Message,
                };
            }
        }

        protected async Task<TModel> GetTFromService<TModel>(string uri)
        {
            if (uri.StartsWith("/"))
                uri = uri.Remove(0, 1);

            SetJWTToken();

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TModel>();

            throw new Exception();
        }

        protected async Task<ResponseBaseEntity<TResult>> DoPost<TResult, TRequest>(string uri, TRequest body) where TResult : new()
        {
            return await DoPostBodyAsync<TResult, TRequest>(async () =>
            {
                return await _httpClient.PostAsJsonAsync(uri, body);
            });
        }

        protected async Task<ResponseBaseEntity<TResult>> DoPost<TResult, TRequest>(string uri, string jsonRowData) where TResult : new()
        {
            return await DoPostBodyAsync<TResult, TRequest>(async () =>
            {
                var content = new StringContent(jsonRowData, Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync(uri, content);
            });         
        }

        private async Task<ResponseBaseEntity<TResult>> DoPostBodyAsync<TResult, TRequest>(Func<Task<HttpResponseMessage>> postAsync) where TResult : new()
        {
            //if (uri.StartsWith("/"))
            //    uri = uri.Remove(0, 1);

            SetJWTToken();

            try
            {
                var response = await postAsync();

                var debug = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<TResult>();
                    return GetResponseModel(true, "성공", responseContent);
                }
            }
            catch (NotSupportedException) // When content type is not valid
            {
                return GetResponseModel(false, "The content type is not supported.", default(TResult));
            }
            catch (JsonException) // Invalid JSON
            {
                return GetResponseModel(false, "Invalid JSON.", default(TResult));
            }

            return new ResponseBaseEntity<TResult>();
        }


        //private ResponseBaseEntity GetResponseModel(string message)
        //{
        //    Console.WriteLine(message);

            //    return new ResponseBaseEntity
            //    {
            //        Message = message,
            //    };
            //}

        private ResponseBaseEntity<T> GetResponseModel<T>(bool isSuccess, string message, T data) where T : new()
        {
            Console.WriteLine(message);

            return new ResponseBaseEntity<T>
            {
                IsSuccess = isSuccess,
                Message = message,
                ResponseData = data,
            };
        }

        private void SetJWTToken()
        {
            var hasAuthorizationHeader = _httpClient.DefaultRequestHeaders.Contains("Authorization");

            if (_jwtToken.IsValid() && hasAuthorizationHeader == false)
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtToken);
        }
    }
}
