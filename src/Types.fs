module Types

open System

[<RequireQualifiedAccess>]
type Page =
    | Root
    | NewCheck of title: string option
    | Checks of range: (DateTime * DateTime)

type CheckListParams =
    abstract startDate: DateTime option
    abstract endDate: DateTime option

type NewCheckParams =
    abstract title: string option

type CheckListArgs = Navigo.Match<obj, CheckListParams>
type NewCheckArgs = Navigo.Match<obj, NewCheckParams>
