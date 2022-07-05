[<AutoOpen>]
module Extensions

open Browser.Types
open Fable.Core

[<Emit("new Event($0, $1)")>]
let inline createEvent name options : Event = jsNative

[<Emit("new CustomEvent($0, $1)")>]
let inline createCustomEvent name options : CustomEvent<_> = jsNative


[<Emit("new FormData($0)")>]
let inline createFormData (form: EventTarget) : FormData = jsNative


type Event with
    member self.targetValueAsInt =
        (self.target :?> HTMLInputElement).valueAsNumber
        |> int

    member self.targetChecked = (self.target :?> HTMLInputElement).``checked``
