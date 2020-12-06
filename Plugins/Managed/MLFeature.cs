/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Information about an ML model input or output feature.
    /// </summary>
    public readonly struct MLFeature {

        #region --Client API--
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
        #endregion


        #region --Operations--

        internal MLFeature (StringBuilder name, int type, long[] shape) {
            this.name = name.ToString();
            this.type = TypeForNativeType(type);
            this.shape = shape.Cast<int>().ToArray();
        }

        private static Type TypeForNativeType (int type) {
            switch (type) {
                case 0: goto case default;
                case 1: return typeof(short);
                case 2: return typeof(int);
                case 3: return typeof(long);
                case 4: return typeof(float);
                case 5: return typeof(double);
                case 6: return typeof(string);
                case 7: return typeof(IList);
                case 8: return typeof(IDictionary);
                default: return null;
            }
        }
        #endregion
    }
}