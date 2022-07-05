module Types

open System
open Fable.Core.JsInterop

[<RequireQualifiedAccess>]
type Page =
    | Summary
    | NewCheck of title: string option
    | CheckList of range: string option

type CheckListParams =
    abstract _id: string option

type NewCheckParams =
    abstract title: string option

type CheckListArgs = Navigo.Match<obj, CheckListParams>
type NewCheckArgs = Navigo.Match<obj, NewCheckParams>

type PouchResponse =
    abstract id: string
    abstract ok: bool
    abstract rev: string

type MissionRow = {| name: string; isDone: bool |}

type MissionRecord =
    {| createdAt: DateTime
       title: string
       missions: MissionRow array
       missionValue: int
       _rev: string option
       _id: string |}
