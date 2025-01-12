﻿// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal class DefaultTestData
    {
        public readonly IEnumerable<Client> Clients = new List<Client>()
        {
            new Client { ClientId = Guid.NewGuid().ToString() }
        };

        public readonly IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>()
        {
            new IdentityResources.OpenId()
        };

        public readonly IEnumerable<ApiScope> ApiScopes = new List<ApiScope>()
        {
        };

        public readonly IEnumerable<ApiResource> ApiResources = new List<ApiResource>()
        {
        };
    }
}
