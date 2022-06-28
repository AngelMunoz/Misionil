module Navigo

open System
open Fable.Core

type DoneFn<'T> = 'T -> unit

type GenerateOptions =
    abstract includeRoot: bool

[<StringEnum>]
type ResolveStrategy =
    | [<CompiledName "ONE">] One
    | [<CompiledName "ALL">] All

type ResolveOptions =
    abstract strategy: ResolveStrategy option
    abstract hash: bool option
    abstract noMatchWarning: bool option

type NavigateOptions =
    abstract title: string option
    abstract state: obj option
    abstract historyAPIMethod: string option
    abstract updateBrowserURL: bool option
    abstract callHandler: bool option
    abstract callHooks: bool option
    abstract updateState: bool option
    abstract force: bool option
    abstract resolveOptions: ResolveOptions option

type RouterOptions =
    inherit NavigateOptions
    abstract linksSelector: string option

type Route =
    abstract name: string
    abstract path: string
    abstract Handler: 'T -> unit
    abstract hooks: RouterHooks

and RouterHooks =
    abstract before: ((DoneFn<obj> * Match) -> unit) option
    abstract after: (Match -> unit) option
    abstract leave: ((DoneFn<obj> * U2<Match, Match array>) -> unit) option
    abstract already: (Match -> unit) option

and Match =
    abstract url: string
    abstract queryString: string
    abstract hashString: string
    abstract route: Route
    abstract data: Option<obj>
    abstract ``params``: Option<obj>


[<ImportDefault("navigo")>]
type Navigo(root: string, options: obj) =
    member _.destroyed: bool = jsNative
    member _.current: Match array = jsNative
    member _.routes: Route array = jsNative
    member _.on<'T>(fn: 'T -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.on(fn: Match -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.on<'T>(path: string, fn: 'T -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.on(path: string, fn: Match -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.on(map: obj, ?hooks: RouterHooks) : Navigo = jsNative
    member _.off(path: string) : Navigo = jsNative
    member _.off<'T>(fn: 'T -> unit) : Navigo = jsNative
    member _.navigate(where: string, ?options: NavigateOptions) : unit = jsNative
    member _.navigateByName(name: string, ?data: obj, ?options: NavigateOptions) : unit = jsNative
    member _.resolve(?path: string, ?resolveOptions: ResolveOptions) : U2<bool, Match> = jsNative
    member _.destroy() : unit = jsNative
    member _.notFound<'T>(handler: 'T -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.notFound(handler: obj -> unit, ?hooks: RouterHooks) : Navigo = jsNative
    member _.updatePageLinks() : Navigo = jsNative
    member _.link(path: string) : string = jsNative
    member _.lastResolved() : (Match array) option = jsNative
    member _.generate(name: string, ?data: obj, ?options: GenerateOptions) : string = jsNative
    member _.hooks(hooks: RouterHooks) : Navigo = jsNative
    member _.getLinkPath(link: obj) : string = jsNative
    member _.``match``(path: string) : U2<bool, Match array> = jsNative

    member _.matchLocation(path: string, ?currentLocation: string, ?annotatePathWithRoot: bool) : U2<bool, Match> =
        jsNative

    member _.getCurrentLocation() : Match = jsNative
    member _.addBeforeHook(route: U2<string, Route>, hookFunction: Func<_>) : Func<_> = jsNative
    member _.addAfterHook(route: U2<string, Route>, hookFunction: Func<_>) : Func<_> = jsNative
    member _.addAlreadyHook(route: U2<string, Route>, hookFunction: Func<_>) : Func<_> = jsNative
    member _.addLeaveHook(route: U2<string, Route>, hookFunction: Func<_>) : Func<_> = jsNative
    member _.getRoute(name: string) : Route option = jsNative
