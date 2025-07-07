---
marp: true
title: A Very Opinionated ASP.NET Core Architecture
paginate: true
theme: default
author: Al Rodriguez
footer: '@ProgrammerAL and programmerAL.com'
---

# A Very Opinionated ASP.NET Core Architecture

06 - Managed Identity

---

# Today: 06 - Managed Identity

- Replace Connection String with Azure Managed Identity
- Deploy to Azure to see example

---

# Secrets

- Connection String == Secret
- Secrets are:
  - Long Lived
  - Usable by anyone
  - Easy to Leak
  - Work to manage

---

# What is an Azure Managed Identity?

- Abstraction over Service Principal
- Identity assigned to a service, permissions assigned to the identity
- 2 Types - System Assigned and User Assigned

---

# Closing

- This is Very Opinionated
  - Not for everyone
- Question? Just ask

