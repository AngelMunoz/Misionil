module Components.CheckList

open Lit
open System
open Types
open Fable.Core
open Fable.Core.JsInterop


let private getMissions () : JS.Promise<MissionRecord array> = importMember "../database.js"
let private getMission (_id: string) : JS.Promise<MissionRecord> = importMember "../database.js"
let private removeMission (_id: string, rev: string) : JS.Promise<PouchResponse> = importMember "../database.js"

let private downloadMissions (setChecks: MissionRecord array -> unit) () =
    getMissions ()
    |> Promise.map (setChecks)
    |> Promise.start

let private downloadMission (_id: string option, setActiveCheck: MissionRecord option -> unit) () =
    match _id with
    | Some _id ->
        getMission _id
        |> Promise.map (setActiveCheck << Some)
        |> Promise.catch (fun err ->
            JS.console.warn (err)
            setActiveCheck None)
        |> Promise.start
    | None -> setActiveCheck None

let private missionRowTpl (mission: MissionRow) =
    html
        $"""
        <li class="form-section">
            <sl-checkbox ?checked={mission.isDone}>{mission.name}</sl-checkbox>
        </li>
        """

let private missionDetail (activeCheck: MissionRecord option) =
    match activeCheck with
    | None -> Lit.nothing
    | Some activeCheck ->

        html
            $"""
            <article class="mission-detail">
                <header>
                    <h1>{activeCheck.title}</h1>

                </header>
                <ul>
                    {activeCheck.missions
                     |> Array.map missionRowTpl
                     |> Lit.ofArray}
                </ul>
            </article>
        """

let private missionTpl (setActiveCheck: MissionRecord option -> unit) (mission: MissionRecord) =
    html $"<h1 @click={fun _ -> mission |> Some |> setActiveCheck}>{mission.title}</h1>"

[<HookComponent>]
let View (_id: string option) =
    let checks, setChecks = Hook.useState [||]
    let activeCheck, setActiveCheck = Hook.useState None
    Hook.useEffectOnce (downloadMissions setChecks)
    Hook.useEffectOnce (downloadMission (_id, setActiveCheck))

    html
        $"""
        <article class="missions">
            <section class="mission-detail">
                {missionDetail activeCheck}
            </section>
            <section class="mission-list">
                {Lit.mapUnique (fun (f: MissionRecord) -> f._id) (missionTpl setActiveCheck) checks}
            </section>
        </article>
        """
