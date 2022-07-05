module Components.CheckList

open Lit
open System
open Types
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open Browser.Dom


let private getMissions () : JS.Promise<MissionRecord array> = importMember "../database.js"
let private getMission (_id: string) : JS.Promise<MissionRecord> = importMember "../database.js"
let private removeMission (_id: string, rev: string) : JS.Promise<PouchResponse> = importMember "../database.js"
let private saveMission (record: MissionRecord) : JS.Promise<PouchResponse> = importMember "../database.js"

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


let private missionsTemplate (activeCheck: MissionRecord) (updateChecks: MissionRecord -> unit) =
    let handleChange (index: int, row: MissionRow) (event: Event) =
        let row =
            {| row with
                isDone = event.targetChecked |}

        activeCheck.missions[ index ] <- row
        updateChecks activeCheck

    activeCheck.missions
    |> Array.mapi (fun index row ->
        html
            $"""<li class="form-section">
                    <sl-checkbox
                        ?checked={row.isDone}
                        @sl-change={handleChange (index, row)}>{row.name}</sl-checkbox>
                </li>""")

let private missionDetail
    (
        isPrinting: bool,
        setIsPrinting: bool -> unit,
        activeCheck: MissionRecord option,
        updateChecks: MissionRecord -> unit
    ) =
    match activeCheck with
    | None -> Lit.nothing
    | Some activeCheck ->
        let closePrintBtn =
            if isPrinting |> not then
                html
                    $"""
                    <sl-button
                        class="print-hidden"
                        variant="primary"
                        @click={fun _ -> setIsPrinting true}>
                        Imprimir
                    </sl-button>
                    """
            else
                html
                    $"""
                    <sl-button
                        class="print-hidden"
                        variant="primary"
                        @click={fun _ -> window.print}>
                        Imprimir
                    </sl-button>
                    <sl-button
                        class="print-hidden"
                        variant="accent"
                        @click={fun _ -> setIsPrinting false}>
                        Cerrar
                    </sl-button>
                    """

        html
            $"""
            <article class="mission-detail">
                <header>
                    <h1>{activeCheck.title}</h1>
                    {closePrintBtn}
                </header>
                <ul>
                    {missionsTemplate activeCheck updateChecks
                     |> Lit.ofArray}
                </ul>
            </article>
        """

let private missionTpl (setActiveCheck: MissionRecord option -> unit) (mission: MissionRecord) =
    html $"<h1 @click={fun _ -> mission |> Some |> setActiveCheck}>{mission.title}</h1>"

let saveCheckAndUpdateList setActiveCheck setChecks (record: MissionRecord) =
    saveMission record
    |> Promise.tap (fun result ->
        {| record with _rev = Some result.rev |}
        |> Some
        |> setActiveCheck)
    |> Promise.tap (fun _ -> downloadMissions setChecks ())
    |> Promise.start

[<HookComponent>]
let View (_id: string option) =
    let checks, setChecks = Hook.useState [||]
    let activeCheck, setActiveCheck = Hook.useState None
    let isPrinting, setIsPrinting = Hook.useState false
    Hook.useEffectOnce (downloadMissions setChecks)
    Hook.useEffectOnce (downloadMission (_id, setActiveCheck))

    html
        $"""
        <article class="missions">
            <section class={Lit.classes
                                [ "mission-detail"
                                  if isPrinting then "printing" ]}>
                {missionDetail (isPrinting, setIsPrinting, activeCheck, (saveCheckAndUpdateList setActiveCheck setChecks))}
            </section>
            <section class="mission-list print-hidden">
                {Lit.mapUnique (fun (f: MissionRecord) -> f._id) (missionTpl setActiveCheck) checks}
            </section>
        </article>
        """
