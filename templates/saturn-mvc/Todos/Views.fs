namespace AppName.Todos

open AppName.BaseViews

open Giraffe.ViewEngine

[<RequireQualifiedAccess>]
module View =
    let private page attrs content =
        [ Partials.Navbar()
          article [ yield! attrs; _class "page" ] content ]

    let Index (todos: Todo list) : XmlNode =
        let content =
            page [ _class "page f-row" ] [
                aside [ _class "menu" ] [
                    ul [ _class "menu-list" ] [
                        li [] [
                            a [ _href "/todos/add" ] [
                                str "Add Todo"
                            ]
                        ]
                    ]
                ]
                table [ _class "table is-bordered is-striped is-narrow is-hoverable is-fullwidth" ] [
                    thead [] [
                        th [] [ str "Id" ]
                        th [] [ str "Title" ]
                        th [] [ str "Is Done" ]
                    ]
                    tbody [] [
                        for todo in todos do
                            tr [] [
                                td [] [
                                    a [ _href $"/todos/{todo.id}" ] [
                                        str $"{todo.id}"
                                    ]
                                ]
                                td [] [ str todo.title ]
                                td [] [
                                    str (sprintf "%s" (if todo.isDone then "Yes" else "No"))
                                ]
                            ]
                    ]
                ]
            ]

        Layout.Default(content, "Todos")

    let AddTodo () =
        let content =
            page [ _class "page f-row" ] [
                aside [ _class "menu" ] [
                    ul [ _class "menu-list" ] [
                        li [] [
                            a [ _href "/todos" ] [ str "Go Back" ]
                        ]
                    ]
                ]
                form [ _method "POST"; _action "/todos" ] [
                    section [ _class "field" ] [
                        label [ _for "title"; _class "label" ] [
                            str "Todo:"
                        ]
                        div [ _class "control" ] [
                            input [ _name "title"
                                    _id "title"
                                    _class "input"
                                    _type "text"
                                    _placeholder "Todo Title"
                                    _required ]
                        ]
                    ]
                    section [ _class "field" ] [
                        div [ _class "control" ] [
                            label [ _for "isDone"; _class "checkbox" ] [
                                input [ _name "isDone"
                                        _id "isDone"
                                        _type "checkbox" ]
                                str " Is Done"
                            ]
                        ]
                    ]
                    section [] [
                        button [ _type "submit"
                                 _class "button is-primary" ] [
                            str "Save Todo"
                        ]
                    ]
                ]
            ]

        Layout.Default(content, "New Todo")

    let TodoDetail (todo: Todo) =
        let content =
            page [] [
                p [] [
                    h1 [] [ str todo.title ]
                    str (sprintf "Is Done: %s" (if todo.isDone then "Yes" else "No"))
                ]
                hr []
                nav [ _class "d-flex f-row" ] [
                    a [ _href $"/todos/{todo.id}/edit"
                        _class "button is-link" ] [
                        str "Edit Todo"
                    ]
                    a [ _href $"/todos"
                        _class "button is-link" ] [
                        str "Todo List"
                    ]
                    a [ attr "hx-delete" $"/todos/{todo.id}"
                        _class "button is-danger" ] [
                        str "Delete Todo"
                    ]
                ]
            ]

        Layout.Default(content, todo.title, [ script [ _src "https://unpkg.com/htmx.org@1.5.0" ] [] ])

    let EditTodo (todo: Todo) =
        let content =
            page [] [
                form [ _method "POST"
                       _action $"/todos/{todo.id}" ] [
                    section [ _class "field" ] [
                        label [ _for "title"; _class "label" ] [
                            str "Todo:"
                        ]
                        div [ _class "control" ] [
                            input [ _name "title"
                                    _id "title"
                                    _class "input"
                                    _type "text"
                                    _placeholder "Todo Title"
                                    _value todo.title
                                    _required ]
                        ]
                    ]
                    section [ _class "field" ] [
                        div [ _class "control" ] [
                            label [ _for "isDone"; _class "checkbox" ] [
                                input [ _name "isDone"
                                        _id "isDone"
                                        _type "checkbox"
                                        if todo.isDone then _checked ]
                                str " Is Done"
                            ]
                        ]
                    ]
                    section [] [
                        button [ _type "submit"
                                 _class "button is-primary" ] [
                            str "Save Todo"
                        ]
                    ]
                ]
            ]

        Layout.Default(content, "New Todo")
