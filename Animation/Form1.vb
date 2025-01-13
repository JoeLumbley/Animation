' Animation
'
' Animation is the art of creating the illusion of motion by displaying a series
' of static images in quick succession. In our app, we use animation to make it
' appear as though our rectangle is moving towards the right.To ensure that our
' animation runs smoothly on all devices, we have designed it to be frame
' independent. This means that our animation is not affected by changes in the
' frame rate, ensuring a consistent and seamless experience for all users.

' MIT License
' Copyright(c) 2023 Joseph W. Lumbley

' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.

' https://github.com/JoeLumbley/Animation

Option Explicit On





Public Class Form1

    Private Context As New BufferedGraphicsContext

    Private Buffer As BufferedGraphics

    Private ReadOnly MinimumMaxBufferSize As New Size(1280, 720)

    Private BackgroundColor As Color = Color.Black

    ' The RectangleDouble structure represents a rectangle with
    ' double-precision coordinates and dimensions.
    Private Structure RectangleDouble

        Public X, Y, Width, Height, Velocity As Double
        Public Brush As Brush

        Public Sub New(x As Double, y As Double, width As Double, height As Double,
                       velocity As Double, brush As Brush)

            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height
            Me.Velocity = velocity
            Me.Brush = brush
        End Sub

        ' Methods to round attributes to
        ' the nearest integer values.
        Public Function GetNearestX() As Integer

            Return RoundToNearest(X)
        End Function
        Public Function GetNearestY() As Integer

            Return RoundToNearest(Y)
        End Function
        Public Function GetNearestWidth() As Integer

            Return RoundToNearest(Width)
        End Function
        Public Function GetNearestHeight() As Integer

            Return RoundToNearest(Height)
        End Function

        ' Generic method to round attributes to the nearest integer values.
        Private Function RoundToNearest(ByVal value As Double) As Integer

            Return CInt(Math.Round(value))
        End Function

        Public Sub MoveRight(ByVal deltaTime As TimeSpan)

            ' Move the rectangle to the right.
            X += Velocity * deltaTime.TotalSeconds
            ' Displacement = Velocity x Delta Time ( Δs = V * Δt )

        End Sub

        Public Sub Wraparound(ByVal clientRectangle As Rectangle)

            ' When the rectangle exits the right side of the client area.
            If X > clientRectangle.Right Then

                ' The rectangle reappears on the left side of the client area.
                X = clientRectangle.Left - Width

            End If

        End Sub

        Public Sub MoveRightAndWraparound(ByVal clientRectangle As Rectangle,
                                          ByVal deltaTime As TimeSpan)

            MoveRight(deltaTime)

            Wraparound(clientRectangle)

        End Sub

        Public Sub CenterVertically(ByVal clientRectangle As Rectangle)

            ' Center our rectangle vertically in the client area of our form.
            Y = clientRectangle.Height \ 2 - Height \ 2

        End Sub

    End Structure

    Private Rectangle As New RectangleDouble(0.0F, 0.0F, 256.0F, 256.0F, 32.0F,
                                             New SolidBrush(Color.Orchid))

    ' The DeltaTimeStructure represents the time difference
    ' between two frames.
    Private Structure DeltaTimeStructure

        Public CurrentFrame As DateTime
        Public LastFrame As DateTime
        Public ElapsedTime As TimeSpan

        Public Sub New(currentFrame As Date, lastFrame As Date,
                       elapsedTime As TimeSpan)

            Me.CurrentFrame = currentFrame
            Me.LastFrame = lastFrame
            Me.ElapsedTime = elapsedTime
        End Sub

        Public Sub Update()

            ' Set the current frame's time to the current system time.
            CurrentFrame = Now

            ' Calculates the elapsed time ( delta time Δt ) between the
            ' current frame and the last frame.
            ElapsedTime = CurrentFrame - LastFrame

            ' Updates the last frame's time to the current frame's time for
            ' use in the next update.
            LastFrame = CurrentFrame

        End Sub

    End Structure

    Private DeltaTime As New DeltaTimeStructure(DateTime.Now, DateTime.Now,
                                                TimeSpan.Zero)

    Private Structure DisplayStructure

        Public Location As Point
        Public Text As String
        Public Font As Font
        Public Brush As Brush

        Public Sub New(location As Point, text As String, font As Font,
                       brush As Brush)

            Me.Location = location
            Me.Text = text
            Me.Font = font
            Me.Brush = brush
        End Sub

        Public Sub MoveToPosition(ByVal clientRectangle As Rectangle)

            ' Place the FPS display at the bottom of the client area.
            Location = New Point(Location.X,
                                 clientRectangle.Bottom - 75)

        End Sub

    End Structure

    Private FPSDisplay As New DisplayStructure(New Point(0, 0),
                                                          "--",
                                      New Font("Segoe UI", 25),
                            New SolidBrush(Color.MediumOrchid))

    Private Structure FrameCounterStructure

        Public FrameCount As Integer
        Public StartTime As DateTime
        Public TimeElapsed As TimeSpan
        Public FPS As String

        Public Sub New(frameCount As Integer, startTime As Date,
                       timeElapsed As TimeSpan)

            Me.FrameCount = frameCount
            Me.StartTime = startTime
            Me.TimeElapsed = timeElapsed
        End Sub

        Public Sub Update()

            TimeElapsed = Now.Subtract(StartTime)

            If TimeElapsed.TotalSeconds < 1 Then

                FrameCount += 1

            Else

                FPS = $"{FrameCount} FPS"

                FrameCount = 0

                StartTime = Now

            End If

        End Sub

    End Structure

    Private FrameCounter As New FrameCounterStructure(0, DateTime.Now,
                                                      TimeSpan.Zero)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeApp()

        Debug.Print($"Running...{DateTime.Now}")

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        If Not WindowState = FormWindowState.Minimized Then

            FPSDisplay.MoveToPosition(ClientRectangle)

            Rectangle.CenterVertically(ClientRectangle)

            DisposeBuffer()

            Timer1.Enabled = True

        Else

            Timer1.Enabled = False

        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        UpdateFrame()

        Invalidate() ' Calls OnPaint Sub

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        AllocateBuffer()

        DrawFrame()

        ' Show buffer on form.
        Buffer?.Render(e.Graphics)

        FrameCounter.Update()

        EraseFrame()

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)

        ' Intentionally left blank. Do not remove.

    End Sub

    Private Sub UpdateFrame()

        DeltaTime.Update()

        Rectangle.MoveRightAndWraparound(ClientRectangle,
                                         DeltaTime.ElapsedTime)

        FPSDisplay.Text = FrameCounter.FPS

    End Sub

    Private Sub InitializeBuffer()

        Context = BufferedGraphicsManager.Current

        If Screen.PrimaryScreen IsNot Nothing Then

            Context.MaximumBuffer = Screen.PrimaryScreen.WorkingArea.Size

        Else

            Context.MaximumBuffer = MinimumMaxBufferSize

            Debug.Print($"Primary screen not detected.")

        End If

        AllocateBuffer()

    End Sub

    Private Sub AllocateBuffer()

        If Buffer Is Nothing Then

            Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)

            Buffer.Graphics.CompositingMode =
                    Drawing2D.CompositingMode.SourceOver

            Buffer.Graphics.TextRenderingHint =
                 Drawing.Text.TextRenderingHint.AntiAlias

            EraseFrame()

        End If

    End Sub

    Private Sub DrawFrame()

        ' Draw rectangle.
        Buffer?.Graphics.FillRectangle(Rectangle.Brush,
                                       Rectangle.GetNearestX,
                                       Rectangle.GetNearestY,
                                       Rectangle.GetNearestWidth,
                                       Rectangle.GetNearestHeight)

        ' Draw frames per second display.
        Buffer?.Graphics.DrawString(FPSDisplay.Text,
                                    FPSDisplay.Font,
                                    FPSDisplay.Brush,
                                    FPSDisplay.Location)

    End Sub

    Private Sub EraseFrame()

        Buffer?.Graphics.Clear(BackgroundColor)

    End Sub

    Private Sub DisposeBuffer()

        If Buffer IsNot Nothing Then

            Buffer.Dispose()

            Buffer = Nothing ' Set to Nothing to avoid using a disposed object

            ' The buffer will be reallocated in OnPaint

        End If

    End Sub

    Private Sub InitializeApp()

        InitializeForm()

        InitializeBuffer()

        Timer1.Interval = 15

        Timer1.Start()

    End Sub

    Private Sub InitializeForm()

        CenterToScreen()

        SetStyle(ControlStyles.UserPaint, True)

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        Text = "Animation - Code with Joe"

        Me.WindowState = FormWindowState.Maximized

    End Sub





End Class

' Monica is our an AI assistant.
' https://monica.im/

' No sacrifice, no victory. - The Witwicky family motto

' or in Latin "absque sui detrimento non datur victoria"

' Victory is not granted without self-sacrifice
' A powerful reminder that every success often comes with its own set of
' challenges and sacrifices. It’s like a call to embrace the journey,
' with all its ups and downs.
