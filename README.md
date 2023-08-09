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

DeltaTime is the time since the last frame was drawn.

```
DeltaTime = CurrentFrame - LastFrame
```

### Velocity

Velocity is the speed we want our rectangle to move, measured in pixels per second.


We can adjust this value to change the speed of our rectangle.

```
Private Velocity As Single = 250.0F
```


### Distance

We calculate the distance that our rectangle should move based on its Velocity * DeltaTime.

Distance = Velocity * DeltaTime

This ensures that our rectangle moves at a consistent speed regardless of the frame rate.

For example, if the frame rate is low, DeltaTime will be larger,

and our rectangle will move a larger distance in that frame to compensate.

| Frames per Second | DeltaTime | Distance |
| --- | --- | --- |
| 428.1 High | 0.0023366 Seconds | 0.58415 Pixels|
| 38.8 Low | 0.0257694 Seconds | 6.44235 Pixels|

### RectPostion.X
```
Private RectPostion As New Vector2(Rect.X, Rect.Y)
```

```
RectPostion.X += Velocity * DeltaTime.TotalSeconds 'Δs = V * Δt
```
  


### Rect.X
```
Rect.X = Math.Round(RectPostion.X)
```


Rect.X = RectPostion.X

70 = 69.7753143

383 = 382.522064

630 = 630.4418

573 = 573.354

8 = 8.075825

276 = 275.7165

