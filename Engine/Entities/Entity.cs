using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Managers.Instance;
using System;
using System.Collections.Generic;

namespace AdAstra.Engine.Entities
{
    internal class Entity : IDisposable
    {
        public string Name { get; set; }
        public List<string> Tags => _tags;
        public Entity Parent => _parent;
        public bool IsRoot => _parent == null;
        public bool IsActive => _isActive;
        public EntityManager Manager => _manager;
        public Transform Transform => GetComponent<Transform>() ?? AddComponent<Transform>();

        private List<string> _tags;
        private Entity _parent;
        private bool _isActive;
        private EntityManager _manager;
        private List<Entity> _children;
        private List<Component> _components;

        public void Initialize(EntityManager manager)
        {
            if (_isActive) return;
            if (manager == null) return;
            _tags = [];
            _manager = manager;
            _children = [];
            _components = [];
            _isActive = true;
            AddComponent<Transform>();
        }

        public void Update()
        {
            if (!_isActive) return;

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update();
            }
        }

        public void Draw()
        {
            if (!_isActive) return;

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw();
            }
        }

        public void Dispose()
        {
            RemoveChildren();
            RemoveComponents();
            RemoveParent();
            _tags?.Clear();
            _tags = null;
            _children?.Clear();
            _children = null;
            _components?.Clear();
            _components = null;
            _manager = null;
            _parent = null;
            _isActive = false;
        }

        public void AddTag(string tag)
        {
            if (!_isActive || _tags == null) return;
            if (string.IsNullOrWhiteSpace(tag)) return;
            if (_tags.Contains(tag)) return;
            _tags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            if (!_isActive || _tags == null) return false;
            if (string.IsNullOrWhiteSpace(tag)) return false;
            return _tags.Contains(tag);
        }

        public void RemoveTag(string tag)
        {
            if (!_isActive || _tags == null) return;
            if (string.IsNullOrWhiteSpace(tag)) return;
            if (!_tags.Contains(tag)) return;
            _tags.Remove(tag);
        }

        public void SetParent(Entity parent)
        {
            if (!_isActive) return;
            if (parent == null || parent == this || parent == _parent || _children.Contains(parent)) return;
            if (_parent != null) RemoveParent();
            parent.AddChild(this);
            _parent = parent;
        }

        public void RemoveParent()
        {
            if (!_isActive) return;
            if (_parent == null) return;
            _parent.RemoveChild(this, false);
            _parent = null;
        }

        public void AddChild(Entity child)
        {
            if (!_isActive || _children == null) return;
            if (child == null || child == this || child == _parent || _children.Contains(child)) return;
            _children.Add(child);
            child.SetParent(this);
        }

        public bool HasChild(string name)
        {
            if (!_isActive || _children == null) return false;
            if (string.IsNullOrWhiteSpace(name)) return false;
            return _children.Exists(c => c.Name == name);
        }

        public Entity[] GetChildren()
        {
            if (!_isActive || _children == null) return null;
            return [.._children];
        }
        public Entity GetChild(string name)
        {
            if (!_isActive || _children == null) return null;
            return _children.Find(c => c.Name == name);
        }
        
        public bool TryGetChild(string name, out Entity child)
        {
            if (!_isActive || _children == null)
            {
                child = null;
                return false;
            }
            child = GetChild(name);
            return child != null;
        }

        public void RemoveChild(Entity child, bool shouldDispose = true)
        {
            if (!_isActive || _children == null) return;
            if (child == null || child == this || child == _parent || !_children.Contains(child)) return;
            _children.Remove(child);
            child.RemoveParent();
            if (shouldDispose) child.Dispose();
        }

        public void RemoveChildren(bool shouldDispose = true)
        {
            if (!_isActive || _children == null) return;
            for (int i = 0; i < _children.Count; i++)
            {
                if (i >= _children.Count) break;
                if (_children[i] == null) continue;
                _children[i].RemoveParent();
                if (i >= _children.Count) break;
                if (_children[i] == null) continue;
                if (shouldDispose) _children[i].Dispose();
            }
            _children.Clear();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            if (!_isActive || _components == null) return null;
            T component = new();
            if (_components.Exists(c => c is T)) return _components.Find(c => c is T) as T;
            component.Initialize(this);
            _components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            if (!_isActive || _components == null) return null;
            return _components.Find(c => c is T) as T;
        }
        
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            if (!_isActive || _components == null)
            {
                component = null;
                return false;
            }
            component = _components.Find(c => c is T) as T;
            return component != null;
        }

        public bool HasComponent<T>() where T : Component
        {
            if (!_isActive || _components == null) return false;
            return _components.Exists(c => c is T);
        }

        public void RemoveComponent<T>() where T : Component
        {
            if (!_isActive || _components == null) return;
            Component component = _components.Find(c => c is T);
            if (component == null) return;
            component.Dispose();
            _components.Remove(component);
        }

        public void RemoveComponents()
        {
            if (!_isActive || _components == null) return;
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Dispose();
            }
            _components.Clear();
        }
    }
}
