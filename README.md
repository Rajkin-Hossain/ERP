# DDD + Clean Architecture (.NET 10) + MongoDB + RabbitMQ + Outbox

A production-ready template for building a **DDD-based** service using **Clean Architecture** on **.NET 10**, with:
- **MongoDB** for persistence
- **RabbitMQ** for messaging
- **Outbox Pattern** for reliable event publishing (no lost events, no dual-write issues)

---

## Goals

- Keep **Domain** pure (business rules first).
- Keep dependencies pointing **inward** (Clean Architecture).
- Publish integration events **reliably** using **Outbox** + background dispatcher.
- Support microservice-ready boundaries (one bounded context per service).

---

## Tech Stack

- **.NET 10**
- **MongoDB**
- **RabbitMQ**
- **MassTransit** (recommended) or your own `IServiceBus` abstraction
- **Outbox** (stored in MongoDB)
- (Optional) **Serilog**, **FluentValidation**, **MediatR**, **HealthChecks**

---

## Architecture Overview

Clean Architecture dependency rule:

