using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public struct AssetsOperatorRes {
        public AsyncOperationStatus Status { get; init; }
        public DownloadStatus DownloadStatus { get; init; }
        public Exception Error { get; init; }

        public override string ToString() {
            return string.Format("{{ Status = {0}, DownloadStatus = {1}/{2}, Error = {3} }}", Status, DownloadStatus.DownloadedBytes, DownloadStatus.TotalBytes, Error);
        }
    }

    public interface IAssetsOperator {

    }

    public abstract class AssetsOperator<TReq> : CoroutineHandler<TReq, AssetsOperatorRes>, IAssetsOperator {
        public AspectReference<AssetsChannel> AssetsChannel { get; private set; }

        protected override void OnAwake() {
            AssetsChannel = CacheAspect<AssetsChannel>();
        }

        protected override IEnumerator DoHandleAsync(int reqIdentity, DateTime reqTime, TReq req) {
            var handle = CreateOperationHandle(req);
            yield return null;
            long lastDownloadedBytes = 0;
            DownloadStatus status;
            while (!handle.IsDone) {
                status = handle.GetDownloadStatus();
                if (status.DownloadedBytes > lastDownloadedBytes) {
                    lastDownloadedBytes = status.DownloadedBytes;
                    AssetsChannel.Target.FireEvent(new AssetsChannel.OperationProgress {
                        Operator = this,
                        Key = GetOperationKey(req),
                        Status = status,
                    });
                }
                yield return null;
            }
            status = handle.GetDownloadStatus();
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                AssetsChannel.Target.FireEvent(new AssetsChannel.OperationSucceeded {
                    Operator = this,
                    Key = GetOperationKey(req),
                    Status = status,
                });
            } else {
                AssetsChannel.Target.FireEvent(new AssetsChannel.OperationFailed {
                    Operator = this,
                    Key = GetOperationKey(req),
                    Status = status,
                    Error = handle.OperationException,
                });
            }
            SetResponse(reqIdentity, new AssetsOperatorRes{
                Status = handle.Status,
                DownloadStatus = status,
                Error = handle.OperationException,
            });
            Addressables.Release(handle);
        }

        protected abstract string GetOperationKey(TReq req);
        protected abstract AsyncOperationHandle CreateOperationHandle(TReq req);
    }
}