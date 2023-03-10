using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class CatalogLoader : AssetsOperator<CatalogLoader.Req, IResourceLocator> {
        public struct Req {
            public string Key { get; init; }
            public string Url { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Url = {1} }}", Key, Url);
            }
        }

        protected override bool FireProgressEvents() {
            return true;
        }

        protected override string GetOperationKey(Req req) {
            return req.Key;
        }

        protected override AsyncOperationHandle CreateOperationHandle(Req req) {
            return Addressables.LoadContentCatalogAsync(req.Url);
        }
    }
}