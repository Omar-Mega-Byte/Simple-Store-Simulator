namespace SimpleStoreSimulator.GUI.Converters

open System
open System.Globalization
open Avalonia.Data.Converters
open Avalonia.Media

type BoolToColorConverter() =
    interface IValueConverter with
        member this.Convert(value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            match value with
            | :? bool as b -> 
                if b then 
                    box Brushes.Red // Low stock
                else 
                    box Brushes.Green // In stock
            | _ -> box Brushes.Black

        member this.ConvertBack(value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            raise (NotImplementedException())
