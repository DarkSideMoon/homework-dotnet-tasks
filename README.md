# Samples for Net Core metrics :bike:

![Metrics DotNet Samples](https://img.shields.io/badge/-Metrics%20DotNet%20Samples-002157?style=flat-square&logo=GitBook)

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/makes-people-smile.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-developers.svg)](https://forthebadge.com)

| OS                        | Status                                                                                                                                                 |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Windows win10-x64         | [![Build status](https://ci.appveyor.com/api/projects/status/q35imesu50g2flpg/branch/main?svg=true)](https://ci.appveyor.com/project/Greenwood/metrics-dotnet-samples/branch/main) |
| Linux Ubuntu 14.04.5 LTS  | [![Build Status](https://travis-ci.com/DarkSideMoon/metrics-dotnet-samples.svg?branch=main)](https://travis-ci.com/DarkSideMoon/metrics-dotnet-samples) |

## App Metrics guide
Getting started you cna find here: https://www.app-metrics.io/getting-started/
Greate guide you can find here: https://www.app-metrics.io/web-monitoring/aspnet-core/

## Localhost urls 
 - Swagger -> http://localhost:5000/swagger
 - Base health check -> http://localhost:5000/healthcheck
 - Db health check -> http://localhost:5000/healthcheck/db
 - UI for health checks -> http://localhost:5000/healthchecks-ui
 - App metrics in json -> http://localhost:5000/metrics
 - App metrics in text -> http://localhost:5000/metrics-text
 - App metrics current environment -> http://localhost:5000/env

## Up and runnnig TIG stack
I am using this TIG stack (Telegraf, InfluxDb, Grafana) from https://github.com/nicolargo/docker-influxdb-grafana
Also, I make some changes. 
All stuff you can find in folder `environment`