﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.EventHubs.Authorization;
using Azure.Messaging.EventHubs.Compatibility;
using Azure.Messaging.EventHubs.Core;
using Moq;
using NUnit.Framework;
using TrackOne;
using TrackOne.Amqp;

namespace Azure.Messaging.EventHubs.Tests
{
    /// <summary>
    ///   The suite of tests for the <see cref="TrackOneEventHubClient" />
    ///   class.
    /// </summary>
    ///
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class TrackOneEventHubClientTests
    {
        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ConstructorRequiresTheHost(string host)
        {
            Assert.That(() => new TrackOneEventHubClient(host, "test-path", Mock.Of<TokenCredential>(), new EventHubClientOptions()), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ConstructorRequiresTheEventHubPath(string path)
        {
            Assert.That(() => new TrackOneEventHubClient("my.eventhub.com", path, Mock.Of<TokenCredential>(), new EventHubClientOptions()), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorRequiresTheCredential()
        {
            Assert.That(() => new TrackOneEventHubClient("my.eventhub.com", "somePath", null, new EventHubClientOptions()), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorRequiresTheOption()
        {
            Assert.That(() => new TrackOneEventHubClient("my.eventhub.com", "somePath", Mock.Of<TokenCredential>(), null), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public void CreateFailsOnUnknownConnectionType()
        {
            var options = new EventHubClientOptions
            {
                TransportType = (TransportType)Int32.MinValue
            };

            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);

            Assert.That(() => TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public void CreateFailsForUnknownCredentialType()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var credential = Mock.Of<TokenCredential>();

            Assert.That(() => TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientCreatesTheProperClientType()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                Assert.That(client, Is.Not.Null, "The client should have been returned.");
                Assert.That(client, Is.InstanceOf<AmqpEventHubClient>(), "The client should be specific to the AMQP protocol.");
            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        [TestCase(TransportType.AmqpTcp)]
        [TestCase(TransportType.AmqpWebSockets)]
        public async Task CreateClientTranslatesTheTransportType(TransportType connectionType)
        {
            var options = new EventHubClientOptions
            {
                TransportType = connectionType
            };

            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                if (connectionType.ToString().ToLower().Contains("websockets"))
                {
                    Assert.That(client.ConnectionStringBuilder.TransportType.ToString().ToLower(), Contains.Substring("websockets"), "The transport type should be based on WebSockets.");
                }
                else
                {
                    Assert.That(client.ConnectionStringBuilder.TransportType.ToString().ToLower(), Does.Not.Contain("websockets"), "The transport type should be based on TCP.");
                }

            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientTranslatesTheSharedKeyCredential()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                Assert.That(client.InternalTokenProvider, Is.InstanceOf<TrackOneSharedAccessTokenProvider>(), "The token provider should be the track one SAS adapter.");
                Assert.That(((TrackOneSharedAccessTokenProvider)client.InternalTokenProvider).SharedAccessSignature.Value, Is.EqualTo(signature.Value), "The SAS should match.");

            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientTranslatesTheEventHubCredential()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new EventHubTokenCredential(Mock.Of<TokenCredential>(), resource);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                Assert.That(client.InternalTokenProvider, Is.InstanceOf<TrackOneGenericTokenProvider>(), "The token provider should be the track one generic adapter.");
                Assert.That(((TrackOneGenericTokenProvider)client.InternalTokenProvider).Credential, Is.EqualTo(credential), "The source credential should match.");

            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientFormsTheCorrectEndpoint()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                var endpoint = client.ConnectionStringBuilder.Endpoint;
                Assert.That(endpoint.Scheme.ToLowerInvariant(), Contains.Substring(options.TransportType.GetUriScheme().ToLowerInvariant()), "The scheme should be part of the endpoint.");
                Assert.That(endpoint.Host.ToLowerInvariant(), Contains.Substring(host.ToLowerInvariant()), "The host should be part of the endpoint.");
                Assert.That(endpoint.AbsolutePath.ToLowerInvariant(), Contains.Substring(eventHubPath.ToLowerInvariant()), "The host should be part of the endpoint.");
            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientPopulatesTheEventHubPath()
        {
            var options = new EventHubClientOptions();
            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                Assert.That(client.EventHubName, Is.EqualTo(eventHubPath), "The client should recognize the Event Hub path.");
            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateClient" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateClientPopulatesTheProxy()
        {
            var options = new EventHubClientOptions
            {
                TransportType = TransportType.AmqpWebSockets,
                Proxy = Mock.Of<IWebProxy>()
            };

            var host = "my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var client = (AmqpEventHubClient)TrackOneEventHubClient.CreateClient(host, eventHubPath, credential, options);

            try
            {
                Assert.That(client.WebProxy, Is.SameAs(options.Proxy), "The client should honor the proxy.");
            }
            finally
            {
                await client?.CloseAsync();
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CloseAsync" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CloseAsyncDoesNotDelegateIfTheClientWasNotCreated()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            await client.CloseAsync(default);
            Assert.That(mock.WasCloseAsyncInvoked, Is.False);
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CloseAsync" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CloseAsyncDelegatesToTheClient()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            // Invoke an operation to force the client to be lazily instantiated.  Otherwise,
            // Close does not delegate the call.

            await client.GetPropertiesAsync(default);
            await client.CloseAsync(default);
            Assert.That(mock.WasCloseAsyncInvoked, Is.True);
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.GetPropertiesAsync" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task GetPropertiesAsyncDelegatesToTheClient()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            try
            {
                await client.GetPropertiesAsync(default);
                Assert.That(mock.WasGetRuntimeInvoked, Is.True);
            }
            finally
            {
                await client?.CloseAsync(default);
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.GetPartitionPropertiesAsync" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task GetPartitionPropertiesAsyncDelegatesToTheClient()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            try
            {
                var partitionId = "0123";

                await client.GetPartitionPropertiesAsync(partitionId, CancellationToken.None);
                Assert.That(mock.GetPartitionRuntimePartitionInvokedWith, Is.EqualTo(partitionId));
            }
            finally
            {
                await client?.CloseAsync(default);
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateProducer" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateProducerDelegatesToTheClient()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            try
            {
                var producerOptions = new EventHubProducerOptions { PartitionId = "45345" };

                // Because the producer is lazily instantiated, an operation needs to be requested to force creation.  Because we are returning a null
                // producer from within the mock client, that operation will fail with a null reference exception.

                Assert.That(async () => await client.CreateProducer(producerOptions)?.SendAsync(new[] { new EventData(new byte[] { 0x12 }) }), Throws.InstanceOf<NullReferenceException>(), "because the EventDataSender was not populated.");
                Assert.That(mock.CreateProducerInvokedWith, Is.EqualTo(producerOptions.PartitionId));
            }
            finally
            {
                await client?.CloseAsync(default);
            }
        }

        /// <summary>
        ///   Verifies functionality of the <see cref="TrackOneEventHubClient.CreateConsumer" />
        ///   method.
        /// </summary>
        ///
        [Test]
        public async Task CreateConsumerDelegatesToTheClient()
        {
            var options = new EventHubClientOptions();
            var host = "http://my.eventhub.com";
            var eventHubPath = "some-path";
            var resource = $"amqps://{ host }/{ eventHubPath }";
            var signature = new SharedAccessSignature(resource, "keyName", "KEY", TimeSpan.FromHours(1));
            var credential = new SharedAccessSignatureCredential(signature);
            var mock = new ObservableClientMock(host, eventHubPath, credential, options);
            var client = new TrackOneEventHubClient(host, eventHubPath, credential, options, (host, path, credential, options) => mock);

            try
            {
                var partitionId = "32234";
                var consumerGroup = "AGroup";
                var eventPosition = EventPosition.FromOffset(34);
                var consumerOptions = new EventHubConsumerOptions { Identifier = "Test" };

                // Because the consumer is lazily instantiated, an operation needs to be requested to force creation.  Because we are returning a null
                // consumer from within the mock client, that operation will fail with a null reference exception.

                Assert.That(async () => await client.CreateConsumer(consumerGroup, partitionId, eventPosition, consumerOptions).ReceiveAsync(10), Throws.InstanceOf<NullReferenceException>(), "because the PartitionReceiver was not populated.");

                (var calledConsumerGroup, var calledPartition, var calledPosition, var calledPriority, var calledOptions) = mock.CreateReiverInvokedWith;

                Assert.That(calledConsumerGroup, Is.EqualTo(consumerGroup), "The consumer group should match.");
                Assert.That(calledPartition, Is.EqualTo(partitionId), "The partition should match.");
                Assert.That(calledPosition.Offset, Is.EqualTo(eventPosition.Offset), "The event position offset should match.");
                Assert.That(calledOptions.Identifier, Is.EqualTo(consumerOptions.Identifier), "The options should match.");
            }
            finally
            {
                await client?.CloseAsync(default);
            }
        }

        /// <summary>
        ///   Allows for observation of operations performed by the client for testing purposes.
        /// </summary>
        ///
        private class ObservableClientMock : TrackOne.EventHubClient
        {
            public bool WasCloseAsyncInvoked;
            public bool WasGetRuntimeInvoked;
            public string GetPartitionRuntimePartitionInvokedWith;
            public string CreateProducerInvokedWith;
            public (string ConsumerGroup, string Partition, TrackOne.EventPosition startingPosition, long? priority, TrackOne.ReceiverOptions Options) CreateReiverInvokedWith;


            public ObservableClientMock(string host,
                                        string path,
                                        TokenCredential credential,
                                        EventHubClientOptions options) : base(new TrackOne.EventHubsConnectionStringBuilder(new Uri(host), path, "keyName", "KEY!"))
            {
            }

            protected override Task OnCloseAsync()
            {
                WasCloseAsyncInvoked = true;
                return Task.CompletedTask;
            }

            protected override PartitionReceiver OnCreateReceiver(string consumerGroupName, string partitionId, TrackOne.EventPosition eventPosition, long? epoch, TrackOne.ReceiverOptions consumerOptions)
            {
                CreateReiverInvokedWith =
                (
                   consumerGroupName,
                   partitionId,
                   eventPosition,
                   epoch,
                   consumerOptions
                );

                return default(PartitionReceiver);
            }

            protected override Task<EventHubPartitionRuntimeInformation> OnGetPartitionRuntimeInformationAsync(string partitionId)
            {
                GetPartitionRuntimePartitionInvokedWith = partitionId;

                var partitionRuntimeInformation = new EventHubPartitionRuntimeInformation();
                partitionRuntimeInformation.LastEnqueuedOffset = "-1";

                return Task.FromResult(partitionRuntimeInformation);
            }

            protected override Task<EventHubRuntimeInformation> OnGetRuntimeInformationAsync()
            {
                WasGetRuntimeInvoked = true;
                return Task.FromResult(new EventHubRuntimeInformation());
            }

            internal override EventDataSender OnCreateEventSender(string partitionId)
            {
                CreateProducerInvokedWith = partitionId;
                return default(EventDataSender);
            }
        }
    }
}
