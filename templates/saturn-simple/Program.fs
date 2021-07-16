module AppName.Program

open Giraffe
open Saturn

[<EntryPoint>]
let main args =
    let app =
        application { use_router (text "Hello World from Saturn") }

    run app
    0
