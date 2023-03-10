using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public partial class Assets : Env, ISingleton {
        private static Assets _Instance;
        public static Assets Instance { get => Singleton.GetInstance(ref _Instance); }

        // Aspects
        public AspectReference<AssetsChannel> AssetsChannel { get; private set; }
        public AspectReference<CatalogLoader> CatalogLoader { get; private set; }
        public AspectReference<AssetsPreloader> AssetsPreloader { get; private set; }
        public AspectReference<AssetsSizeCalculator> AssetsSizeCalculator { get; private set; }
        public AspectReference<CacheCleaner> CacheCleaner { get; private set; }

        protected override void OnAwake() {
            Singleton.SetupInstance(ref _Instance, this);

            AssetsChannel = CacheAspect<AssetsChannel>();
            CatalogLoader = CacheAspect<CatalogLoader>();
            AssetsPreloader = CacheAspect<AssetsPreloader>();
            AssetsSizeCalculator = CacheAspect<AssetsSizeCalculator>();
            CacheCleaner = CacheAspect<CacheCleaner>();
        }

        public void ClearAllCache() {
            if (!Caching.ClearCache()) {
                Error("ClearAllCache Failed");
            } else if (LogDebug) {
                Debug("ClearAllCache Succeeded");
            }
        }
    }
}

