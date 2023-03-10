using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class AssetsSizeCalculator : AssetsOperator<AssetsPreloader.Req, long> {
        protected override string GetOperationKey(AssetsPreloader.Req req) {
            return req.Key;
        }

        protected override AsyncOperationHandle CreateOperationHandle(AssetsPreloader.Req req) {
            return Addressables.GetDownloadSizeAsync(req.Key);
        }
    }
}