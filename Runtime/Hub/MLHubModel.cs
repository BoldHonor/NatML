/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System;

    internal sealed class MLHubModel : MLModel {

        #region --Operations--

        private readonly string session;

        internal MLHubModel (string session, byte[] data) : base(data) => this.session = session;

        private protected override IntPtr[] Predict (params IntPtr[] inputs) => base.Predict(inputs);
        #endregion
    }
}