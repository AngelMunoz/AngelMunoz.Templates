namespace AppName.Todos

open Giraffe

open Saturn
open Saturn.Endpoint
open Microsoft.AspNetCore.Http
open AppName.BaseViews


module Controller =
    open AppName.Todos

    let private todos =
        fun ctx ->
            task {
                let! todos = Model.Find()
                let view = View.Index todos
                return! Controller.renderHtml ctx view
            }

    let private addTodo =
        fun ctx ->
            task {
                let view = View.AddTodo()
                return! Controller.renderHtml ctx view
            }

    let private createTodo =
        fun (ctx: HttpContext) ->
            task {
                let title = ctx.GetFormValue("title")
                let isDone = ctx.GetFormValue("isDone")

                let partial =
                    { title = title |> Option.defaultValue ""
                      isDone = isDone |> Option.defaultValue "off" }

                let! todo = Model.Create partial
                let view = View.TodoDetail todo
                return! Controller.renderHtml ctx view
            }

    let private showTodo =
        fun ctx id ->
            task {
                let! todo = Model.FindOne id

                match todo with
                | Some todo ->
                    let view = View.TodoDetail todo
                    return! Controller.renderHtml ctx view
                | None ->
                    ctx.SetStatusCode(404)
                    return! Controller.renderHtml ctx (Layout.NotFound())
            }

    let private editTodo =
        fun (ctx: HttpContext) id ->
            task {

                match! Model.FindOne id with
                | Some todo ->
                    let view = View.EditTodo todo
                    return! Controller.renderHtml ctx view
                | None ->
                    ctx.SetStatusCode(404)
                    return! Controller.renderHtml ctx (Layout.NotFound())
            }

    let private updateTodo =
        fun (ctx: HttpContext) id ->
            task {
                match! Model.FindOne id with
                | Some todo ->
                    let title = ctx.GetFormValue("title")
                    let isDone = ctx.GetFormValue("isDone")

                    let! todo =
                        Model.Update
                            { todo with
                                  title = title |> Option.defaultValue ""
                                  isDone = (isDone |> Option.defaultValue "off") = "on" }

                    let view = View.TodoDetail todo
                    return! Controller.renderHtml ctx view
                | None ->
                    ctx.SetStatusCode(404)
                    return! Controller.renderHtml ctx (Layout.NotFound())
            }

    let private deleteTodo =
        fun (ctx: HttpContext) id ->
            task {
                let! _ = Model.Delete id
                ctx.SetHttpHeader("HX-Redirect", "/todos")
                ctx.SetStatusCode(204)
                return! Controller.text ctx ""
            }

    let TodoController =
        controller {
            index todos
            add addTodo
            create createTodo
            show showTodo
            edit editTodo
            update updateTodo
            delete deleteTodo
        }
