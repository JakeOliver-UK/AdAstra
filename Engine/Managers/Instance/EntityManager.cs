using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Utils;
using System;
using System.Linq;

namespace AdAstra.Engine.Managers.Instance
{
    internal class EntityManager : IDisposable
    {
        public bool IsOverlay => _isOverlay;

        private readonly bool _isOverlay;
        private Entity[] _entities;

        public EntityManager(bool isOverlay)
        {
            _isOverlay = isOverlay;
            _entities = new Entity[5000];
            Initialize();
        }

        public EntityManager(bool isOverlay, int poolSize)
        {
            _isOverlay = isOverlay;
            if (poolSize <= 0) poolSize = 1;
            _entities = new Entity[poolSize];
            Initialize();
        }

        public void Initialize()
        {
            for (int i = 0; i < _entities.Length; i++)
            {
                _entities[i] = new();
            }
        }

        public Entity Create(string name)
        {
            string type = _isOverlay ? "Overlay" : "World";
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to create entity '{name}' in {type} Entity Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }

            if (Contains(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to create entity '{name}' in {type} Entity Manager as an entity with that name already exists.");
                return null;
            }

            Entity entity = GetNext();

            if (entity == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to create entity '{name}' in {type} Entity Manager as entity pool is empty.");
                return null;
            }

            entity.Name = name;
            entity.Initialize(this);
            return entity;
        }

        private Entity GetNext()
        {
            for (int i = 0; i < _entities.Length; i++)
            {
                if (_entities[i] != null && !_entities[i].IsActive) return _entities[i];
            }
            return null;
        }

        public Entity Get(string name)
        {
            string type = _isOverlay ? "Overlay" : "World";

            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get entity '{name}' from {type} Entity Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }

            return _entities.FirstOrDefault(e => e != null && e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Entity[] GetWithTag(string tag)
        {
            string type = _isOverlay ? "Overlay" : "World";

            if (string.IsNullOrWhiteSpace(tag))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get entities with tag '{tag}' from {type} Entity Manager as tag cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }

            return [.. _entities.Where(e => e != null && e.Tags.Contains(tag))];
        }

        public Entity[] GetWithComponent<T>() where T : Component => [.. _entities.Where(e => e != null && e.GetComponent<T>() != null)];

        public void Update()
        {
            Entity[] activeEntities = GetActive();

            for (int i = 0; i < activeEntities.Length; i++)
            {
                activeEntities[i].Update();
            }
        }

        public void Draw()
        {
            Entity[] activeEntities = GetActive();

            for (int i = 0; i < activeEntities.Length; i++)
            {
                activeEntities[i].Draw();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _entities.Length; i++)
            {
                _entities[i]?.Dispose();
                _entities[i] = null;
            }
            _entities = null;
        }

        public Entity this[string name] => Get(name);
        public Entity[] GetActive() => [.. _entities.Where(e => e != null && e.IsActive)];
        public Entity[] GetInactive() => [.. _entities.Where(e => e != null && !e.IsActive)];
        public bool Contains(string name) => _entities.Any(e => e != null && e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        public int CountActive() => _entities.Count(e => e != null && e.IsActive);
        public int CountInactive() => _entities.Count(e => e != null && !e.IsActive);
        public int Count => _entities.Length;
    }
}
