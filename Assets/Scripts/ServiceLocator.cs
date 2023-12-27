using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Responsibility: Its primary role is to act as a central hub for services that can be used throughout the game, allowing for 
easy access to common functionalities like logging, AI services, or data management.
SOLID Principles: The Service Locator pattern is a form of Dependency Injection, which is in line with the Dependency Inversion 
Principle. It allows high-level modules (like your game logic) to depend on abstractions (like interfaces for logging or AI) 
rather than concrete implementations.
*/
public class ServiceLocator
{
    private static ServiceLocator _instance;
    private Dictionary<Type, object> _services;

    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ServiceLocator();
            }
            return _instance;
        }
    }

    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void RegisterService<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        return default;
    }
}