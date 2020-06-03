# Instrumentation (i13n) Toolkit

This is a small instrumentation toolkit for .NET components which is designed to be simple, lightweight and robust enough for use in production.

# Design

The heart of the project is a `Monitor` instance which acts like a central store for instrumentation.

## Counters

A `Counter` is a named wiget that holds a count.

Counters are named and are accessible by their name in the Monitor

## Timers

A timer is like a mini stopwatch that can be started and stopped several times to accumulate time spent doing someting.

Like counters, Timers are named and addressable by their name in the monitor.

It is possible to have many timers open at once.
