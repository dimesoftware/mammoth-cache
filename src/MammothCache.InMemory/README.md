# MammotCache

A caching library that provides mammoth-sized caching capabilities, just like the extinct mammoth.
 
More seriously though, this library is mostly a decorator that wraps existing caching frameworks and exposes a single API.

## About this project

This project provides a framework agnostic `ICache` interface which contains a set of methods such as `Set<T>` and `Get<T>`. The project also includes implementations of the `ICache`, such as the `InMemoryCache` class.

The primary goal of this project is to decorate existing caching frameworks and abstract the underlying implementations, which may vary greatly. For instance, during development, you may want to use an **in-memory** cache. However, this rarely suffices for production purposes, where you might want to consider **Redis**, just to name one. 

Without having to redesign the application, you can create a wrapper of your preferred caching framework and inject it into the application with the `ICache` interface.
	
## Installation

Use the base package to use throughout your application:

`dotnet add package MammothCache`

In the startup class of your application, add any of the following packages:

- MammothCache.InMemory
- MammotCache.Redis


## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

# License

[![License](http://img.shields.io/:license-mit-brightgreen.svg?style=flat-square)](http://badges.mit-license.org)