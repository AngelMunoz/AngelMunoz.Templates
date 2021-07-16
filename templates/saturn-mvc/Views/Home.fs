namespace AppName.Views

open Giraffe.ViewEngine
open AppName.BaseViews

[<RequireQualifiedAccess>]
module Home =

    let Index () =
        let content =
            [ Partials.Navbar(
                leftLinks =
                    [ a [ _href "/todos"; _class "navbar-item" ] [
                          str "Check the Todos!"
                      ] ]
              )
              article [ _class "page" ] [
                  header [] [
                      h1 [] [ str "Welcome to Saturn!" ]
                  ]
                  p [] [
                      str
                          """
                          Saturn is an F# web framework for asp.net
                          """
                  ]
              ] ]

        Layout.Default(content, "Home")
