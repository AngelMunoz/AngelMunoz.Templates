open Falco
open Falco.Routing
open Falco.HostBuilder


let handler context = 
    let route = Request.getRoute context

    let name = 
        match route.TryGetStringNonEmpty "name" with
        | Some name -> name
        | None -> "F#"
    
    Response.ofPlainText $"Hello, {name}!" context

[<EntryPoint>]
let main argv = 
    webhost argv {
        endpoints [ get "/{name?}" handler ]
    }