/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Editor {

    using System;
    using System.IO;
    using UnityEngine;
    using UnityEditor.Experimental.AssetImporters;

    /// <summary>
    /// NatML model importer.
    /// </summary>
    [ScriptedImporter(1, "onnx")]
    public class MLImporter : ScriptedImporter {

        [Header("Classification")]
        public TextAsset classLabels;

        public override void OnImportAsset (AssetImportContext ctx) {
            // Populate model data
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.graphData = File.ReadAllBytes(ctx.assetPath);
            modelData.imageNormalization = new MLModelData.Normalization { mean = Vector3.zero, std = Vector3.one };
            if (classLabels)
                modelData.classLabels = classLabels.text.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // Import
            ctx.AddObjectToAsset("MLModelData", modelData);
            ctx.SetMainObject(modelData);
        }
    }
}