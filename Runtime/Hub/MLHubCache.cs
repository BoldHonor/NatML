/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System;
    using System.IO;
    using UnityEngine;
    using Features;

    public static class MLHubCache {

        #region --Client API--
        public static MLModelData LoadModelData (string tag) {
            // Check
            var cacheName = tag.Replace('/', '_');
            var cachePath = Path.Combine(Application.persistentDataPath, "ML", $"{cacheName}.nml");
            if (!File.Exists(cachePath))
                return null;
            // Load
            var cachedData = JsonUtility.FromJson<MLCachedData>(File.ReadAllText(cachePath));
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.data = File.ReadAllBytes(cachedData.data);
            modelData.classLabels = cachedData.labels.Length != 0 ? cachedData.labels : null;
            modelData.imageNormalization = cachedData.normalization;
            modelData.imageAspectMode = cachedData.aspectMode;
            return modelData;
        }

        public static void SaveModelData (MLModelData modelData) { // INCOMPLETE

        }
        #endregion


        #region --Operations--

        [Serializable]
        private struct MLCachedData {
            public string data;
            public string[] labels;
            public MLModelData.Normalization normalization;
            public MLImageFeature.AspectMode aspectMode;
        }
        #endregion
    }
}