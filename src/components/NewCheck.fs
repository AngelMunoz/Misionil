module Components.NewCheck

open Lit
open Fable.Core
open Browser.Types
open System
open Types

open Fable.Core.JsInterop
open Router


let private saveMission (record: MissionRecord) : JS.Promise<PouchResponse> = importMember "../database.js"

type private MisionOptions =
    { checkAmount: int; valuePerCheck: int }


let private onFormSubmission (e: Event, missionValue: int) =
    e.preventDefault ()
    let form = createFormData e.target
    let today = DateTime.Today
    let title = $"{DateTime.Today.ToLongDateString()} - Misiones"
    let missions = form.values ()
    let _id = $"{DateTime.Now}"

    let transformedMissions: MissionRow array =
        (unbox<string seq> missions)
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)
        |> Seq.map (fun s -> {| name = unbox s; isDone = false |})
        |> Array.ofSeq

    let record: MissionRecord =
        {| _id = _id
           rev = None
           createdAt = today
           title = title
           missions = transformedMissions
           missionValue = missionValue |}

    saveMission record
    |> Promise.tap (fun s -> JS.console.log s)
    |> Promise.start

    Router.navigate $"/checklist?_id={_id}"


let private setValuePerCheck (value: int, config: MisionOptions, setConfig: MisionOptions -> unit) =
    { config with valuePerCheck = value } |> setConfig

let private setCheckAmount (value: int, config: MisionOptions, setConfig: MisionOptions -> unit) =
    { config with checkAmount = value } |> setConfig

let private missionCheckTpl (index) =
    html
        $"""
        <section class="form-section">
          <sl-input type="text" .name={$"mission-{index}"}></sl-input>
        </section>
        """

[<HookComponent>]
let View (title: string option) =
    let config, setConfig = Hook.useState { checkAmount = 5; valuePerCheck = 1 }

    html
        $"""
        <article class="checklist-page">
          <header>
            <h3>Configuraciones de las Tareas</h3>
            <section class="form-section">
              <label for="">Cantidad de misiones</label>
              <sl-input
                placeholder="Cuantas misiones?"
                type="number"
                name="checkAmount"
                .min={1}
                .step={1}
                .value={config.checkAmount}
                @sl-change={fun (e: Event) -> setCheckAmount (e.targetValueAsInt, config, setConfig)}></sl-input>
            </section>
            <section class="form-section">
              <label for="">Valor de cada mision</label>
              <sl-input
                placeholder="Cuanto vale cada mision?"
                type="number"
                name="valuePerCheck"
                .min={1}
                .step={1}
                .value={config.valuePerCheck}
                @sl-change={fun (e: Event) -> setValuePerCheck (e.targetValueAsInt, config, setConfig)}></sl-input>
            </section>
          </header>
          <form @submit={fun e -> onFormSubmission (e, config.valuePerCheck)}>
            <h3>{DateTime.Now.ToLongDateString()} - Misiones</h3>
            <hr>
            {Lit.mapUnique (fun i -> $"{i}") missionCheckTpl [ 0 .. config.checkAmount ]}
            <section class="form-value">
              <sl-button type="submit" variant="primary">Guardar</sl-button>
            </section>
          </form>
        </article>
        """
