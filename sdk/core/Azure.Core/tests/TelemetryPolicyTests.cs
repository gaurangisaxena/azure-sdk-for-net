﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Core.Pipeline.Policies;
using Azure.Core.Testing;
using NUnit.Framework;

namespace Azure.Core.Tests
{
    public class TelemetryPolicyTests : PolicyTestBase
    {
        [Test]
        public async Task IncludesComponentNameAndVersion()
        {
            var transport = new MockTransport(new MockResponse(200));
            var telemetryPolicy = new TelemetryPolicy("base-test", "1.0.0", null);

            await SendGetRequest(transport, telemetryPolicy);

            Assert.True(transport.SingleRequest.TryGetHeader("User-Agent", out var userAgent));
            Assert.AreEqual(userAgent, $"azsdk-net-base-test/1.0.0 ({RuntimeInformation.FrameworkDescription}; {RuntimeInformation.OSDescription})");
        }

        [Test]
        public async Task ApplicationIdIsIncluded()
        {
            var transport = new MockTransport(new MockResponse(200));
            var telemetryPolicy = new TelemetryPolicy("base-test", "1.0.0", "application-id");

            await SendGetRequest(transport, telemetryPolicy);

            Assert.True(transport.SingleRequest.TryGetHeader("User-Agent", out var userAgent));
            StringAssert.StartsWith("application-id ", userAgent);
        }

        [NonParallelizable]
        [Theory]
        [TestCase("true")]
        [TestCase("TRUE")]
        [TestCase("1")]
        public void CanDisableTelemetryWithEnvironmentVariable(string value)
        {
            try
            {
                Environment.SetEnvironmentVariable("AZURE_TELEMETRY_DISABLED", value);

                var testOptions = new TestOptions();
                Assert.False(testOptions.Diagnostics.IsTelemetryEnabled);
            }
            finally
            {
                Environment.SetEnvironmentVariable("AZURE_TELEMETRY_DISABLED", null);
            }
        }

        [NonParallelizable]
        [Test]
        public void UsesDefaultApplicationId()
        {
            try
            {
                DiagnosticsOptions.DefaultApplicationId = "Global-application-id";

                var testOptions = new TestOptions();
                Assert.AreEqual("Global-application-id", testOptions.Diagnostics.ApplicationId);
            }
            finally
            {
                DiagnosticsOptions.DefaultApplicationId = null;
            }
        }

        private class TestOptions : ClientOptions
        {

        }
    }
}
