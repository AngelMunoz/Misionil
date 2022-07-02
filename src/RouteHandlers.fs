module RouteHandlers

open System
open Types

let onChecks (args: CheckListArgs) : Page =
    let getDateRange (args: CheckListArgs) =
        let startDate =
            args.``params``
            |> Option.map (fun arg -> arg.startDate)
            |> Option.flatten
            |> Option.defaultValue DateTime.Now

        let endDate =
            args.``params``
            |> Option.map (fun arg -> arg.endDate)
            |> Option.flatten
            |> Option.defaultValue DateTime.Now

        startDate, endDate

    args |> getDateRange |> Page.Checks


let onNewCheck (args: NewCheckArgs) : Page =
    let getTitle (data: NewCheckArgs) =
        data.``params``
        |> Option.map (fun data -> data.title)
        |> Option.flatten

    args |> getTitle |> Page.NewCheck
