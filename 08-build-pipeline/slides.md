---
marp: true
title: A Very Opinionated ASP.NET Core Architecture
paginate: true
theme: default
author: Al Rodriguez
footer: '@ProgrammerAL and programmerAL.com'
---

# A Very Opinionated ASP.NET Core Architecture

08 - Build with Cake

---

# Today: 08 - Build with Cake

- Review what C# Cake is
- Create the inital Cake Build script
- Part 1 of 2

---

# Why should developers do a "DevOps Task"?

- Build is commonly owned by a "DevOps" team
* We know the app
* We automate
* Our tools are better

---

# C# Cake

* Framework for writing CI/CD with C#
  - Other tools like it, Nuke.build
* Default is a single file
  - We'll use Cake.Frosting
  - https://cakebuild.net/docs/getting-started/setting-up-a-new-scripting-project

---

# Build Pipeline and Cake

- YAML pipeline will do proprietary stuff
  - Ex: GitHub Actions will install correct version of .NET
- YAML will run the C# Cake.Frosting project via CLI
  - `dotnet run my-build.csproj`

---

# Closing

- This is Very Opinionated
  - Not for everyone
- Question? Just ask

