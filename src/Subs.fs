module Flatris.Subs

open Types
open Fable.Import.Browser
open Elmish

let [<Literal>] LEFT_BTN_CODE = 37
let [<Literal>]  RIGHT_BTN_CODE = 39 
let [<Literal>]  UP_BTN_CODE = 38
let [<Literal>] DOWN_BTN_CODE = 40

let keyDownHandler dispatch (e:KeyboardEvent)=
    let action = 
        match int e.keyCode with
        | LEFT_BTN_CODE -> MoveLeft
        | RIGHT_BTN_CODE -> MoveRight
        | DOWN_BTN_CODE -> Accelerate true
        | UP_BTN_CODE -> Rotate
        | _ -> Noop
    dispatch action |> ignore
    e :> obj

let keyUpHandler dispatch (e:KeyboardEvent)=
    let action = 
        match int e.keyCode with        
        | DOWN_BTN_CODE -> Accelerate false
        | _ -> Noop
    dispatch action |> ignore
    e :> obj

let keySub  dispatch =
    window.addEventListener_keyup(fun e -> keyUpHandler dispatch e) |> ignore
    window.addEventListener_keydown(fun e -> keyDownHandler dispatch e ) |> ignore

let timerTick  dispatch =
    window.setInterval(fun _ -> 
        dispatch (Tick 10)
    , 10) |> ignore

let subscription _ = Cmd.batch [(Cmd.ofSub timerTick); Cmd.ofSub keySub]
