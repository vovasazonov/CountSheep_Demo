using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Engine;
using Project.CoreDomain.Services.FileLoader;
using Project.CoreDomain.Services.Serialization;
using UnityEngine;

namespace Project.CoreDomain.Services.Data
{
    public class DataStorageService : IDataStorageService, IDisposable, ITaskAsyncInitializable
    {
        private readonly string _path = $"{Application.persistentDataPath}/.data";
        private readonly IFileLoader _fileLoader;
        private readonly ISerializerService _serializerService;
        private readonly IEngineService _engineService;
        private readonly Dictionary<string, object> _deserializedData = new();
        private Dictionary<string, object> _serializedData;
        private bool _isLoaded;

        public DataStorageService(
            ISerializerService serializerService,
            IEngineService engineService,
            IFileLoaderService fileLoaderService
        )
        {
            _serializerService = serializerService;
            _engineService = engineService;
            _fileLoader = fileLoaderService.Binary;
            AddListeners();
        }

        private void AddListeners()
        {
            _engineService.Paused += OnAppPause;
        }

        private void RemoveListeners()
        {
            _engineService.Paused -= OnAppPause;
        }

        private void OnAppPause()
        {
            if (_isLoaded)
            {
                Save();
            }
        }

        public void Load()
        {
            object data = _fileLoader.Load<object>(_path);
            _serializedData = data == null ? new Dictionary<string, object>() : _serializerService.DeserializeJson<Dictionary<string, object>>(data) ?? new Dictionary<string, object>();
            _isLoaded = true;
        }

        public bool Contains(string key)
        {
            return _serializedData.ContainsKey(key) || _deserializedData.ContainsKey(key);
        }

        public T Get<T>(string key) where T : class
        {
            if (!_deserializedData.ContainsKey(key))
            {
                Deserialize<T>(key);
            }

            return _deserializedData[key] as T;
        }

        public T Create<T>(string key) where T : class, new()
        {
            T data = new T();
            _deserializedData.Add(key, data);
            return data;
        }

        public void Reset()
        {
            _serializedData.Clear();
            _deserializedData.Clear();
            Save();
        }

        public void Save()
        {
            foreach (var key in _deserializedData.Keys)
            {
                Serialize(key);
            }

            object serialized = _serializerService.SerializeToJson(_serializedData);
            _fileLoader.Save(serialized, _path);
        }

        private void Serialize(string key)
        {
            var data = _deserializedData[key];
            _serializedData[key] = _serializerService.SerializeToJson(data);
        }

        private void Deserialize<T>(string key)
        {
            var data = _serializedData[key];
            _deserializedData[key] = _serializerService.DeserializeJson<T>(data);
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        public UniTask InitializeAsync()
        {
            Load();
            return UniTask.CompletedTask;
        }
    }
}