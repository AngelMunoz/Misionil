module RouteHandlers

open System
open Types

let onChecks (args: CheckListArgs) : Page =
    let getDateRange (args: CheckListArgs) =
        let _id =
            args.``params``
            |> Option.map (fun arg -> arg._id)
            |> Option.flatten

        _id

    args |> getDateRange |> Page.CheckList


let onNewCheck (args: NewCheckArgs) : Page =
    let getTitle (data: NewCheckArgs) =
        data.``params``
        |> Option.map (fun data -> data.title)
        |> Option.flatten

    args |> getTitle |> Page.NewCheck
