/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Editor {

    using UnityEngine;
    using UnityEditor.Experimental.AssetImporters;
    using System.IO;

    /// <summary>
    /// ONNX model importer
    /// </summary>
    [ScriptedImporter(1, "onnx")]
    public class ModelImporter : ScriptedImporter {

        public override void OnImportAsset (AssetImportContext ctx) {
            var modelData = File.ReadAllBytes(ctx.assetPath);
        }
    }
}