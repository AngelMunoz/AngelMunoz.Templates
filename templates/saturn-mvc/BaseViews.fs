namespace AppName.BaseViews

open Giraffe.ViewEngine

[<RequireQualifiedAccess>]
module private Html =
    let augmentNodes (nodes: XmlNode list option) (defaults: XmlNode list) =
        nodes
        |> Option.map (fun extras -> List.concat [ defaults; extras ])
        |> Option.orElse (Some defaults)

    let MainLayout
        (content: XmlNode list)
        (_title: string option)
        (scripts: XmlNode list option)
        (styles: XmlNode list option)
        =

        let _title = defaultArg _title "Welcome"
        let styles = defaultArg styles []
        let scripts = defaultArg scripts []

        html [ _lang "en-US" ] [
            head [] [
                meta [ _charset "utf-8" ]
                meta [ _name "viewport"
                       _content "width=device-width, initial-scale=1" ]
                title [] [ str $"{_title} | Saturn" ]
                yield! styles
            ]
            body [] [
                yield! content
                yield! scripts
            ]
        ]

    let Navbar (attrs: XmlAttribute list option) (left: XmlNode list option) (right: XmlNode list option) =
        let attrs = defaultArg attrs []
        let left = defaultArg left []
        let right = defaultArg right []

        nav [ yield! attrs; _class "navbar" ] [
            section [ _class "navbar-start" ] [
                yield! left
            ]
            section [ _class "navbar-end" ] [
                yield! right
            ]
        ]

type Partials() =
    static member Navbar(?attrs: XmlAttribute list, ?leftLinks: XmlNode list, ?rightLinks: XmlNode list) =
        let leftLinks =
            Html.augmentNodes
                leftLinks
                [ a [ _href "/"; _class "navbar-item" ] [
                      str "Home"
                  ] ]

        Html.Navbar attrs leftLinks rightLinks



(* Classes with static members are really cool to use optional parameters
   we can leverage them to have flexibility for our consuming code *)
type Layout() =

    static member DefaultStyles() : XmlNode list =
        [ link [ _rel "stylesheet"
                 _href "https://cdn.jsdelivr.net/npm/bulma@0.9.3/css/bulma.min.css" ]
          link [ _rel "stylesheet"
                 _href "/css/styles.css" ] ]

    static member DefaultScripts() : XmlNode list =
        [ script [ _type "module"; _src "/js/index.js" ] [] ]

    static member Default(content: XmlNode list, ?title: string, ?scripts: XmlNode list, ?styles: XmlNode list) =


        let scripts =
            Html.augmentNodes scripts (Layout.DefaultScripts())

        let styles =
            Html.augmentNodes styles (Layout.DefaultStyles())

        Html.MainLayout content title scripts styles

    static member NotFound(?content: XmlNode list) =
        Layout.Default(
            content
            |> Option.defaultValue [ Partials.Navbar()
                                     str "Not Found" ]
        )
