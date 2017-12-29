module Flatris.State

open Elmish
open Types
open System
open Flatris
open Flatris.Game

let init() : Model * Cmd<Msg> = 
    {
        active = Grid.empty;
        position = 0, 0.0;
        grid = Grid.empty;
        lines = 0;
        next = Grid.empty;
        random = Random();        
        score = 0;
        state = Stopped;
        acceleration = false;
        sideMove = None;
        rotation = false;
        width = 10;
        height = 20;
    }, []

let update msg model =
    match msg with
    | Start -> 
        {(init()|> fst) with state = Playing}
        |> (spawnTetrimino),[]
    | Pause -> 
        {model with state = Paused}, []
    | Resume -> 
        {model with state = Playing},[]
    | Tick time -> 
        if model.state = Playing 
        then
            (model |> animate (min time 25)),[]
        else 
            model,[]    
    | MoveLeft -> {model with sideMove = Some -1},[]    
    | MoveRight -> {model with sideMove = Some 1},[]    
    | Rotate -> {model with rotation = true},[]
    | Accelerate on -> {model with acceleration = on},[]
    | Noop -> model,[]
    