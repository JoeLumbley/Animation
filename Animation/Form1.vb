'Animation
'
'Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession.
'In our app, we use animation to make it appear as though our rectangle is moving towards the right.
'To ensure that our animation runs smoothly on all devices, we have designed it to be frame independent.
'This means that our animation is not affected by changes in the frame rate,
'ensuring a consistent and seamless experience for all users.

'MIT License
'Copyright(c) 2023 Joseph W. Lumbley

'Permission Is hereby granted, free Of charge, to any person obtaining a copy
'of this software And associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
'copies of the Software, And to permit persons to whom the Software Is
'furnished to do so, subject to the following conditions:

'The above copyright notice And this permission notice shall be included In all
'copies Or substantial portions of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
'IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
'LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
'OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
'SOFTWARE.

Public Class Form1

    ' The RectangleDouble structure represents a rectangle with double-precision coordinates and dimensions.
    ' It provides methods to round its attributes to the nearest integer values.
    Public Structure RectangleDouble
        ' The X-coordinate of the rectangle.
        Public X As Double

        ' The Y-coordinate of the rectangle.
        Public Y As Double

        ' The width of the rectangle.
        Public Width As Double

        ' The height of the rectangle.
        Public Height As Double

        ' Constructor to initialize the RectangleDouble structure with specific values for X, Y, Width, and Height.
        Public Sub New(x As Double, y As Double, width As Double, height As Double)
            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height
        End Sub

        ' Function to get the nearest integer value of X.
        Public Function GetNearestX() As Integer
            Return Math.Round(X)
        End Function

        ' Function to get the nearest integer value of Y.
        Public Function GetNearestY() As Integer
            Return Math.Round(Y)
        End Function

        ' Function to get the nearest integer value of Width.
        Public Function GetNearestWidth() As Integer
            Return Math.Round(Width)
        End Function

        ' Function to get the nearest integer value of Height.
        Public Function GetNearestHeight() As Integer
            Return Math.Round(Height)
        End Function
    End Structure

    Private Rect As New RectangleDouble(0, 0, 256, 256)

    Private Context As New BufferedGraphicsContext

    Private Buffer As BufferedGraphics

    Private Structure DeltaTimeStructure
        Public CurrentFrame As DateTime
        Public LastFrame As DateTime
        Public Value As TimeSpan

        Public Sub New(currentFrame As Date, lastFrame As Date, value As TimeSpan)
            Me.CurrentFrame = currentFrame
            Me.LastFrame = lastFrame
            Me.Value = value
        End Sub
    End Structure

    Private DeltaTime As New DeltaTimeStructure(Now, Now, Now - Now)


    'Private CurrentFrame As DateTime = Now 'Get current time.

    'Private LastFrame As DateTime = DeltaTime.CurrentFrame 'Initialize last frame time to current time.

    'Private DeltaTime As TimeSpan = CurrentFrame - LastFrame 'Initialize delta time to 0







    Private Velocity As Double = 64.0F

    ' The DisplayStructure structure represents a display element with a location, text, font, and value.
    ' It provides a constructor to initialize these properties.
    Private Structure DisplayStructure
        ' The location of the display element.
        Public Location As Point

        ' The text to be displayed.
        Public Text As String

        ' The font used for the display text.
        Public Font As Font

        ' The value associated with the display element.
        Public Value As Integer

        ' Constructor to initialize the DisplayStructure with specific values for location, text, font, and value.
        Public Sub New(location As Point, text As String, font As Font, value As Double)
            Me.Location = location
            Me.Text = text
            Me.Font = font
            Me.Value = value
        End Sub
    End Structure

    Private FPSDisplay As New DisplayStructure(New Point(0, 0), "", New Font("Segoe UI", 25), 0.0F)

    Private Structure FrameCounterStructure
        Public FrameCount As Integer
        Public StartTime As DateTime
        Public TimeElapsed As TimeSpan
        Public SecondsElapsed As Double

        Public Sub New(frameCount As Integer, startTime As Date, timeElapsed As TimeSpan, secondsElapsed As Double)
            Me.FrameCount = frameCount
            Me.StartTime = startTime
            Me.TimeElapsed = timeElapsed
            Me.SecondsElapsed = secondsElapsed
        End Sub
    End Structure

    Private FrameCounter As New FrameCounterStructure(0, Now, TimeSpan.Zero, 0)

    Private ReadOnly AlineCenter As New StringFormat With {.Alignment = StringAlignment.Center}

    Private ReadOnly AlineCenterMiddle As New StringFormat With {.Alignment = StringAlignment.Center,
                                                                 .LineAlignment = StringAlignment.Center}

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeApp()

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        If Not WindowState = FormWindowState.Minimized Then

            ResizeFPS()

            ResizeRectangle()

            DisposeBuffer()

        End If

    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Not WindowState = FormWindowState.Minimized Then

            UpdateFrame()

            Invalidate() ' Calls OnPaint Sub

        End If

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        AllocateBuffer()

        DrawFrame()

        ' Show buffer on form.
        Buffer.Render(e.Graphics)

        UpdateFrameCounter()

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)

        ' Intentionally left blank. Do not remove.

    End Sub

    Private Sub UpdateFrame()

        UpdateDeltaTime()

        MoveRectangle()

    End Sub

    Private Sub InitializeBuffer()

        ' Set context to the context of this app.
        Context = BufferedGraphicsManager.Current

        ' Set buffer size to the primary working area.
        Context.MaximumBuffer = Screen.PrimaryScreen.WorkingArea.Size

        AllocateBuffer()

    End Sub

    Private Sub AllocateBuffer()

        If Buffer Is Nothing Then

            Buffer = Context.Allocate(CreateGraphics(), ClientRectangle)

            With Buffer.Graphics

                .CompositingMode = Drawing2D.CompositingMode.SourceOver
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            End With

        End If

    End Sub

    Private Sub DrawFrame()

        With Buffer.Graphics

            .Clear(Color.Black)

            .FillRectangle(Brushes.Purple, Rect.GetNearestX, Rect.GetNearestY, Rect.GetNearestWidth, Rect.GetNearestHeight)

            ' Draw frames per second display.
            .DrawString(FPSDisplay.Value.ToString & " FPS", FPSDisplay.Font, Brushes.MediumOrchid, FPSDisplay.Location)

        End With

    End Sub

    Private Sub DisposeBuffer()

        If Buffer IsNot Nothing Then

            Buffer.Dispose()

            Buffer = Nothing ' Set to Nothing to avoid using a disposed object

            ' The buffer will be reallocated in OnPaint

        End If

    End Sub

    Private Sub UpdateDeltaTime()
        ' Delta time (Δt) is the elapsed time since the last frame.

        DeltaTime.CurrentFrame = Now

        DeltaTime.Value = DeltaTime.CurrentFrame - DeltaTime.LastFrame ' Calculate delta time

        DeltaTime.LastFrame = DeltaTime.CurrentFrame ' Update last frame time

    End Sub

    Private Sub MoveRectangle()

        ' Move the rectangle to the right.
        Rect.X += Velocity * DeltaTime.Value.TotalSeconds 'Δs = V * Δt
        ' Displacement = Velocity x Delta Time

        ' Wraparound
        ' When the rectangle exits the right side of the client area.
        If Rect.X > ClientRectangle.Right Then

            ' The rectangle reappears on the left side the client area.
            Rect.X = ClientRectangle.Left - Rect.Width

        End If

    End Sub

    Private Sub InitializeApp()

        InitializeForm()

        InitializeBuffer()

        Timer1.Interval = 10

        Timer1.Start()

    End Sub

    Private Sub InitializeForm()

        CenterToScreen()

        SetStyle(ControlStyles.UserPaint, True)

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        Text = "Animation - Code with Joe"

        Me.WindowState = FormWindowState.Maximized

    End Sub

    Private Sub UpdateFrameCounter()

        FrameCounter.TimeElapsed = Now.Subtract(FrameCounter.StartTime)

        FrameCounter.SecondsElapsed = FrameCounter.TimeElapsed.TotalSeconds

        If FrameCounter.SecondsElapsed < 1 Then

            FrameCounter.FrameCount += 1

        Else

            FPSDisplay.Value = FrameCounter.FrameCount

            FrameCounter.FrameCount = 0

            FrameCounter.StartTime = Now

        End If

    End Sub

    Private Sub ResizeRectangle()

        ' Center our rectangle vertically in the client area of our form.
        Rect.Y = ClientRectangle.Height \ 2 - Rect.Height \ 2

    End Sub

    Private Sub ResizeFPS()

        ' Place the FPS display at the bottom of the client area.
        FPSDisplay.Location.Y = ClientRectangle.Bottom - 75

    End Sub

End Class

' Monica is our an AI assistant.
' https://monica.im/
