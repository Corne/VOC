// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Collections.Generic
open Owin
open Microsoft.AspNet.SignalR
open Microsoft.AspNet.SignalR.Hubs
open Microsoft.Owin.Hosting
open VOC.Core.Games
open VOC.Core.Players
open System.Linq

type MyHub() =
    inherit Hub()
    member this.Test(message: string) =
        printfn "Test Message: %s" message

type GameHub() =
    inherit Hub()
    let container = new GameContainer()
    let players = new HashSet<IPlayer>()
    do
        players.Add(new Player("Henk")) |> ignore
        players.Add(new Player("Bob")) |> ignore
    let game = container.Create(players)
    do
        game.Start()

    member this.RollDice() =
        printfn "Roll Dice"
        let commandfactory = container.GetCommandFactory(game)
        let rolldice = commandfactory.NewRollDice(players.First())
        game.Execute(rolldice)
        printf "Result %d" rolldice.Dice.Current.Result


[<EntryPoint>]
let main argv = 
    let start (app: IAppBuilder) =
        app.MapSignalR() |> ignore
    let host = "http://localhost:8085"
    use app = WebApp.Start(host, start)

    printfn "Server running on %s" host
    Console.ReadLine() |> ignore
    0 // return an integer exit code

