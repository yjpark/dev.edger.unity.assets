using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Edger.Unity;
using Edger.Unity.Context;

namespace Edger.Unity.Addressable {
    public struct ContentLoaderRes {
        public AsyncOperationStatus Status { get; init; }
        public DownloadStatus DownloadStatus { get; init; }
        public Exception Error { get; init; }

        public override string ToString() {
            return string.Format("{{ Status = {0}, DownloadStatus = {1}/{2}, Error = {3} }}", Status, DownloadStatus.DownloadedBytes, DownloadStatus.TotalBytes, Error);
        }
    }
    public abstract class ContentLoader<TReq> : CoroutineHandler<TReq, ContentLoaderRes> {
        public AspectReference<ContentChannel> ContentChannel { get; private set; }

        protected override void OnAwake() {
            ContentChannel = CacheAspect<ContentChannel>();
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
                    ContentChannel.Target.FireEvent(new ContentChannel.DownloadProgress {
                        Key = GetContentKey(req),
                        Status = status,
                    });
                }
                yield return null;
            }
            status = handle.GetDownloadStatus();
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                ContentChannel.Target.FireEvent(new ContentChannel.DownloadSucceeded {
                    Key = GetContentKey(req),
                    Status = status,
                });
            } else {
                ContentChannel.Target.FireEvent(new ContentChannel.DownloadFailed {
                    Key = GetContentKey(req),
                    Status = status,
                    Error = handle.OperationException,
                });
            }
            SetResponse(reqIdentity, new ContentLoaderRes{
                Status = handle.Status,
                DownloadStatus = status,
                Error = handle.OperationException,
            });
            Addressables.Release(handle);
        }

        protected abstract string GetContentKey(TReq req);
        protected abstract AsyncOperationHandle CreateOperationHandle(TReq req);
    }
}