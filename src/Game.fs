module Flatris.Game

open Types
open Flatris.Grid
open Flatris.Tetriminos

let moveTetrimino model =
    match model.sideMove with
    |Some dx ->
        let (x, y) = model.position
        let newX = x + dx        
        if Grid.collide model.width model.height newX (y |> int) model.active model.grid
        then  { model with sideMove = None}
        else { model with position = newX,y; sideMove = None}
    |None -> model
  

let rotateTetrimino model =
    match model.rotation with
    | false -> model
    | true -> 
        let (x,y) = model.position
        let rotated = rotate model.active
        if collide model.width model.height x (y|>int) rotated model.grid
        then {model with rotation = false}
        else {model with rotation = false; active = rotated}


let level model = model.lines / 10 + 1

let spawnTetrimino model =
    let active = if model.next |> List.isEmpty then getFromColorSet model.random else model.next
    let next = getFromColorSet model.random
    let (x,y) = initPosition model.width active
    { model with next = next; active = active; position = ( x, float y)}

let clearLines model =
    let (grid, lines) = Grid.clearLines model.width model.grid 0
    let bonus =
        match lines with
        |0 -> 0
        |1 -> 100
        |2 -> 300
        |3 -> 500
        |_ -> 800
    { model with grid = grid; score = model.score + bonus * level model; lines = model.lines + lines}

let dropTetrimino elapsed model =
    let (x,y) = model.position
    let speed = if model.acceleration then 25 else max 25 (800 - 25 * (level model - 1))
    let newY = y + float elapsed / float speed
    if collide model.width model.height x (newY |> int) model.active model.grid
    then
        { model with 
            grid = stamp x (y |> int) model.active model.grid;
            score = model.score + List.length model.active * (if model.acceleration then 2 else 1)}
        |> spawnTetrimino
        |> clearLines
    else
        { model with position = ( x, newY ) }

let checkEndGame model =
    if List.exists (fun {pos=p} -> snd p < 0) model.grid
    then { model with state = Stopped }
    else model

let animate elapsed model =
    model
        |> moveTetrimino
        |> rotateTetrimino
        |> dropTetrimino elapsed
        |> checkEndGame
