//
//  NMLTypes.h
//  NatML
//
//  Created by Yusuf Olokoba on 3/14/21.
//  Copyright © 2021 Yusuf Olokoba. All rights reserved.
//

#pragma once

#include <stdint.h>

// Platform defines
#ifdef __cplusplus
    #define BRIDGE extern "C"
#else
    #define BRIDGE
#endif

#ifdef _WIN64
    #define EXPORT __declspec(dllexport)
#else
    #define EXPORT
    #define APIENTRY
#endif


#pragma region --Enumerations--
/*!
 @enum NMLDataType

 @abstract Feature data type.

 @constant NML_DTYPE_UNDEFINED
 Type is undefined or invalid.

 @constant NML_DTYPE_UINT8
 Type is `uint8_t` in C/C++ and `byte` in C#.

 @constant NML_DTYPE_INT16
 Type is `int16_t` in C/C++ and `short` in C#.

 @constant NML_DTYPE_INT32
 Type is `int32_t` in C/C++ and `int` in C#.

 @constant NML_DTYPE_INT64
 Type is `int64_t` in C/C++ and `long` in C#.

 @constant NML_DTYPE_FLOAT
 Type is `float` in C/C++/C#.

 @constant NML_DTYPE_DOUBLE
 Type is `double` in C/C++/C#.

 @constant NML_DTYPE_STRING
 Type is `std::string` in C++ and `string` in C#.

 @constant NML_DTYPE_SEQUENCE
 Type is a sequence.

 @constant NML_DTYPE_DICTIONARY
 Type is a dictionary.
*/
enum NMLDataType {
    NML_DTYPE_UNDEFINED = 0,
    NML_DTYPE_UINT8 = 1,
    NML_DTYPE_INT16 = 2,
    NML_DTYPE_INT32 = 3,
    NML_DTYPE_INT64 = 4,
    NML_DTYPE_FLOAT = 5,
    NML_DTYPE_DOUBLE = 6,
    NML_DTYPE_STRING = 7,
    NML_DTYPE_SEQUENCE = 8,
    NML_DTYPE_DICTIONARY = 9
};
typedef enum NMLDataType NMLDataType;

/*!
 @enum NMLAspectMode

 @abstract Pixel buffer aspect mode.

 @constant NML_ASPECT_MODE_SCALE
 Pixel buffer is scaled to fit feature size.

 @constant NML_ASPECT_MODE_FILL
 Pixel buffer is aspect-filled to the feature size.
*/
enum NMLAspectMode {
    NML_ASPECT_MODE_SCALE = 0,
    NML_ASPECT_MODE_FILL = 1,
};
typedef enum NMLAspectMode NMLAspectMode;
#pragma endregion
