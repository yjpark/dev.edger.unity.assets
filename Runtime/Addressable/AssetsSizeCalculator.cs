using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class AssetsSizeCalculator : AssetsOperator<string, long> {
        protected override string GetOperationKey(string req) {
            return req;
        }

        protected override AsyncOperationHandle CreateOperationHandle(string req) {
            return Addressables.GetDownloadSizeAsync(req);
        }
    }
}