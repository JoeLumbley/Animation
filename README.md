# Animation
**Animation** is the process of creating the illusion of movement by displaying a series of static images rapidly.



![003](https://github.com/user-attachments/assets/5679d72b-eb92-4403-b09b-a552a5b54f89)





# Code Walkthrough

This code demonstrates how to create a simple animation in a Windows Forms application using Visual Basic .NET.

The animation consists of a rectangle that moves horizontally across the screen, giving the illusion of motion. 

This walkthrough will break down the code line by line.

## Table of Contents

1. [Animation Basics](#animation-basics)
2. [Code Structure](#code-structure)
3. [Line-by-Line Explanation](#line-by-line-explanation)
4. [Conclusion](#conclusion)

---

## Animation Basics

**Animation** is the process of creating the illusion of movement by displaying a series of static images rapidly. In this app, we animate a rectangle moving to the right. The animation is designed to be frame-independent, meaning it will run smoothly regardless of the device's frame rate.

## Code Structure

The code is structured as follows:

- **Imports**: Libraries needed for drawing and numerical operations.
- **Class Definition**: Contains all the properties and methods for our form.
- **Event Handlers**: Respond to user actions like loading the form and resizing it.
- **Rendering Methods**: Handle the drawing and updating of the animation.

## Line-by-Line Explanation

### Imports

```vb
Imports System.Drawing.Drawing2D
Imports System.Numerics
```
- These lines import necessary libraries for advanced drawing and mathematical operations.

### Class Definition

```vb
Public Class Form1
```
- This defines a new class called `Form1`, which represents our main application window.

### Variables Declaration

```vb
Private Context As New BufferedGraphicsContext
Private Buffer As BufferedGraphics
Private FrameCount As Integer = 0
Private StartTime As DateTime = Now
Private TimeElapsed As TimeSpan
Private SecondsElapsed As Double = 0
Private FPS As Integer = 0
```
- **Context**: Manages the graphics buffer for smoother rendering.
- **Buffer**: Stores the graphics to be displayed.
- **FrameCount**: Counts frames to calculate frames per second (FPS).
- **StartTime**: Records when the animation starts.
- **TimeElapsed**: Measures the time that has passed.
- **SecondsElapsed**: Stores the total seconds elapsed.
- **FPS**: Holds the frames per second value.

### Rectangle and Position

```vb
Private Rect As New Rectangle(0, 100, 256, 256)
Private RectPostion As New Vector2(Rect.X, Rect.Y)
```
- **Rect**: Defines a rectangle starting at (0, 100) with a width and height of 256 pixels.
- **RectPostion**: Stores the current position of the rectangle using a vector.

### Frame Timing

```vb
Private CurrentFrame As DateTime = Now
Private LastFrame As DateTime = CurrentFrame
Private DeltaTime As TimeSpan = CurrentFrame - LastFrame
Private Velocity As Single = 100.0F
```
- **CurrentFrame**: Current time for the frame.
- **LastFrame**: Time of the previous frame.
- **DeltaTime**: Time difference between frames, used to maintain smooth movement.
- **Velocity**: Speed at which the rectangle moves (100 pixels per second).

### Form Load and Resize Events

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApp()
End Sub
```
- This method initializes the application when the form loads.

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
    If Not WindowState = FormWindowState.Minimized Then
        FPS_Postion.Y = ClientRectangle.Bottom - 75
        RectPostion.Y = ClientRectangle.Height \ 2 - Rect.Height \ 2
        DisposeBuffer()
    End If
End Sub
```
- Adjusts the position of the FPS display and centers the rectangle when the form is resized.

### Timer Tick Event

```vb
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    If Not WindowState = FormWindowState.Minimized Then
        UpdateFrame()
        Invalidate() ' Calls OnPaint Sub
    End If
End Sub
```
- This method is called at regular intervals to update the animation and refresh the display.

### Paint Events

```vb
Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
    AllocateBuffer()
    DrawFrame()
    Buffer.Render(e.Graphics)
    UpdateFrameCounter()
    MyBase.OnPaint(e)
End Sub
```
- Handles the painting of the form. It allocates the buffer, draws the current frame, and renders it.

### Update Frame Method

```vb
Private Sub UpdateFrame()
    UpdateDeltaTime()
    MoveRectangle()
End Sub
```
- Updates the time since the last frame and moves the rectangle accordingly.

### Rectangle Movement

```vb
Private Sub MoveRectangle()
    RectPostion.X += Velocity * DeltaTime.TotalSeconds
    If RectPostion.X > ClientRectangle.Right Then
        RectPostion.X = ClientRectangle.Left - Rect.Width
    End If
End Sub
```
- Moves the rectangle to the right based on the velocity and delta time. If it moves off the screen, it reappears on the left.

### Frame Counter Update

```vb
Private Sub UpdateFrameCounter()
    TimeElapsed = Now.Subtract(StartTime)
    SecondsElapsed = TimeElapsed.TotalSeconds
    If SecondsElapsed < 1 Then
        FrameCount += 1
    Else
        FPS = FrameCount
        FrameCount = 0
        StartTime = Now
    End If
End Sub
```
- Counts frames to calculate the FPS, resetting every second.


### Conclusion
This code provides a foundational understanding of creating animations in a Windows Forms application. By breaking down each section, beginners can grasp how animation works, from initializing graphics to updating and rendering frames. Feel free to experiment with the code to see how changes affect the animation!

---

















## Frame Independent


To ensure that our animation runs smoothly on all devices, we have designed it to be frame independent.

This means that our animation is not affected by changes in the frame rate,
ensuring a consistent and seamless experience for all users.





## Time-Based Motion

To implement time-based motion, we use.


### DeltaTime
```
Private DeltaTime As TimeSpan
```
DeltaTime is the time since the last frame was drawn.

```
DeltaTime = CurrentFrame - LastFrame
```

### Velocity

```
Private Velocity As Single = 250.0F
```

Velocity is the speed we want our rectangle to move, measured in pixels per second.


We can adjust this value to change the speed of our rectangle.




### Distance

We calculate the distance that our rectangle should move based on its velocity multiplied by deltaTime.

Distance = Velocity * DeltaTime

This ensures that our rectangle moves at a consistent speed regardless of the frame rate.

For example, if the frame rate is low, deltaTime will be larger, and our rectangle will move a larger distance in that frame to compensate.

| Frames per Second | DeltaTime | Distance |
| --- | --- | --- |
| 428.1 High | 0.0023366 Seconds | 0.58415 Pixels|
| 38.8 Low | 0.0257694 Seconds | 6.44235 Pixels|

### RectPostion.X

RectPosition is a Vector2 that represents the position of our rectangle.

```
Private RectPostion As New Vector2(Rect.X, Rect.Y)
```

![Vec2XY_3](https://github.com/JoeLumbley/Animation/assets/77564255/3ac4b43b-50f6-45ce-ae1b-34bd28714d6e)



We are using a Vector2 because it can store single-precision floating-point values, which allows for more precise positioning of our rectangle.

Example:

69.7753143 is a single-precision floating-point number (Single).

This is important for our animation as the position of our rectangle is updated rapidly and with high precision.

```
RectPostion.X += Velocity * DeltaTime.TotalSeconds 'Δs = V * Δt
```

We use vector arithmetic to calculate the new position of our rectangle based on its current position and its velocity.

The += operator is used to update the value of RectPostion.X in place, adding the result of velocity * deltaTime to its current value. 


  


### Rect.X
```
Private Rect As New Rectangle(0, 100, 300, 300)
```
When drawing our rectangle to the screen we use the integer representation of its position rather than the Vector2 representation.

This ensures that the position of our rectangle is always aligned with the pixels on the screen.

We do this to avoid visual artifacts such as blurring or jagged edges.

We use Math.Round to round the X component of RectPosition to the nearest integer value.

```
Rect.X = Math.Round(RectPostion.X)
```

Example:

The value of RectPostion.X is 69.7753143

The nearest integer value is 70

So we set Rect.X to 70

```
FillRectangle(Brushes.Purple, Rect)
```

The integer value is then used to draw our rectangle on our form using the FillRectangle function.



