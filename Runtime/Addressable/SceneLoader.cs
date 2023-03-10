using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class SceneLoader : AssetsOperator<string, SceneInstance> {
        protected override bool FireProgressEvents() {
            return true;
        }

        protected override string GetOperationKey(string req) {
            return req;
        }

        protected override AsyncOperationHandle CreateOperationHandle(string req) {
            return Addressables.LoadSceneAsync(req);
        }
    }
}