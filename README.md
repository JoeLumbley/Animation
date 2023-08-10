# Animation

Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession.

In our app, we use animation to make it appear as though our rectangle is moving towards the right.


## Frame Independent


To ensure that our animation runs smoothly on all devices, we have designed it to be frame independent.

This means that our animation is not affected by changes in the frame rate,
ensuring a consistent and seamless experience for all users.

![001](https://github.com/JoeLumbley/Animation/assets/77564255/63b4a8fb-5097-4887-8d16-1b09fb8a209a)



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

![Vec2XY_2](https://github.com/JoeLumbley/Animation/assets/77564255/fe99bc1f-6f92-4c4c-a312-993342d145c0)



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



