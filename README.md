# Animation
This project showcases the fundamentals of creating smooth animations. The application features a moving rectangle that glides across the screen, illustrating key concepts such as frame independence and real-time rendering.



![010](https://github.com/user-attachments/assets/d871475e-c09c-42d6-bb59-693aca7bb7b4)




In this app, you'll learn how to manage timing, handle graphics rendering, and implement basic animation principles. Whether you're a beginner looking to understand the basics of animation or an experienced developer seeking a refresher, this project provides a hands-on approach to mastering animation techniques.

---



# Project Walkthrough

Welcome to the Animation project! In this lesson, we'll break down the code line by line, helping you understand how it works. This project demonstrates the fundamentals of creating smooth animations using a Windows Form application. Let's dive right in!

Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession. In our app, we use animation to make it appear as though our rectangle is moving towards the right. To ensure that our animation runs smoothly on all devices, we have designed it to be frame-independent. This means that our animation is not affected by changes in the frame rate, ensuring a consistent and seamless experience for all users.



## Code Walkthrough

### Setting Up the Environment

```vb
Option Explicit On
```
This line ensures that all variables must be declared before they are used. This helps prevent errors caused by typos in variable names.

### The Form Class

```vb
Public Class Form1
```
This line defines a new class named `Form1`, which represents our main application window. All the code inside this class will define the behavior and appearance of our form.

### Buffered Graphics

```vb
Private Context As New BufferedGraphicsContext
Private Buffer As BufferedGraphics
Private ReadOnly MinimumMaxBufferSize As New Size(1280, 720)
Private BackgroundColor As Color = Color.Black
Private ReadOnly FpsIdentifier As New String(" FPS")
```
- `Context`: This variable manages the buffered graphics.
- `Buffer`: This holds our drawing operations before rendering them on the screen.
- `MinimumMaxBufferSize`: Sets the minimum size for our graphics buffer.
- `BackgroundColor`: Defines the color of the background (black).
- `FpsIdentifier`: A string that will be appended to our FPS count for display purposes.

### Rectangle Structure

```vb
Private Structure RectangleDouble
    Public X, Y, Width, Height, Velocity As Double
    Public Brush As Brush
```
This defines a structure named `RectangleDouble` that represents a rectangle with double-precision coordinates and dimensions. The properties `X`, `Y`, `Width`, `Height`, and `Velocity` define the rectangle's position and size, while `Brush` determines the color used to fill the rectangle.

#### Constructor

```vb
Public Sub New(x As Double, y As Double, width As Double, height As Double, velocity As Double, brush As Brush)
    Me.X = x
    Me.Y = y
    Me.Width = width
    Me.Height = height
    Me.Velocity = velocity
    Me.Brush = brush
End Sub
```
This constructor initializes a new rectangle with specified values for its position, size, velocity, and color.

#### Methods for Movement

```vb
Public Sub MoveRight(ByVal deltaTime As TimeSpan)
    X += Velocity * deltaTime.TotalSeconds
End Sub
```
This method moves the rectangle to the right based on its velocity and the elapsed time since the last frame. The formula used is **Displacement = Velocity x Delta Time**.

```vb
Public Sub Wraparound(ByVal clientRectangle As Rectangle)
    If X > clientRectangle.Right Then
        X = clientRectangle.Left - Width
    End If
End Sub
```
This method checks if the rectangle has exited the right side of the client area. If it has, it reappears on the left side.

```vb
Public Sub MoveRightAndWraparound(ByVal clientRectangle As Rectangle, ByVal deltaTime As TimeSpan)
    MoveRight(deltaTime)
    Wraparound(clientRectangle)
End Sub
```
This method combines the movement and wraparound logic, making it easy to call both in one function.

```vb
Public Sub CenterVertically(ByVal clientRectangle As Rectangle)
    Y = clientRectangle.Height \ 2 - Height \ 2
End Sub
```
This method centers the rectangle vertically in the client area of our form.

### Delta Time Structure

```vb
Private Structure DeltaTimeStructure
    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan
```
This structure tracks the timing information for our animation, including the current and last frame times and the elapsed time between them.

#### Constructor and Update Method

```vb
Public Sub New(currentFrame As Date, lastFrame As Date, elapsedTime As TimeSpan)
    Me.CurrentFrame = currentFrame
    Me.LastFrame = lastFrame
    Me.ElapsedTime = elapsedTime
End Sub
```
This constructor initializes the `DeltaTimeStructure` with the current and last frame times.

```vb
Public Sub Update()
    CurrentFrame = Now
    ElapsedTime = CurrentFrame - LastFrame
    LastFrame = CurrentFrame
End Sub
```
The `Update` method sets the current frame's time to the current system time, calculates the elapsed time since the last frame, and updates the last frame's time for the next update.

### Display Structure

```vb
Private Structure DisplayStructure
    Public Location As Point
    Public Text As String
    Public Font As Font
    Public Brush As Brush
```
This structure holds information about display elements, including their location, text, and font.

### Frame Counter Structure

```vb
Private Structure FrameCounterStructure
    Public FrameCount As Integer
    Public StartTime As DateTime
    Public TimeElapsed As TimeSpan
    Public SecondsElapsed As Double
```
This structure counts the frames rendered and tracks the timing for calculating frames per second (FPS).

### Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApp()
    Debug.Print($"Running...{DateTime.Now}")
End Sub
```
This method initializes the application when the form loads, calling `InitializeApp()` to set up the necessary components and printing the current time to the debug console.

### Form Resize Event

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
    If Not WindowState = FormWindowState.Minimized Then
        ResizeFPS()
        Rectangle.CenterVertically(ClientRectangle)
        DisposeBuffer()
    End If
End Sub
```
This method handles resizing the window. It adjusts the FPS display and rectangle size accordingly, ensuring everything looks good when the window is resized.

### Timer Tick Event

```vb
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    If Not WindowState = FormWindowState.Minimized Then
        UpdateFrame()
        Invalidate() ' Calls OnPaint Sub
    End If
End Sub
```
This event is triggered at regular intervals (every 15 milliseconds). It updates the animation frame and redraws the form.

### OnPaint Method

```vb
Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
    AllocateBuffer()
    DrawFrame()
    Buffer?.Render(e.Graphics)
    UpdateFrameCounter()
    MyBase.OnPaint(e)
End Sub
```
This method handles the rendering of graphics. It allocates a buffer, draws the current frame, and updates the frame counter.

### Drawing the Frame

```vb
Private Sub DrawFrame()
    Buffer?.Graphics.Clear(BackgroundColor)
    Buffer?.Graphics.FillRectangle(Rectangle.Brush, Rectangle.GetNearestX, Rectangle.GetNearestY, Rectangle.GetNearestWidth, Rectangle.GetNearestHeight)
    Buffer?.Graphics.DrawString(FPSDisplay.Text, FPSDisplay.Font, FPSDisplay.Brush, FPSDisplay.Location)
End Sub
```
This method clears the buffer with the background color, fills a rectangle with the specified brush, and draws the FPS display string.

### Updating the Frame Counter

```vb
Private Sub UpdateFrameCounter()
    FrameCounter.TimeElapsed = Now.Subtract(FrameCounter.StartTime)
    FrameCounter.SecondsElapsed = FrameCounter.TimeElapsed.TotalSeconds
    If FrameCounter.SecondsElapsed < 1 Then
        FrameCounter.FrameCount += 1
    Else
        FPSDisplay.Text = $"{FrameCounter.FrameCount}{FpsIdentifier}"
        FrameCounter.FrameCount = 0
        FrameCounter.StartTime = Now
    End If
End Sub
```
This method keeps track of how many frames have been rendered in the last second and updates the FPS display accordingly.

### Conclusion

This walkthrough covers the main components of the animation project. Feel free to experiment with the code, adjust parameters, and see how they affect the animation! Understanding these principles will help you master animation techniques in Visual Basic .NET. Happy coding!




---




# Exercises

Here are some exercises you can try to enhance your understanding of the animation project:


1. **Change Rectangle Size**
   - Modify the dimensions of the rectangle in the `RectangleDouble` structure.
   - **Task**: Experiment with different width and height values. Observe how the animation changes with larger or smaller rectangles.

2. **Change Rectangle Color**
   - Update the `RectangleBrush` color in the initialization section.
   - **Task**: Choose different colors (e.g., red, blue, green) for the rectangle. How does the visual impact change with different colors?

3. **Adjust Rectangle Velocity**
   - Locate the variable that defines the rectangle's velocity.
   - **Task**: Increase or decrease the velocity value. How does this affect the speed at which the rectangle moves across the screen?

4. **Add Multiple Rectangles**
   - Create additional instances of `RectangleDouble` to represent multiple rectangles.
   - **Task**: Animate them independently or in a pattern (e.g., staggered movement). How does this change the overall animation?

5. **Implement User Controls**
   - Add keyboard controls to change the rectangle's size, color, or velocity while the application is running.
   - **Task**: Use keys (e.g., arrow keys for velocity, 'C' for color change) to interactively modify the rectangle's properties. 

6. **Create a Bouncing Effect**
   - Modify the `MoveRectangle` method to make the rectangle bounce off the edges of the window.
   - **Task**: Instead of wrapping around, change the direction when it hits the edge. What changes do you need to make in the code?

7. **Change Background Color**
   - Modify the `BackgroundColor` variable to change the background of the form.
   - **Task**: Experiment with different background colors and observe how they affect the visibility of the rectangle.

These exercises will help you to better understand the concepts of animation and improve your programming skills in Visual Basic .NET. Feel free to experiment and combine these tasks for more complex behaviors!









---







# More on DeltaTime

DeltaTime is a critical concept in game development and animation that refers to the time elapsed between the current frame and the last frame. It ensures smooth and consistent motion, regardless of frame rate variations.

### Importance of DeltaTime
- **Frame Rate Independence**: By using DeltaTime, animations and movements can remain consistent across different hardware configurations and frame rates.
- **Smooth Animations**: It allows for smoother transitions and movements, as the changes are proportional to the time elapsed.
- **Timing Control**: DeltaTime helps in managing timing for various game mechanics, such as physics calculations, animations, and input handling.

### How DeltaTime Works
1. **Capture Time**: Record the current time at the beginning of each frame.
2. **Calculate Elapsed Time**: Subtract the last recorded time from the current time to get the DeltaTime.
3. **Apply DeltaTime**: Use this value to adjust movements, animations, and other time-dependent calculations.

### Example Code
Here’s a simplified example illustrating how to implement DeltaTime in a Windows Forms application:

```vb
Private Structure DeltaTimeStructure
    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan

    Public Sub New(currentFrame As DateTime, lastFrame As DateTime)
        Me.CurrentFrame = currentFrame
        Me.LastFrame = lastFrame
        Me.ElapsedTime = CurrentFrame - LastFrame
    End Sub
End Structure

Private DeltaTime As DeltaTimeStructure

' Timer Tick Event
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    Dim currentTime As DateTime = DateTime.Now
    DeltaTime = New DeltaTimeStructure(currentTime, DeltaTime.CurrentFrame)

    ' Update game objects based on DeltaTime.ElapsedTime
    UpdateGameObjects(DeltaTime.ElapsedTime.TotalSeconds)

    ' Render the frame
    Invalidate()
End Sub

Private Sub UpdateGameObjects(deltaTime As Double)
    ' Example: Move an object based on its speed
    Object.Position += Object.Speed * deltaTime
End Sub
```

### Best Practices
- **Keep it Simple**: Use DeltaTime for essential calculations only. Avoid overcomplicating the logic.
- **Cap Frame Rate**: Implement a frame rate cap to prevent extremely high DeltaTime values, which can lead to erratic behavior.
- **Consistent Units**: Ensure that all movement speeds and calculations are consistent in terms of units (e.g., meters per second).

### Common Pitfalls
- **Ignoring DeltaTime**: Not using DeltaTime can result in inconsistent behavior across different devices.
- **Large Frame Drops**: Sudden drops in frame rate can lead to large DeltaTime values, causing movements to appear jerky or too fast.
- **Complex Calculations**: Overusing DeltaTime in complex calculations can lead to performance issues.

DeltaTime is an essential concept for achieving smooth and consistent animations in game development. By understanding and implementing DeltaTime effectively, developers can create a more enjoyable and fluid user experience.

### Further Reading
- Look into game development frameworks and engines that handle DeltaTime automatically, such as Unity or Unreal Engine.
- Explore physics engines that utilize DeltaTime for accurate simulations.

Feel free to ask if you have any specific questions or need further clarification on any aspect of DeltaTime!

This walkthrough covers the main components of the animation project. Feel free to experiment with the code, adjust parameters, and see how they affect the animation! 







---




# Related Projects

If you're interested in exploring a similar project, check out **Animation C#**, which is a port of this animation project. You can find the C# version in its repository: [Animation C# Repository](https://github.com/JoeLumbley/Animation-CS). 

For more information about the original project, visit the [Animation Repository](https://github.com/JoeLumbley/Animation). 

Happy coding!




![012](https://github.com/user-attachments/assets/d50c1b9e-690e-422e-8be5-620a96b295ab)


---



# License Information

This project is shared under the MIT License, which allows you to use, modify, and distribute it freely, as long as you include the original copyright notice.

```plaintext
MIT License
Copyright(c) 2023 Joseph W. Lumbley
```









