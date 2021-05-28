# NatML
NatML is a high-performance cross-platform machine learning runtime for Unity Engine. Designed for app and game developers, NatML completely removes the need to have any experience with machine learning in order to take advantage of the feautres that they can provide. Features include:

- **Bare Metal Performance**. NatML takes advantage of hardware machine learning accelerators, like CoreML on iOS and macOS, NNAPI on Android, and DirectML on Windows. As a result, it is [multiple times faster](https://github.com/natsuite/ML-Bench) than even Unity's Barracuda engine.

- **Extremely Easy to Use**. NatML exposes machine learning models with simple functions that return familiar data types, with all conversions to and from the model handled for you. No need to write pre-processing scripts or shaders, wrangle tensors, or anything of that sort.

- **Full Support for ONNX**. NatML supports the full ONNX specification, with all layers supported on the CPU, and a large number supported on dedicated hardware accelerators. You can drag and drop any ONNX model from anywhere and run it, no conversions needed.

- **Cross Platform**. NatML supports Android, iOS, macOS, and Windows alike. As a result, you can build your app once, test it in the Editor, and deploy it to the device all in one seamless workflow.

- **Growing Ecosystem**. NatML is designed with a singular focus on applications. As a result, there is a growing ecosystem of open-source and paid packages for ML models and applications that run on NatML. You can also [publish your own NatML packages](https://docs.natsuite.io/natml/advanced/authoring).

- **Computer Vision**. NatML supports the most common machine learning use-cases in computer vision. Currently, there are packages for [object classification](https://github.com/natsuite/ML-Classifier), [object detection](https://github.com/natsuite/ML-Detector), [semantic segmentation](https://github.com/natsuite/ML-Segmenter), and [style transfer](https://github.com/natsuite/ML-Style). More packages are coming!

- **Augmented Reality**. NatML is particularly suited for augmented reality because it delegates work to ML accelerators, freeing up the GPU to render your app smoothly.

- **Lightweight Package**. NatML is distributed in a self-contained package, with no external dependencies. As a result, you can simply import the package and get going--no setup necessary.

## Using NatML in Three Simple Steps
You will always use NatML in three steps. First, create a **model** from model data:
```csharp
// Create a model from model data
var modelData = await MLModelData.FromFile("/path/to/model.onnx");
var model = modelData.Deserialize();
```

Then create a **predictor** in order to make predictions with the model:
```csharp
// Create a classification predictor
var predictor = new MLClassificationPredictor(model, modelData.labels);
```

Finally, make predictions on one or more input **features**:
```csharp
// Make prediction on an image
Texture2D image = ...;
var (label, confidence) = predictor.Predict(image);
```

## Using Models in the Wild
Because there is a massive selection of ML models for different tasks, NatML relies on its developer ecosystem to write and distribute their custom models and predictors. We encourage developers to do this, whether as open-source predictor packages or as paid packages on the Asset Store.

[Visit the online documentation](https://docs.natsuite.io/natml/advanced/authoring) for more information on authoring predictors. If you need a model for a specific problem, we provide quick and affordable model development services for developers.

___

## Requirements
- Unity 2019.2+
- Android API Level 24+
- iOS 13+
- macOS 10.15+
- Windows 10+, 64-bit only

## Resources
- Join the [NatSuite community on Discord](https://discord.gg/y5vwgXkz2f).
- See the [NatML documentation](https://docs.natsuite.io/natml).
- See more [NatSuite projects on GitHub](https://github.com/natsuite).
- Read the [NatSuite blog](https://blog.natsuite.io/).
- Discuss [NatML on Unity Forums](https://forum.unity.com/threads/open-beta-natml-machine-learning-runtime.1109339/).
- Contact us at [hi@natsuite.io](mailto:hi@natsuite.io).

Thank you very much!