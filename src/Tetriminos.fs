module Flatris.Tetriminos
open Grid
open System
open Types

let tetriminos = 
    [| [ ( 0, 1 ); ( 1, 1 ); ( 2, 1 ); ( 3, 1 ) ];
     [ ( 0, 0 ); ( 1, 0 ); ( 0, 1 ); ( 1, 1 ) ]; 
     [ ( 1, 0 ); ( 0, 1 ); ( 1, 1 ); ( 2, 1 ) ];
     [ ( 0, 0 ); ( 0, 1 ); ( 1, 1 ); ( 2, 1 ) ];
     [ ( 2, 0 ); ( 0, 1 ); ( 1, 1 ); ( 2, 1 ) ];
     [ ( 1, 0 ); ( 2, 0 ); ( 0, 1 ); ( 1, 1 ) ];
     [ ( 0, 0 ); ( 1, 0 ); ( 1, 1 ); ( 2, 1 ) ]|]

let colors =
    [|
        "#505160";
        "#68829e";
        "#598234";
        "#003b46";
        "#07575b";
        "#004445";
        "#de7a22";
        "#6ab187";
    |]

let getNextTertimino (random:Random) (a:'a) : Grid<'a> = 
    tetriminos.[random.Next(0,tetriminos.Length)] |> fromList a

let getRandomColorful (random:Random): Grid<Color> =
    let color = Color ("#" + random.Next(0,Convert.ToInt32("FFFFFF", 16)).ToString("x6"))
    getNextTertimino random color

let getFromColorSet (random:Random): Grid<Color> =
    let color = Color (colors.[random.Next(0,colors.Length)])
    getNextTertimino random color
