using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class CacheCleaner : AssetsOperator<CacheCleaner.Req, bool> {
        public struct Req {
            public static Req PRESERVE_ALL = new Req { PreserveOnlyCategories = null };

            public string[] PreserveOnlyCategories { get; init; }

            public override string ToString() {
                return string.Format("{{ PreserveOnlyCategories = {0} }}", GetOperationKey());
            }

            public string GetOperationKey() {
                if (PreserveOnlyCategories != null && PreserveOnlyCategories.Length > 0) {
                    return String.Join(" | ", PreserveOnlyCategories);
                } else {
                    return "";
                }
            }
        }

        protected override string GetOperationKey(Req req) {
            return req.GetOperationKey();
        }

        protected override AsyncOperationHandle CreateOperationHandle(Req req) {
            return Addressables.CleanBundleCache(req.PreserveOnlyCategories);
        }
    }
}