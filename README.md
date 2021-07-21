# Repository for homework tasks :mortar_board:

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/makes-people-smile.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-developers.svg)](https://forthebadge.com)

| OS                        | Status                                                                                                                                                 |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Windows win10-x64         | [![Build status](https://ci.appveyor.com/api/projects/status/q35imesu50g2flpg?svg=true)](https://ci.appveyor.com/project/Greenwood/metrics-dotnet-samples) |
| Linux Ubuntu 14.04.5 LTS  | [![Build Status](https://travis-ci.com/DarkSideMoon/metrics-dotnet-samples.svg?branch=main)](https://travis-ci.com/DarkSideMoon/metrics-dotnet-samples) |
| .NET Core Linux           | ![Build Status](https://github.com/DarkSideMoon/metrics-dotnet-samples/actions/workflows/dotnet-core-linux.yml/badge.svg) |
| .NET Core Windows         | ![Build Status](https://github.com/DarkSideMoon/metrics-dotnet-samples/actions/workflows/dotnet-core-windows.yml/badge.svg) |

## App Metrics guide
Getting started you cna find here: https://www.app-metrics.io/getting-started/
Greate guide you can find here: https://www.app-metrics.io/web-monitoring/aspnet-core/

## Up and runnnig TIG stack
I am using this TIG stack (Telegraf, InfluxDb, Grafana) from https://github.com/nicolargo/docker-influxdb-grafana
Also, I make some changes. 
All stuff you can find in folder `environment`

## Localhost urls 
 - Swagger -> http://localhost:5000/swagger
 - Base health check -> http://localhost:5000/healthcheck
 - Db health check -> http://localhost:5000/healthcheck/db
 - UI for health checks -> http://localhost:5000/healthchecks-ui
 - App metrics in json -> http://localhost:5000/metrics
 - App metrics in text -> http://localhost:5000/metrics-text
 - App metrics current environment -> http://localhost:5000/env

## Benchmarks
All benchmarks results you can find in folder [benchmarks](https://github.com/DarkSideMoon/metrics-dotnet-samples/tree/master/benchmarks)

## Homeworks
All homework tasks you can find in *Projects* tab

- [x] Compare databases
- [ ] Service metrics app
- [ ] Google user metrics
- [x] Stress Testing