namespace AppName.Todos


type Todo =
    { id: int
      title: string
      isDone: bool }

[<CLIMutable>]
type PartialTodo = { title: string; isDone: string }

[<RequireQualifiedAccess>]
module Model =
    open System.Threading.Tasks
    open System.Linq

    let private _todos = lazy (ResizeArray())

    (* Fake async services *)

    let Find () =
        Task.FromResult(_todos.Value |> List.ofSeq)

    let FindOne (id: int) =
        Task.FromResult(
            _todos.Value
            |> Seq.tryFind (fun item -> item.id = id)
        )

    let Create (todo: PartialTodo) =
        let todo =
            { id = _todos.Value.Count + 1
              title = todo.title
              isDone = todo.isDone = "on" }

        _todos.Value.Add(todo)
        Task.FromResult(todo)


    let Update (todo: Todo) =
        let todos =
            _todos
                .Value
                .Select(fun item -> if item.id = todo.id then todo else item)
                .ToArray()

        _todos.Value.Clear()
        _todos.Value.AddRange(todos)

        Task.FromResult(todo)

    let Delete (id: int) =
        let todo =
            _todos.Value.Find(fun item -> item.id = id)

        Task.FromResult(_todos.Value.Remove(todo))
