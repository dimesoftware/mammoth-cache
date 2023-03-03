# Redis Caching Decorator

Simple caching contracts and implementation using existing caching frameworks.

## About this project

This project provides a framework agnostic `ICache` interface which contains a set of methods such as `Set<T>` and `Get<T>`. 
This package is an implementation of this contract for **Redis** using the popular `StackExchange.Redis` package.
	
## Installation

`dotnet add package MammothCache.Redis`

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

# License

[![License](http://img.shields.io/:license-mit-brightgreen.svg?style=flat-square)](http://badges.mit-license.org)