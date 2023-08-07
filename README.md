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


```
        RectPostion.X += Velocity * DeltaTime.TotalSeconds 'Δs = V * Δt
```

This ensures that our rectangle moves at a consistent speed regardless of the frame rate.


For example, if the frame rate is low, DeltaTime will be larger, and our rectangle will move a larger distance in that frame to compensate for the lower frame rate.

Distance   =   Velocity * DeltaTime

  6.44235  =    250   * 0.0257694
  
323.161163 =    250   * 1.2926446






