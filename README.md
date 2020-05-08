# Introduction

Simple caching contracts and implementation using existing caching frameworks.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

This project provides a framework agnostic `ICache` interface which contains a set of methods such as  `Set<T>` and `Get<T>`. The project also includes a few implementations of the `ICache`, mostly `CacheManager` was used.

The primary goal of this project is to have a **facade** that abstracts the underlying implementations, which may vary greatly. For example, during development, you may want to use an in-memory cache. However, this rarely suffices for production-purposes. Without having to redesign the application, you can create a wrapper of your preferred caching framework and inject it into the application with the `ICache` interface.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

`dotnet add package Dime.Caching`

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

# License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)