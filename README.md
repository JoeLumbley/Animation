# Animation
This project showcases the fundamentals of creating smooth animations. The application features a moving rectangle that glides across the screen, illustrating key concepts such as frame independence and real-time rendering.



![010](https://github.com/user-attachments/assets/d871475e-c09c-42d6-bb59-693aca7bb7b4)




In this app, you'll learn how to manage timing, handle graphics rendering, and implement basic animation principles. Whether you're a beginner looking to understand the basics of animation or an experienced developer seeking a refresher, this project provides a hands-on approach to mastering animation techniques.



# Animation Code Walkthrough

Welcome to the Animation project! In this lesson, we will break down the code line by line, helping you understand how it works. This project demonstrates the fundamentals of creating smooth animations using a Windows Form application. Let's dive right in!

---

## Overview

Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession. In our app, we use animation to make it appear as though our rectangle is moving towards the right. To ensure that our animation runs smoothly on all devices, we have designed it to be frame-independent. This means that our animation is not affected by changes in the frame rate, ensuring a consistent and seamless experience for all users.

### License Information

This code is shared under the MIT License, which allows you to use, modify, and distribute it freely, as long as you include the original copyright notice.

---

## Code Breakdown

```vb
Option Explicit On
```
- This line ensures that all variables must be declared before they are used. It helps prevent errors caused by typos in variable names.

```vb
Public Class Form1
```
- This line defines a new class named `Form1`, which represents our main application window. All the code inside this class will define the behavior and appearance of our form.

### Variables and Structures

#### Buffered Graphics Context

```vb
Private Context As New BufferedGraphicsContext
Private Buffer As BufferedGraphics
```
- `Context` is an instance of `BufferedGraphicsContext`, which manages the buffered graphics.
- `Buffer` is an instance of `BufferedGraphics`, which will hold our drawing operations before rendering them on the screen.

#### Screen Size and Colors

```vb
Private ReadOnly MinimumMaxBufferSize As New Size(1280, 720)
Private ReadOnly BackgroundColor As Color = Color.Black
Private ReadOnly RectangleBrush As New SolidBrush(Color.Orchid)
Private ReadOnly FpsDisplayBrush As New SolidBrush(Color.MediumOrchid)
Private ReadOnly FpsIdentifier As New String(" FPS")
```
- `MinimumMaxBufferSize` sets the minimum size for our graphics buffer.
- `BackgroundColor` defines the color of the background (black).
- `RectangleBrush` defines the color of the rectangle (orchid).
- `FpsDisplayBrush` defines the color for the frames per second (FPS) display (medium orchid).
- `FpsIdentifier` is a string that will be appended to our FPS count for display purposes.

### Rectangle Structure

```vb
Public Structure RectangleDouble
    Public X, Y, Width, Height As Double
```
- This defines a structure named `RectangleDouble` that represents a rectangle with double-precision coordinates and dimensions.

```vb
Public Sub New(x As Double, y As Double, width As Double, height As Double)
    Me.X = x
    Me.Y = y
    Me.Width = width
    Me.Height = height
End Sub
```
- This is the constructor for the `RectangleDouble` structure. It initializes a new rectangle with specified values for its `X`, `Y`, `Width`, and `Height`.

#### Rounding Methods

```vb
Public Function GetNearestX() As Integer
    Return Math.Round(X)
End Function
```
- This method rounds the `X` coordinate to the nearest integer. Similar methods exist for `Y`, `Width`, and `Height`.

### Delta Time Structure

```vb
Private Structure DeltaTimeStructure
    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan
```
- This structure tracks the timing information for our animation, including the current and last frame times and the elapsed time between them.

```vb
Public Sub New(currentFrame As Date, lastFrame As Date, elapsedTime As TimeSpan)
    Me.CurrentFrame = currentFrame
    Me.LastFrame = lastFrame
    Me.ElapsedTime = elapsedTime
End Sub
```
- This constructor initializes the `DeltaTimeStructure` with the current and last frame times.

### Other Structures

#### Display Structure

```vb
Private Structure DisplayStructure
    Public Location As Point
    Public Text As String
    Public Font As Font
```
- This structure holds information about display elements, including their location, text, and font.

#### Frame Counter Structure

```vb
Private Structure FrameCounterStructure
    Public FrameCount As Integer
    Public StartTime As DateTime
    Public TimeElapsed As TimeSpan
    Public SecondsElapsed As Double
```
- This structure counts the frames rendered and tracks the timing for calculating frames per second (FPS).

---

### Event Handlers

#### Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApp()
    Debug.Print($"Running...{DateTime.Now}")
End Sub
```
- This method initializes the application when the form loads. It calls `InitializeApp()` to set up the necessary components and prints the current time to the debug console.

#### Form Resize Event

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
    If Not WindowState = FormWindowState.Minimized Then
        ResizeFPS()
        ResizeRectangle()
        DisposeBuffer()
    End If
End Sub
```
- This method handles resizing the window. It adjusts the FPS display and rectangle size accordingly, ensuring everything looks good when the window is resized.

---

### Timer Tick Event

```vb
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    If Not WindowState = FormWindowState.Minimized Then
        UpdateFrame()
        Invalidate() ' Calls OnPaint Sub
    End If
End Sub
```
- This event is triggered at regular intervals (every 15 milliseconds). It updates the animation frame and redraws the form.

---

### Painting the Frame

#### OnPaint Method

```vb
Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
    AllocateBuffer()
    DrawFrame()
    Buffer?.Render(e.Graphics)
    UpdateFrameCounter()
    MyBase.OnPaint(e)
End Sub
```
- This method handles the rendering of graphics. It allocates a buffer, draws the current frame, and updates the frame counter. The buffer is then rendered onto the form.

#### Drawing the Frame

```vb
Private Sub DrawFrame()
    Buffer?.Graphics.Clear(BackgroundColor)
    Buffer?.Graphics.FillRectangle(RectangleBrush, Rectangle.GetNearestX, Rectangle.GetNearestY, Rectangle.GetNearestWidth, Rectangle.GetNearestHeight)
    Buffer?.Graphics.DrawString(FPSDisplay.Text, FPSDisplay.Font, FpsDisplayBrush, FPSDisplay.Location)
End Sub
```
- This method clears the buffer with the background color, fills a rectangle with the specified brush, and draws the FPS display string.

---

### Updating the Animation

#### MoveRectangle Method

```vb
Private Sub MoveRectangle()
    Rectangle.X += Velocity * DeltaTime.ElapsedTime.TotalSeconds
    If Rectangle.X > ClientRectangle.Right Then
        Rectangle.X = ClientRectangle.Left - Rectangle.Width
    End If
End Sub
```
- This method updates the rectangle's position based on its velocity and the elapsed time. If the rectangle moves off the right edge, it reappears on the left.

---

### Initialization Methods

#### InitializeApp Method

```vb
Private Sub InitializeApp()
    InitializeForm()
    InitializeBuffer()
    Timer1.Interval = 15
    Timer1.Start()
End Sub
```
- This method initializes the application, setting up the form and buffer, and starts the timer.

---



This walkthrough covers the main components of the animation project. Feel free to experiment with the code, adjust parameters, and see how they affect the animation! 

---





## Related Projects

If you're interested in exploring a similar project, check out **Animation C#**, which is a port of this animation project. You can find the C# version in its repository: [Animation C# Repository](https://github.com/JoeLumbley/Animation-CS). 

For more information about the original project, visit the [Animation Repository](https://github.com/JoeLumbley/Animation). 

Happy coding!




![012](https://github.com/user-attachments/assets/d50c1b9e-690e-422e-8be5-620a96b295ab)
















