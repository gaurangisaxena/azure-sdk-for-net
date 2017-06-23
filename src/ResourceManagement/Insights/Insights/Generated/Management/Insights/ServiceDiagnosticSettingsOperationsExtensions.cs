// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;

namespace Microsoft.Azure.Management.Insights
{
    public static partial class ServiceDiagnosticSettingsOperationsExtensions
    {
        /// <summary>
        /// Deletes the diagnostic settings for the specified resource.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <returns>
        /// Generic empty response. We only pass it to ensure json error
        /// handling
        /// </returns>
        public static EmptyResponse Delete(this IServiceDiagnosticSettingsOperations operations, string resourceUri)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IServiceDiagnosticSettingsOperations)s).DeleteAsync(resourceUri);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Deletes the diagnostic settings for the specified resource.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <returns>
        /// Generic empty response. We only pass it to ensure json error
        /// handling
        /// </returns>
        public static Task<EmptyResponse> DeleteAsync(this IServiceDiagnosticSettingsOperations operations, string resourceUri)
        {
            return operations.DeleteAsync(resourceUri, CancellationToken.None);
        }
        
        /// <summary>
        /// Gets the active diagnostic settings. To get the diagnostic settings
        /// being applied, use GetStatus.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <returns>
        /// A standard service response including an HTTP status code and
        /// request ID.
        /// </returns>
        public static ServiceDiagnosticSettingsGetResponse Get(this IServiceDiagnosticSettingsOperations operations, string resourceUri)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IServiceDiagnosticSettingsOperations)s).GetAsync(resourceUri);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Gets the active diagnostic settings. To get the diagnostic settings
        /// being applied, use GetStatus.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <returns>
        /// A standard service response including an HTTP status code and
        /// request ID.
        /// </returns>
        public static Task<ServiceDiagnosticSettingsGetResponse> GetAsync(this IServiceDiagnosticSettingsOperations operations, string resourceUri)
        {
            return operations.GetAsync(resourceUri, CancellationToken.None);
        }
        
        /// <summary>
        /// Create or update new diagnostic settings for the specified
        /// resource. This operation is long running. Use GetStatus to check
        /// the status of this operation.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <param name='parameters'>
        /// Required. Parameters supplied to the operation.
        /// </param>
        /// <returns>
        /// Generic empty response. We only pass it to ensure json error
        /// handling
        /// </returns>
        public static EmptyResponse Put(this IServiceDiagnosticSettingsOperations operations, string resourceUri, ServiceDiagnosticSettingsPutParameters parameters)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IServiceDiagnosticSettingsOperations)s).PutAsync(resourceUri, parameters);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Create or update new diagnostic settings for the specified
        /// resource. This operation is long running. Use GetStatus to check
        /// the status of this operation.
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Insights.IServiceDiagnosticSettingsOperations.
        /// </param>
        /// <param name='resourceUri'>
        /// Required. The resource identifier of the configuration.
        /// </param>
        /// <param name='parameters'>
        /// Required. Parameters supplied to the operation.
        /// </param>
        /// <returns>
        /// Generic empty response. We only pass it to ensure json error
        /// handling
        /// </returns>
        public static Task<EmptyResponse> PutAsync(this IServiceDiagnosticSettingsOperations operations, string resourceUri, ServiceDiagnosticSettingsPutParameters parameters)
        {
            return operations.PutAsync(resourceUri, parameters, CancellationToken.None);
        }
    }
}