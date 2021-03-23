/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// </summary>
    public sealed class MLDispatcher<TResult> : IDisposable {

        #region --Client API--
        /// <summary>
        /// </summary>
        public bool readyForPrediction {
            [MethodImpl(MethodImplOptions.Synchronized)] get;
            [MethodImpl(MethodImplOptions.Synchronized)] private set;
        }

        /// <summary>
        /// </summary>
        /// <param name="runner"></param>
        public MLDispatcher (Func<MLFeature[], TResult> runner) { // DEPLOY
            var context = SynchronizationContext.Current;
            this.queue = new Queue<(MLFeature[], Action<TResult>)>();
            this.cts = new CancellationTokenSource();
            this.task = new Task(() => {
                while (!cts.Token.IsCancellationRequested) {
                    // Dequeue
                    MLFeature[] inputs;
                    Action<TResult> handler;
                    lock ((queue as ICollection).SyncRoot)
                        if (queue.Count > 0)
                            (inputs, handler) = queue.Dequeue();
                        else
                            continue;
                    // Run
                    readyForPrediction = false;
                    var result = runner(inputs);
                    readyForPrediction = true;
                    // Invoke handler
                    context.Post(state => handler((TResult)state), result);
                }
                UnityEngine.Debug.Log("Dispatcher exiting");
            }, cts.Token, TaskCreationOptions.LongRunning);
            this.readyForPrediction = true;
            // Start
            task.Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public Task<TResult> Predict (params MLFeature[] inputs) {
            var tcs = new TaskCompletionSource<TResult>();
            Predict(inputs, tcs.SetResult);
            return tcs.Task;
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="handler"></param>
        public void Predict (MLFeature input, Action<TResult> handler) => Predict(new [] { input }, handler);

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="handler"></param>
        public void Predict (MLFeature[] inputs, Action<TResult> handler) { // DEPLOY
            lock ((queue as ICollection).SyncRoot)
                queue.Enqueue((inputs, handler));
        }

        /// <summary>
        /// </summary>
        public void Dispose () { // Blocks until complete
            cts.Cancel();
            task.Wait();
        }
        #endregion


        #region --Operations--
        private readonly Queue<(MLFeature[], Action<TResult>)> queue;
        private readonly CancellationTokenSource cts;
        private readonly Task task;
        #endregion
    }
}