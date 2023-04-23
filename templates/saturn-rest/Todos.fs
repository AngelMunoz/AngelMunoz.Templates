[<RequireQualifiedAccess>]
module AppName.Todos

open Microsoft.AspNetCore.Http

open Giraffe
open Saturn

type Todo = { id: int; text: string; isDone: bool }

let private _todos = lazy (ResizeArray<Todo>())


let Find _ (ctx: HttpContext) =
    ctx.WriteJsonAsync(_todos.Value.ToArray())

let FindOne (id: int) _ (ctx: HttpContext) =
    ctx.WriteJsonAsync(_todos.Value.Find(fun item -> item.id = id))

let Create _ (ctx: HttpContext) =
    task {
        let! todo = ctx.BindJsonAsync<{| title: string; isDone: bool |}>()

        let todo =
            { id = _todos.Value.Count + 1
              text = todo.title
              isDone = todo.isDone }

        _todos.Value.Add(todo)
        return! ctx.WriteJsonAsync(todo)
    }

let Update (id: int) _ (ctx: HttpContext) =
    task {
        let todo =
            _todos.Value
            |> Seq.tryFind (fun item -> item.id = id)

        match todo with
        | Some _ ->
            let! todo = ctx.BindJsonAsync<Todo>()

            let index =
                _todos.Value.FindIndex(fun index -> index.id = id)

            _todos.Value.[index] <- todo
            return! ctx.WriteJsonAsync(todo)
        | None ->
            ctx.SetStatusCode(404)
            return! ctx.WriteJsonAsync({| message = "Not Found" |})
    }

let Delete (id: int) _ (ctx: HttpContext) =
    task {
        let todo =
            _todos.Value
            |> Seq.tryFind (fun item -> item.id = id)

        match todo with
        | Some todo ->
            _todos.Value.Remove(todo) |> ignore
            ctx.SetStatusCode(204)
            return! ctx.WriteTextAsync ""
        | None ->
            ctx.SetStatusCode(404)
            return! ctx.WriteJsonAsync({| message = "Not Found" |})
    }
