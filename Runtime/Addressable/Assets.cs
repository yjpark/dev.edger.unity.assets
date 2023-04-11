using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public partial class Assets : Env, ISingleton {
        private static Assets _Instance;
        public static Assets Instance { get => Singleton.GetInstance(ref _Instance); }

#if ODIN_INSPECTOR
        [Button(ButtonSizes.Large)]
#endif
        public static void ClearAllCache() {
            if (Caching.ClearCache()) {
                Log.Info("Assets.ClearAllCache Succeeded");
            } else {
                Log.Error("Assets.ClearAllCache Failed");
            }
        }

        // Aspects
        public AspectReference<AssetsChannel> AssetsChannel { get; private set; }
        public AspectReference<CatalogLoader> CatalogLoader { get; private set; }
        public AspectReference<AssetsPreloader> AssetsPreloader { get; private set; }
        public AspectReference<AssetsSizeCalculator> AssetsSizeCalculator { get; private set; }

        public AspectReference<PrefabLoader> PrefabLoader { get; private set; }
        public AspectReference<TextLoader> TextLoader { get; private set; }
        public AspectReference<BytesLoader> BytesLoader { get; private set; }

        public AspectReference<CacheCleaner> CacheCleaner { get; private set; }

        protected override void OnAwake() {
            Singleton.SetupInstance(ref _Instance, this);

            AssetsChannel = CacheAspect<AssetsChannel>();
            CatalogLoader = CacheAspect<CatalogLoader>();
            AssetsPreloader = CacheAspect<AssetsPreloader>();
            AssetsSizeCalculator = CacheAspect<AssetsSizeCalculator>();

            PrefabLoader = CacheAspect<PrefabLoader>();
            TextLoader = CacheAspect<TextLoader>();
            BytesLoader = CacheAspect<BytesLoader>();

            CacheCleaner = CacheAspect<CacheCleaner>();
        }
    }
}

