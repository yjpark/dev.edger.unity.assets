using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public partial class Assets : Env, ISingleton {
        private static Assets _Instance;
        public static Assets Instance { get => Singleton.GetInstance(ref _Instance); }

        // Aspects
        public AspectReference<ContentChannel> ContentChannel { get; private set; }
        public AspectReference<CatalogLoader> CatalogLoader { get; private set; }
        public AspectReference<AssetLoader> AssetLoader { get; private set; }

        protected override void OnAwake() {
            Singleton.SetupInstance(ref _Instance, this);

            ContentChannel = CacheAspect<ContentChannel>();
            CatalogLoader = CacheAspect<CatalogLoader>();
            AssetLoader = CacheAspect<AssetLoader>();
        }

        public void ClearCaching() {
            Caching.ClearCache();
        }
    }
}

