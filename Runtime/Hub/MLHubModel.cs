/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System;
    using Stopwatch = System.Diagnostics.Stopwatch;

    internal sealed class MLHubModel : MLModel {

        #region --Operations--

        private readonly string id;

        internal MLHubModel (string id, byte[] data) : base(data) => this.id = id;

        private protected override IntPtr[] Predict (params IntPtr[] inputs) {
            var watch = Stopwatch.StartNew();
            var outputs = base.Predict(inputs);
            Report(watch.Elapsed.TotalMilliseconds);
            return outputs;
        }

        private void Report (double latency) { // INCOMPLETE

        }
        #endregion
    }
}