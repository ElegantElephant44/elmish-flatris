module Flatris.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Types
open Grid
open Flatris

let [<Literal>] BLOCK_SIZE = 25

let label text =
    div
        [ ClassName "label" ]
        [ str text]
let counter (value:int) =
    div
        [ ClassName "counter" ]
        [ str (value.ToString())]

let playButton state dispatch = 
    let (text, msg) = 
        match state with
        |Playing -> "Pause", Pause
        |Stopped -> "New game", Start
        |Paused -> "Resume", Resume
    button         
        [ClassName "play-button"
         OnClick (fun _ -> dispatch msg)] [str text]

let renderBlock (x,y) (color:Color) size borderStroke=
    let (Color c) = color
    rect
        [   X (!! float (x * size) + borderStroke);
            Y (!! float (y * size) + borderStroke);
            SVGAttr.Width (!! size - borderStroke * 2.0); 
            SVGAttr.Height (!! size - borderStroke * 2.0);
            !! ("fill", c);
             ] 
        []
let renderGridWithSpans grid =
    List.map (fun b-> renderBlock b.pos b.value BLOCK_SIZE 0.5) grid

let renderGridWithoutSpans grid =
    List.map (fun b -> renderBlock b.pos b.value BLOCK_SIZE 0.0) grid
 
let renderGridWithActive model renderGrid =
    renderGrid(stamp (fst model.position) (snd model.position |> int) model.active model.grid) 
    @ renderGrid model.grid

let drawGameField model =    
    svg
        [Style [Width (model.width * BLOCK_SIZE)
                Height (model.height * BLOCK_SIZE)
                BackgroundColor "rgba(52, 73, 95, 0.1)"]]                
        (renderGridWithActive model renderGridWithSpans)

let infoPanel model =
    div
        [ClassName "info-panel"
         Style [Display (if model.state <> Playing then "block" else "none")]]
        [p
            []
            [str "Play with a keyboard using the arrow keys ← ↑ ↓ →"]
        ]

let root (model:Model) dispatch =
    div
        [ ClassName "main" ]
        [ div
            [ ClassName "main-div" ]        
            [ drawGameField model
              infoPanel model
              div
                [ ClassName "score-panel" ]
                [ div 
                    [ ClassName "title" ]
                    [ str "Flatris" ] 
                  label "Score"
                  counter model.score
                  label "Lines cleared"
                  counter model.lines
                  label "Next shape" 
                  svg 
                    [] 
                    (renderGridWithSpans model.next)                    
                  playButton model.state dispatch
                ] ] ]
          
