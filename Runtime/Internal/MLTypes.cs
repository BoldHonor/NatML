/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;

    public enum MLFeatureType : int { // CHECK // Must match `NatML.h`
        Undefined = 0,
        Int16 = 1,
        Int32 = 2,
        Int64 = 3,
        Float = 4,
        Double = 5,
        String = 6,
        Sequence = 7,
        Dictionary = 8
    }

    [Flags]
    public enum MLTensorFlag : int {
        PixelBuffer = 1 << 0
    }

    public static partial class Bridge {

        public static MLFeatureType NativeType (this Type type) {
            switch (type) {
                case var t when t == typeof(short): return MLFeatureType.Int16;
                case var t when t == typeof(int): return MLFeatureType.Int32;
                case var t when t == typeof(long): return MLFeatureType.Int64;
                case var t when t == typeof(float): return MLFeatureType.Float;
                case var t when t == typeof(double): return MLFeatureType.Double;
                case var t when t == typeof(string): return MLFeatureType.String;
                case var t when t.IsAssignableFrom(typeof(IList)): return MLFeatureType.Sequence;
                case var t when t.IsAssignableFrom(typeof(IDictionary)): return MLFeatureType.Dictionary;
                default: return 0;
            }
        }

        public static Type ManagedType (this MLFeatureType type) {
            switch (type) {
                case MLFeatureType.Int16: return typeof(short);
                case MLFeatureType.Int32: return typeof(int);
                case MLFeatureType.Int64: return typeof(long);
                case MLFeatureType.Float: return typeof(float);
                case MLFeatureType.Double: return typeof(double);
                case MLFeatureType.String: return typeof(string);
                case MLFeatureType.Sequence: return typeof(IList);
                case MLFeatureType.Dictionary: return typeof(IDictionary);
                default: return null;
            }
        }
    }
}