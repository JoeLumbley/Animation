# Animation

Animation is the art of creating the illusion of motion by displaying a series of static images in quick succession.

In our app, we use animation to make it appear as though our rectangle is moving towards the right.


## Frame Independent


To ensure that our animation runs smoothly on all devices, we have designed it to be frame independent.

This means that our animation is not affected by changes in the frame rate,
ensuring a consistent and seamless experience for all users.

![001](https://github.com/JoeLumbley/Animation/assets/77564255/63b4a8fb-5097-4887-8d16-1b09fb8a209a)



## Time-Based Motion


```

    Private Velocity As Single = 250.0F

```


The Velocity variable represents the speed at which we want our rectangle to move, measured in pixels per second. We can adjust this value to change the speed of our rectangle.



