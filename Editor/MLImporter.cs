/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Editor {

    using UnityEngine;
    using UnityEditor.Experimental.AssetImporters;
    using System.IO;

    /// <summary>
    /// ONNX model importer.
    /// </summary>
    [ScriptedImporter(1, "onnx")]
    public class MLImporter : ScriptedImporter {

        public override void OnImportAsset (AssetImportContext ctx) {
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.data = File.ReadAllBytes(ctx.assetPath);
            ctx.AddObjectToAsset("MLModelDaa", modelData);
            ctx.SetMainObject(modelData);
        }
    }
}