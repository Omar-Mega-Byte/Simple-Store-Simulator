// Simple Store Simulator - Main Program
// Programming Languages - 3 Course Project
// Team Leader: عمر أحمد الرفاعي طليس
// UI Developer: علي محمد جمعة زكي

open Product
open ConsoleUI
open System

// ============================================
// MAIN PROGRAM - USES NEW UI MODULE
// ============================================
// The complete UI implementation is now in the UI module:
// - DisplayHelpers.fs - Formatting and display functions
// - Menu.fs - Menu system and navigation
// - ConsoleUI.fs - Main UI logic and handlers

// ============================================
// PROGRAM ENTRY POINT
// ============================================

[<EntryPoint>]
let main argv =
    // Initialize the product catalog
    let catalog = initializeCatalog()
    
    // Start the application using the new UI module
    startApplication catalog
    
    0 // Return success exit code
