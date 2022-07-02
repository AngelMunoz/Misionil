module Router

open System

open Navigo
open Types

exception NotFoundExeption of Match


let Router = Navigo("/", {| hash = true |})

let RouterPage =
    { new IObservable<Page> with
        override _.Subscribe(observer) =
            Router
                .on((fun _ -> observer.OnNext Page.Root))
                .on("/checklist", (RouteHandlers.onChecks >> observer.OnNext))
                .on("/new-check", (RouteHandlers.onNewCheck >> observer.OnNext))
                .notFound(fun (data: Match) -> observer.OnError(NotFoundExeption data))
                .resolve ()
            |> ignore

            { new IDisposable with
                member _.Dispose() = () } }
