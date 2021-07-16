# MVC Group 
For a particular kind of model you might want to put everything under one folder/namespace like in this case.
This can be useful for *Isolated Models* (that are not related to other models) but it might not make sense if your models are starting to get along with other models
an alternate option would be to go for 

```

Models/
    Todos.fs
    Products.fs
Views/
    Todos.fs
    Products.fs
Controllers/
    Todos.fs
    Products.fs
```

Of course feel free to re-arange at will

