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
    public class AssetsChannel : Channel<AssetsChannel.Evt> {
        public struct OperationProgress {
            public IAssetsOperator Operator { get; init; }
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2} }}", Key, Status.DownloadedBytes, Status.TotalBytes);
            }
        }
        public struct OperationSucceeded {
            public IAssetsOperator Operator { get; init; }
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2} }}", Key, Status.DownloadedBytes, Status.TotalBytes);
            }
        }
        public struct OperationFailed {
            public IAssetsOperator Operator { get; init; }
            public string Key { get; init; }
            public DownloadStatus Status { get; init; }
            public Exception Error { get; init; }

            public override string ToString() {
                return string.Format("{{ Key = {0}, Status = {1}/{2}, Error = {3} }}", Key, Status.DownloadedBytes, Status.TotalBytes, Error);
            }
        }
        public class Evt : OneOfBase<OperationProgress, OperationSucceeded, OperationFailed> {
            private Evt(OneOf<OperationProgress, OperationSucceeded, OperationFailed> _) : base(_) {}

            public static implicit operator Evt(OperationProgress _) => new Evt(_);
            public static implicit operator Evt(OperationSucceeded _) => new Evt(_);
            public static implicit operator Evt(OperationFailed _) => new Evt(_);
        }
    }
}