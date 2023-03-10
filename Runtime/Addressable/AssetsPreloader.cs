using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class AssetsPreloader : AssetsOperator<AssetsPreloader.Req, object> {
        public struct Req {
            public String Key { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0} }}", Key);
            }
        }

        protected override bool FireProgressEvents() {
            return true;
        }

        protected override string GetOperationKey(Req req) {
            return req.Key;
        }

        protected override AsyncOperationHandle CreateOperationHandle(Req req) {
            return Addressables.DownloadDependenciesAsync(req.Key);
        }

        protected override object GetResult(AsyncOperationHandle handle) {
            return handle.Result;
        }
    }
}