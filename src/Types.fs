module Flatris.Types
open System
open Grid

type Color = Color of string

type AnimationState =  { active : bool; elapsed : int }

type State = 
    | Paused
    | Playing
    | Stopped

type Model = 
    {   
        active : Grid<Color>;
        position : int * float;
        grid : Grid<Color>;
        lines : int;
        next : Grid<Color>;
        random : Random;
        score : int;
        state : State;
        acceleration : bool;
        sideMove : int option;
        rotation : bool
        width : int;
        height : int;        
    }

type Msg = 
    | Start
    | Pause
    | Resume    
    | Tick of int
    | MoveLeft
    | MoveRight
    | Rotate
    | Accelerate of bool
    | Noop
