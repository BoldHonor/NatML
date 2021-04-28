/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLAsyncModule<TOutput> : MLModule<Task<TOutput>> {

        #region --Client API--
        /// <summary>
        /// Model input feature types.
        /// </summary>
        public override MLFeatureType[] inputs => module.inputs;

        /// <summary>
        /// Model output feature types.
        /// </summary>
        public override MLFeatureType[] outputs => module.outputs;

        /// <summary>
        /// </summary>
        public bool readyForPrediction {
            [MethodImpl(MethodImplOptions.Synchronized)] get;
            [MethodImpl(MethodImplOptions.Synchronized)] private set;
        }

        /// <summary>
        /// </summary>
        /// <param name="module"></param>
        public MLAsyncModule (MLModule<TOutput> module) : this(module.Predict) { }

        /// <summary>
        /// </summary>
        /// <param name="predictor"></param>
        public MLAsyncModule (Func<MLFeature[], TOutput> predictor) : base("") {
            // Check
            
            // Save
            this.module = predictor.Target as IMLModule;
            this.queue = new ConcurrentQueue<(MLFeature[], TaskCompletionSource<TOutput>)>();
            this.cts = new CancellationTokenSource();
            this.task = new Task(() => {
                while (!cts.Token.IsCancellationRequested)
                    if (queue.TryDequeue(out var request)) {
                        readyForPrediction = false;
                        request.Item2.SetResult(predictor(request.Item1));
                        readyForPrediction = true;
                    }
            }, cts.Token, TaskCreationOptions.LongRunning);
            // Start
            task.Start();
            readyForPrediction = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override Task<TOutput> Predict (params MLFeature[] inputs) {
            var tcs = new TaskCompletionSource<TOutput>();
            queue.Enqueue((inputs, tcs));
            return tcs.Task;
        }

        /// <summary>
        /// </summary>
        public override void Dispose () {
            cts.Cancel();
            task.Wait();
            module.Dispose();
        }
        #endregion


        #region --Operations--
        private readonly IMLModule module;
        private readonly ConcurrentQueue<(MLFeature[], TaskCompletionSource<TOutput>)> queue;
        private readonly CancellationTokenSource cts;
        private readonly Task task;
        #endregion
    }
}