using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public struct AssetsOperatorRes<T> {
        public AsyncOperationStatus Status { get; init; }
        public DownloadStatus DownloadStatus { get; init; }
        public T Result { get; init; }
        public Exception Error { get; init; }

        public override string ToString() {
            return string.Format("{{ Status = {0}, DownloadStatus = {1}/{2}, Error = {3} }}", Status, DownloadStatus.DownloadedBytes, DownloadStatus.TotalBytes, Error);
        }
    }

    public interface IAssetsOperator {

    }

    public abstract class AssetsOperator<TReq, TResult> : CoroutineHandler<TReq, AssetsOperatorRes<TResult>>, IAssetsOperator {
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
                if (FireProgressEvents()) {
                    status = handle.GetDownloadStatus();
                    if (status.DownloadedBytes > lastDownloadedBytes) {
                        lastDownloadedBytes = status.DownloadedBytes;
                        AssetsChannel.Target.FireEvent(new AssetsChannel.OperationProgress {
                            Operator = this,
                            Key = GetOperationKey(req),
                            Status = status,
                        });
                    }
                }
                yield return null;
            }
            status = handle.GetDownloadStatus();
            var result = GetResult(handle);
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                AssetsChannel.Target.FireEvent(new AssetsChannel.OperationSucceeded {
                    Operator = this,
                    Key = GetOperationKey(req),
                    Status = status,
                    Result = result,
                });
            } else {
                AssetsChannel.Target.FireEvent(new AssetsChannel.OperationFailed {
                    Operator = this,
                    Key = GetOperationKey(req),
                    Status = status,
                    Error = handle.OperationException,
                });
            }
            SetResponse(reqIdentity, new AssetsOperatorRes<TResult>{
                Status = handle.Status,
                DownloadStatus = status,
                Result = result,
                Error = handle.OperationException,
            });
            Addressables.Release(handle);
        }

        protected virtual TResult GetResult(AsyncOperationHandle handle) {
            if (handle.Status == AsyncOperationStatus.Succeeded
                && handle.Is<AsyncOperationHandle<TResult>>()) {
                return handle.Convert<TResult>().Result;
            } else {
                return default(TResult);
            }
        }

        protected virtual bool FireProgressEvents() {
            return false;
        }
        protected abstract string GetOperationKey(TReq req);
        protected abstract AsyncOperationHandle CreateOperationHandle(TReq req);
    }
}