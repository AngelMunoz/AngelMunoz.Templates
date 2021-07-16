module AppName.Program

open Saturn
open Saturn.Endpoint

let api =
    pipeline {
        plug acceptJson
        set_header "x-pipeline-type" "API"
    }

let defaultRouter =
    router {
        pipe_through api
        get "/api/todos" Todos.Find
        getf "/api/todos/%i" Todos.FindOne
        post "/api/todos" Todos.Create
        putf "/api/todos/%i" Todos.Update
        deletef "/api/todos/%i" Todos.Delete
    }

[<EntryPoint>]
let main args =
    let app =
        application {
            use_developer_exceptions
            use_endpoint_router defaultRouter
        }

    run app
    0
