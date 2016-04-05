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

type GameLibrary() =
    let container = new GameContainer()
    let players = new HashSet<IPlayer>()
    do
        players.Add(new Player("Henk")) |> ignore
        players.Add(new Player("Bob")) |> ignore
    let game = container.Create(players)
    do
        game.Start()
    member this.Commands() =
        container.GetCommandFactory(game)
    member this.Get() =
        game

type GameHub(library :GameLibrary) =
    inherit Hub()
    do
        printf "New GameHub"

    member this.RollDice() =
        printfn "Roll Dice"
        let commandfactory = library.Commands()
        let game = library.Get()
        let rolldice = commandfactory.NewRollDice(game.Players.First())
        game.Execute(rolldice)
        printf "Result %d" rolldice.Dice.Current.Result

type Startup() =
    member this.Configuration(app :IAppBuilder) =
        let library = new GameLibrary()
        GlobalHost.DependencyResolver.Register(typedefof<GameHub>, fun() -> new GameHub(library) :> obj)
        app.MapSignalR() |> ignore

[<EntryPoint>]
let main argv = 
    let host = "http://localhost:8085"
    use app = WebApp.Start<Startup>(host)

    printfn "Server running on %s" host
    Console.ReadLine() |> ignore
    0 // return an integer exit code