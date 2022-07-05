[<RequireQualifiedAccess>]
module App

open Lit
open Browser.Dom
open Types
open Router
open FSharp.Control
open Components

[<LitElement "mi-app">]
let private app () =
    LitElement.init (fun cfg -> cfg.useShadowDom <- false)
    |> ignore

    let page, setPage = Hook.useState Page.Summary

    Hook.useEffectOnce (fun _ -> RouterPage |> Observable.add setPage)

    let content =
        match page with
        | Page.Summary -> html $"<mi-home>Home!</mi-home>"
        | Page.CheckList id -> CheckList.View id
        | Page.NewCheck title -> NewCheck.View title

    html
        $"""
        <nav class="print-hidden">
            <sl-button @click={fun _ -> Router.navigate ("/")}>Resumen</sl-button>
            <sl-button @click={fun _ -> Router.navigate ("/checklist")}>Misiones</sl-button>
            <sl-button @click={fun _ -> Router.navigate ("/new-check?title=peter")}>Nuevas Misiones</sl-button>
        </nav>{content}
    """

let register () = ()
