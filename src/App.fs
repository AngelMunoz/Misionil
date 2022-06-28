[<RequireQualifiedAccess>]
module App

open Lit
open Browser.Dom

[<LitElement "mi-app">]
let private app () =
    let _ = LitElement.init (fun cfg -> cfg.useShadowDom <- false)

    html
        $"""
      <h1>lawl</h1>
     """

let register () = ()
