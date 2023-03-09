using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using OneOf;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public class ContentChannel : Channel<ContentChannel.Evt> {
        public struct DownloadProgress {
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2} }}", Key, Status.DownloadedBytes, Status.TotalBytes);
            }
        }
        public struct DownloadSucceeded {
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2} }}", Key, Status.DownloadedBytes, Status.TotalBytes);
            }
        }
        public struct DownloadFailed {
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }
            public Exception Error { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2}, Error = {3} }}", Key, Status.DownloadedBytes, Status.TotalBytes, Error);
            }
        }
        public class Evt : OneOfBase<DownloadProgress, DownloadSucceeded, DownloadFailed> {
            private Evt(OneOf<DownloadProgress, DownloadSucceeded, DownloadFailed> _) : base(_) {}

            public static implicit operator Evt(DownloadProgress _) => new Evt(_);
            public static implicit operator Evt(DownloadSucceeded _) => new Evt(_);
            public static implicit operator Evt(DownloadFailed _) => new Evt(_);
        }
    }
}