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
    /// A node agent SKU supported by the Batch service.
    /// </summary>
    /// <remarks>
    /// The Batch node agent is a program that runs on each node in the pool,
    /// and provides the command-and-control interface between the node and
    /// the Batch service. There are different implementations of the node
    /// agent, known as SKUs, for different operating systems.
    /// </remarks>
    public partial class NodeAgentSku
    {
        /// <summary>
        /// Initializes a new instance of the NodeAgentSku class.
        /// </summary>
        public NodeAgentSku() { }

        /// <summary>
        /// Initializes a new instance of the NodeAgentSku class.
        /// </summary>
        /// <param name="id">The ID of the node agent SKU.</param>
        /// <param name="verifiedImageReferences">The list of images verified
        /// to be compatible with this node agent SKU.</param>
        /// <param name="osType">The type of operating system (e.g. Windows or
        /// Linux) compatible with the node agent SKU.</param>
        public NodeAgentSku(string id = default(string), System.Collections.Generic.IList<ImageReference> verifiedImageReferences = default(System.Collections.Generic.IList<ImageReference>), OSType? osType = default(OSType?))
        {
            Id = id;
            VerifiedImageReferences = verifiedImageReferences;
            OsType = osType;
        }

        /// <summary>
        /// Gets or sets the ID of the node agent SKU.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of images verified to be compatible with
        /// this node agent SKU.
        /// </summary>
        /// <remarks>
        /// This collection is not exhaustive (the node agent may be
        /// compatible with other images).
        /// </remarks>
        [Newtonsoft.Json.JsonProperty(PropertyName = "verifiedImageReferences")]
        public System.Collections.Generic.IList<ImageReference> VerifiedImageReferences { get; set; }

        /// <summary>
        /// Gets or sets the type of operating system (e.g. Windows or Linux)
        /// compatible with the node agent SKU.
        /// </summary>
        /// <remarks>
        /// Possible values include: 'linux', 'windows', 'unmapped'
        /// </remarks>
        [Newtonsoft.Json.JsonProperty(PropertyName = "osType")]
        public OSType? OsType { get; set; }

    }
}
