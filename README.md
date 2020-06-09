# Instrumentation (i13n) Toolkit

This is a small instrumentation toolkit for .NET components which is designed to be simple, lightweight and robust enough for use in production.

# Design

The heart of the project is a `Monitor` instance which acts like a central store for instrumentation.

## Counters

A `Counter` is a named wiget that holds a count of something. For example, everytime a exception is caught, a counter can be incremented with the the name of the exception type (e.g. `IllegalArgument`).

`Counter` instances are named and are accessible by their name in the `Monitor`. All you need to do is call `monitor.increment("myCounter")` to increment a named counter. You can increase the value of a counter by more than one by calling `monitor.Increase("myCounter,5)`  to incr

## Timers

A timer is like a mini stopwatch that can be started and stopped several times to accumulate time spent doing someting.

Like counters, Timers are named and addressable by their name in the monitor.

It is possible to have many timers open at once.

## OpenMetrics

This is a singleton to help make instrumenting your code much easier. Instead of managing multiple `Monitor` instances or relying on dependency injection, just reference the OpenMetrics fixture anywhere in the runtime for a access to `Counter` and `Timer` instances.

The `OpenMetrics` fixture also supports the `Serialize` method to generate [Open Metrics](https://openmetrics.io/) to be exposed for your monitoring solution. Each `Counter` is exposed as an individual metric and each `Timer` exposes 8 metrics:
* _**<Timer.Name>_hits**_ - Number of times the timer was created,
* _**<Timer.Name>_avg**_ - The average duration of all timer instances,
* _**<Timer.Name>_total**_ - The total time in milliseconds spent in all instances of this timer,
* _**<Timer.Name>_min**_ - The minimum time in milliseconds spent in any instance of this timer
* _**<Timer.Name>_max**_ - The maximum time in milliseconds spent in any instance of this time
* _**<Timer.Name>_std**_ - The standard deviation of time in milliseconds spent in all instances of this timer
* _**<Timer.Name>_active**_ - The number of timers currently active,
* _**<Timer.Name>_maxactive**_ - The maximum number of instances of this timer active at one time

Using the `OpenMetrics` fixture, you can easily instrument your IIS application and make it observable to monitoring solutions such as [Prometheus](https://prometheus.io/) by exposing the fixture throug an ASP controller. All `Counter` and `Timer` instances called anywhere in the application runtime becomes immediately observable.
