module Flatris.App

open Elmish
open Elmish.Browser.Navigation
open Fable.Core.JsInterop
open State
open Subs
open View
open Elmish.React
open Elmish.Debug
open Elmish.HMR

importAll "../sass/main.sass"

// App
Program.mkProgram init update root
|> Program.withSubscription subscription
#if DEBUG
|> Program.withDebugger
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
|> Program.run
