# Introduction #

DebugPanel provides image viewers for debugging issues. In release version this form should be replaced by panel providing options to bind gesture with action.

In `DebugPanel.cs` there are two function, that can be filled with drawing sample data (DrawHelper methods)

We mantain global value object called `FrameData`. This should provide every necessary variables which can be used in multiple places. If some variable is algorithm-dependent - do not add it into FrameData.

`FaceRecognition.Process` is method, where all image preprocessing should be done.

Gestures recognition algorithms should be binded into `FaceRecognition` constructor. Treat `RotationGesture` as an example.