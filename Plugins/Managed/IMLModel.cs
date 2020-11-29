/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface implemented by all ML models.
    /// </summary>
    public interface IMLModel : IDisposable, IEnumerable<string> { // CHECK // Enumerable on metadata keys

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        string this [string key] { get; }

        /// <summary>
        /// Model inputs.
        /// </summary>
        MLFeature[] inputs { get; }

        /// <summary>
        /// Model outputs.
        /// </summary>
        MLFeature[] outputs { get; }
    }

    /// <summary>
    /// Information about an ML model input or output feature.
    /// </summary>
    public readonly struct MLFeature {

        /// <summary>
        /// Feature name.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Feature type.
        /// This will typically be a numeric type.
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// Feature shape.
        /// </summary>
        public readonly int[] shape;

        /// <summary>
        /// Feature dimensions.
        /// This corresponds to the number of elements in the feature shape.
        /// </summary>
        public int dimensions => shape?.Length ?? 0; // Mark `readonly` in C# 8
    }

    /// <summary>
    /// Common ML model metadata keys.
    /// </summary>
    public static class MLMetadata {

        /// <summary>
        /// </summary>
        public static readonly string Name = @"__Name";

        /// <summary>
        /// </summary>
        public static readonly string Author = @"__Author";

        /// <summary>
        /// </summary>
        public static readonly string Description = @"__Description";
        
        /// <summary>
        /// </summary>
        public static readonly string Version = @"__Version";
    }
}