// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Batch.Protocol.Models
{
    using System.Linq;

    /// <summary>
    /// Defines headers for Patch operation.
    /// </summary>
    public partial class JobSchedulePatchHeaders
    {
        /// <summary>
        /// Initializes a new instance of the JobSchedulePatchHeaders class.
        /// </summary>
        public JobSchedulePatchHeaders() { }

        /// <summary>
        /// Initializes a new instance of the JobSchedulePatchHeaders class.
        /// </summary>
        /// <param name="clientRequestId">The client-request-id provided by
        /// the client during the request. This will be returned only if the
        /// return-client-request-id parameter was set to true.</param>
        /// <param name="requestId">This header uniquely identifies the
        /// request that was made and can be used for troubleshooting the
        /// request. If a request is consistently failing and you have
        /// verified that the request is properly formulated, you may use
        /// this value to report the error to Microsoft. In your report,
        /// include the value of this header, the approximate time that the
        /// request was made, the Batch account against which the request was
        /// made, and the region that account resides in.</param>
        /// <param name="eTag">The ETag HTTP response header. This is an
        /// opaque string. You can use it to detect whether the resource has
        /// changed between requests. In particular, you can pass the ETag to
        /// one of the If-Modified-Since, If-Unmodified-Since, If-Match or
        /// If-None-Match headers.</param>
        /// <param name="lastModified">The time at which the resource was last
        /// modified.</param>
        /// <param name="dataServiceId">The OData ID of the resource to which
        /// the request applied.</param>
        public JobSchedulePatchHeaders(string clientRequestId = default(string), string requestId = default(string), string eTag = default(string), System.DateTime? lastModified = default(System.DateTime?), string dataServiceId = default(string))
        {
            ClientRequestId = clientRequestId;
            RequestId = requestId;
            ETag = eTag;
            LastModified = lastModified;
            DataServiceId = dataServiceId;
        }

        /// <summary>
        /// Gets or sets the client-request-id provided by the client during
        /// the request. This will be returned only if the
        /// return-client-request-id parameter was set to true.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "client-request-id")]
        public string ClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets this header uniquely identifies the request that was
        /// made and can be used for troubleshooting the request. If a
        /// request is consistently failing and you have verified that the
        /// request is properly formulated, you may use this value to report
        /// the error to Microsoft. In your report, include the value of this
        /// header, the approximate time that the request was made, the Batch
        /// account against which the request was made, and the region that
        /// account resides in.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "request-id")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the ETag HTTP response header. This is an opaque
        /// string. You can use it to detect whether the resource has changed
        /// between requests. In particular, you can pass the ETag to one of
        /// the If-Modified-Since, If-Unmodified-Since, If-Match or
        /// If-None-Match headers.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "ETag")]
        public string ETag { get; set; }

        /// <summary>
        /// Gets or sets the time at which the resource was last modified.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Microsoft.Rest.Serialization.DateTimeRfc1123JsonConverter))]
        [Newtonsoft.Json.JsonProperty(PropertyName = "Last-Modified")]
        public System.DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the OData ID of the resource to which the request
        /// applied.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "DataServiceId")]
        public string DataServiceId { get; set; }

    }
}
