# Animation
This project showcases the fundamentals of creating smooth animations. The application features a moving rectangle that glides across the screen, illustrating key concepts such as frame independence and real-time rendering.



![010](https://github.com/user-attachments/assets/d871475e-c09c-42d6-bb59-693aca7bb7b4)




In this app, you'll learn how to manage timing, handle graphics rendering, and implement basic animation principles. Whether you're a beginner looking to understand the basics of animation or an experienced developer seeking a refresher, this project provides a hands-on approach to mastering animation techniques.



# Code Walkthrough

Join us as we explore the code and uncover the magic behind bringing static images to life!

## Overview of Animation

Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession. In our app, we use animation to make it appear as though our rectangle is moving towards the right. We ensure that our animation runs smoothly on all devices by making it frame-independent, meaning it isn't affected by changes in frame rate.

## Code Structure

### Class Declaration

```vb
Public Class Form1
```
- This line defines a class named `Form1`, which represents our main application window.

### RectangleDouble Structure

```vb
Public Structure RectangleDouble
    Public X As Double
    Public Y As Double
    Public Width As Double
    Public Height As Double
```
- Here, we define a structure called `RectangleDouble` that holds the properties of a rectangle: its position (`X`, `Y`) and its dimensions (`Width`, `Height`).

#### Constructor

```vb
Public Sub New(x As Double, y As Double, width As Double, height As Double)
    Me.X = x
    Me.Y = y
    Me.Width = width
    Me.Height = height
End Sub
```
- This constructor initializes a new rectangle with specified values for its properties.

#### Methods to Get Nearest Integer Values

```vb
Public Function GetNearestX() As Integer
    Return Math.Round(X)
End Function
```
- This method rounds the `X` coordinate to the nearest integer. Similar methods exist for `Y`, `Width`, and `Height`.

### Rectangle Instance

```vb
Private Rectangle As New RectangleDouble(0, 0, 256, 256)
```
- This line creates a new rectangle starting at position (0,0) with a width and height of 256 pixels.

### DeltaTimeStructure

```vb
Private Structure DeltaTimeStructure
    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan
```
- This structure tracks the timing information for our animation, including the current and last frame times and the elapsed time between them.

#### Constructor

```vb
Public Sub New(currentFrame As Date, lastFrame As Date, elapsedTime As TimeSpan)
    Me.CurrentFrame = currentFrame
    Me.LastFrame = lastFrame
    Me.ElapsedTime = elapsedTime
End Sub
```
- Initializes the timing structure with the current and last frame times.

### Velocity

```vb
Private Velocity As Double = 64.0F
```
- This variable sets the speed of the rectangle's movement to 64 pixels per second.

### DisplayStructure

```vb
Private Structure DisplayStructure
    Public Location As Point
    Public Text As String
    Public Font As Font
```
- This structure holds information about display elements, including their location, text, and font.

### FrameCounterStructure

```vb
Private Structure FrameCounterStructure
    Public FrameCount As Integer
    Public StartTime As DateTime
    Public TimeElapsed As TimeSpan
    Public SecondsElapsed As Double
```
- This structure counts the frames rendered and tracks the timing for calculating frames per second (FPS).

### Initialization Methods

#### Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApp()
End Sub
```
- This method initializes the application when the form loads.

#### Resize Event

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
    If Not WindowState = FormWindowState.Minimized Then
        ResizeFPS()
        ResizeRectangle()
        DisposeBuffer()
    End If
End Sub
```
- This method handles resizing the window, ensuring the FPS display and rectangle size are adjusted accordingly.

### Timer Tick Event

```vb
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    If Not WindowState = FormWindowState.Minimized Then
        UpdateFrame()
        Invalidate() ' Calls OnPaint Sub
    End If
End Sub
```
- This event is triggered at regular intervals to update the animation frame and redraw the form.

### OnPaint Method

```vb
Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
    AllocateBuffer()
    DrawFrame()
    Buffer.Render(e.Graphics)
    UpdateFrameCounter()
    MyBase.OnPaint(e)
End Sub
```
- This method handles the rendering of the graphics. It allocates a buffer, draws the current frame, and updates the frame counter.

### UpdateFrame Method

```vb
Private Sub UpdateFrame()
    UpdateDeltaTime()
    MoveRectangle()
End Sub
```
- This method updates the timing and moves the rectangle based on the elapsed time.

### MoveRectangle Method

```vb
Private Sub MoveRectangle()
    Rectangle.X += Velocity * DeltaTime.ElapsedTime.TotalSeconds
    If Rectangle.X > ClientRectangle.Right Then
        Rectangle.X = ClientRectangle.Left - Rectangle.Width
    End If
End Sub
```
- This method updates the rectangle's position based on its velocity and the elapsed time. If it moves off the right edge, it reappears on the left.






This application demonstrates the principles of animation using a simple rectangle. By understanding how to manage timing and movement, you can create engaging graphics in your applications!

Feel free to experiment with the code and adjust parameters to see how they affect the animation. Happy coding!


---





## License Information

This code is shared under the MIT License, allowing you to use, modify, and distribute it freely, as long as you include the original copyright notice.

```plaintext
MIT License
Copyright(c) 2023 Joseph W. Lumbley
```

