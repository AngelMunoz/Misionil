[<RequireQualifiedAccess>]
module App

open Lit
open Browser.Dom
open Types
open Router
open FSharp.Control

[<LitElement "mi-app">]
let private app () =
    LitElement.init (fun cfg -> cfg.useShadowDom <- false)
    |> ignore

    let page, setPage = Hook.useState Page.Root

    Hook.useEffectOnce (fun _ -> RouterPage |> Observable.add setPage)

    let content =
        match page with
        | Page.Root -> html $"<h1>Home!</h1>"
        | Page.Checks (sd, ed) -> html $"<h1>Checklist DateRange {sd}, {ed}</h1>"
        | Page.NewCheck id -> html $"<h1>Title: {id}</h1>"

    html
        $"""
      <sl-button @click={fun _ -> Router.navigate ("/")}>Root</sl-button>
      <sl-button @click={fun _ -> Router.navigate ("/checklist")}>Checklist</sl-button>
      <sl-button @click={fun _ -> Router.navigate ("/new-check?title=peter")}>Checklist</sl-button>
      {content}
    """

let register () = ()
