/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using UnityEditor.iOS.Xcode;
    using UnityEditor.iOS.Xcode.Extensions;
    using UnityEngine;

    public class Build : IPreprocessBuildWithReport { // INCOMPLETE

        int IOrderedCallback.callbackOrder => 0;

        void IPreprocessBuildWithReport.OnPreprocessBuild (BuildReport report) {
            // Check if iOS or macOS
            // Install `coremltools` with `pip`
            // convert onnx to coreml
            // bundle coreml proto with project
        }
    }
}