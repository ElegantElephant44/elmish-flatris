module Flatris.Grid

type Cell<'a> = {value: 'a; pos: int*int} 


type Grid<'a> = Cell<'a> list 

let inline (++) x y = fst x + fst y, snd x + snd y
    
let fromList value  l = l |> List.map (fun x -> {value = value; pos = x})
let empty:Grid<'a> = []

let getEdge (grid:Grid<'a>) = grid |> (List.map (fun {pos = x,y} -> max x y) >> List.max)

let rotate grid =    
    grid
    |> List.map (
        fun cell -> {cell with pos = snd cell.pos, (getEdge grid) - fst cell.pos} )

let rec stamp x y  sample grid = 
    List.map (fun cell -> {cell with pos = cell.pos ++ (x,y)}) sample
    @ grid

let rec collide width height x y sample grid =    
    let checkCell cell = 
        let (newX, newY) = cell.pos ++ (x,y)
        newX >= width || newX < 0 || newY >= height || List.exists (fun {pos = p} -> p = (newX, newY)) grid
    sample |> List.exists checkCell
         

let fullLine width grid = 
    grid 
    |> Seq.ofList
    |> Seq.groupBy (fun {pos=(_,y)} -> y)
    |> Seq.tryPick (fun (y, cells) -> if Seq.length cells = width then Some y else None)
    

let rec clearLines width grid lineCounter =
    match fullLine width grid with
    | None -> (grid, lineCounter)
    | Some lineY -> 
        let filtered = grid |> List.filter (fun {pos=(_,y)} -> y <> lineY)
        let (above, below) = filtered |> List.partition (fun {pos=(_,y)} -> y < lineY)
        let movedAbove = above |> List.map  (fun {value=v; pos=(x,y)} -> {value = v; pos = (x, y + 1)})
        clearLines width (movedAbove @ below) (lineCounter + 1)

let initPosition width grid = 
    let (maxX, maxY) = 
        grid
        |> List.map (fun x -> x.pos) 
        |> List.unzip 
        |> (fun (x,y) -> List.max x, List.max y)     
    (width / 2 - maxX/2 - 1), -maxY - 1
