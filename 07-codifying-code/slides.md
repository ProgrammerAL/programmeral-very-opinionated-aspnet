---
marp: true
title: A Very Opinionated ASP.NET Core Architecture
paginate: true
theme: default
author: Al Rodriguez
footer: '@ProgrammerAL and programmerAL.com'
---

# A Very Opinionated ASP.NET Core Architecture

07 - Codifying Code

---

# Today: 07 - Codifying Code

- Review what the Roslyn Compiler is
- Review an existing code Analyzer
- Review an existing Source Generator

---

# Roslyn Compiler

* Original C# compiler written in C++
* Second version of .NET Compiler called Roslyn
  - C# compiler written in C#, VB.NET compilter written in VB.NET
  - F# has its own thing. Written in F#, but not Roslyn
* Roslyn took learnings from a decade of C#
* Added an API to plug in custom stuff

---

# Roslyn Analyzers

- Custom code to scan code at compile time
- Built-in analyzers and added through NuGet packages

---

# Source Generator

- Generates code at compile time
- Additive only
  - Cannot edit code, only generates new code

---

# Closing

- This is Very Opinionated
  - Not for everyone
- Question? Just ask

