/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;

    public enum NMLFeatureType : int { // CHECK // Must match `NatML.h`
        Undefined = 0,
        UInt8 = 1,
        Int16 = 2,
        Int32 = 3,
        Int64 = 4,
        Float = 5,
        Double = 6,
        String = 7,
        Sequence = 8,
        Dictionary = 9
    }

    [Flags]
    public enum NMLTensorFlag : int {
        PixelBuffer = 1 << 0
    }

    public static partial class Bridge {

        public static NMLFeatureType NativeType (this Type type) {
            switch (type) {
                case var t when t == typeof(byte): return NMLFeatureType.UInt8;
                case var t when t == typeof(short): return NMLFeatureType.Int16;
                case var t when t == typeof(int): return NMLFeatureType.Int32;
                case var t when t == typeof(long): return NMLFeatureType.Int64;
                case var t when t == typeof(float): return NMLFeatureType.Float;
                case var t when t == typeof(double): return NMLFeatureType.Double;
                case var t when t == typeof(string): return NMLFeatureType.String;
                case var t when t.IsAssignableFrom(typeof(IList)): return NMLFeatureType.Sequence;
                case var t when t.IsAssignableFrom(typeof(IDictionary)): return NMLFeatureType.Dictionary;
                default: return NMLFeatureType.Undefined;
            }
        }

        public static Type ManagedType (this NMLFeatureType type) {
            switch (type) {
                case NMLFeatureType.UInt8: return typeof(byte);
                case NMLFeatureType.Int16: return typeof(short);
                case NMLFeatureType.Int32: return typeof(int);
                case NMLFeatureType.Int64: return typeof(long);
                case NMLFeatureType.Float: return typeof(float);
                case NMLFeatureType.Double: return typeof(double);
                case NMLFeatureType.String: return typeof(string);
                case NMLFeatureType.Sequence: return typeof(IList);
                case NMLFeatureType.Dictionary: return typeof(IDictionary);
                default: return null;
            }
        }
    }
}