﻿using System;
using System.Diagnostics;
using System.Net.Http;
using ApiFoundation.Net.Http;
using ApiFoundation.Security.Cryptography;
using ApiFoundation.Services;

namespace ApiFoundation.Utility
{
    internal sealed class EncryptedApiClientWrapper : IDisposable
    {
        private readonly EncryptedApiClient client;

        internal EncryptedApiClientWrapper(string baseAddress)
        {
            DefaultCryptoService cryptoService = new CryptoServiceWrapper();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
            };

            this.client = new EncryptedApiClient(httpClient, cryptoService);
            this.client.EncryptingRequest += (sender, e) =>
            {
                Trace.TraceInformation("EncryptingRequest");

                if (e.RequestMessage.Content != null)
                {
                    var header = e.RequestMessage.Content.Headers;
                    Trace.TraceInformation("HEADER: {0}", header);

                    var raw = e.RequestMessage.Content.ReadAsStringAsync().Result;
                    Trace.TraceInformation("RAW: {0}", raw);
                }
            };
            this.client.RequestEncrypted += (sender, e) =>
            {
                Trace.TraceInformation("RequestEncrypted");

                if (e.RequestMessage.Content != null)
                {
                    var header = e.RequestMessage.Content.Headers;
                    Trace.TraceInformation("HEADER: {0}", header);

                    var raw = e.RequestMessage.Content.ReadAsStringAsync().Result;
                    Trace.TraceInformation("RAW: {0}", raw);
                }
            };
            this.client.DecryptingResponse += (sender, e) =>
            {
                Trace.TraceInformation("DecryptingResponse");

                if (e.ResponseMessage.Content != null)
                {
                    var header = e.ResponseMessage.Content.Headers;
                    Trace.TraceInformation("HEADER: {0}", header);

                    var raw = e.ResponseMessage.Content.ReadAsStringAsync().Result;
                    Trace.TraceInformation("RAW: {0}", raw);
                }
            };
            this.client.ResponseDecrypted += (sender, e) =>
            {
                Trace.TraceInformation("ResponseDecrypted");

                if (e.ResponseMessage.Content != null)
                {
                    var header = e.ResponseMessage.Content.Headers;
                    Trace.TraceInformation("HEADER: {0}", header);

                    var raw = e.ResponseMessage.Content.ReadAsStringAsync().Result;
                    Trace.TraceInformation("RAW: {0}", raw);
                }
            };
        }

        internal EncryptedApiClientWrapper()
            : this("http://localhost:8591")
        {
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        public static implicit operator EncryptedApiClient(EncryptedApiClientWrapper source)
        {
            return source.client;
        }
    }
}