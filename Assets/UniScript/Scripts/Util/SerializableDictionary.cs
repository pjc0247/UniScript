using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniScript.Serialization
{
    [Serializable]
    public class SValue
    {
        public SValueType type;

        public float f;
        public double d;
        public int i;
        public bool b;

        public Vector2 vec2;
        public Vector3 vec3;
        public Vector4 vec4;
        public Color col;
        public Quaternion q;

        public UnityEngine.Object unityObject;

        public SValue(SValueType type)
        {
            this.type = type;
        }
    }
    public enum SValueType
    {
        Null,
        Float, Integer, Bool, Double,

        Vec2, Vec3, Vec4,
        Color,
        Quaternion,

        UnityObject
    }
    public static class SvalueExt
    {
        public static SValue ToSValue(this object _obj)
        {
            if (_obj == null)
                return new SValue(SValueType.Null);

            if (_obj is UnityEngine.Object uobj)
                return new SValue(SValueType.UnityObject) { unityObject = uobj };
            if (_obj is Vector2 vec2)
                return new SValue(SValueType.Vec2) { vec2 = vec2 };
            if (_obj is Vector3 vec3)
                return new SValue(SValueType.Vec3) { vec3 = vec3 };
            if (_obj is Vector4 vec4)
                return new SValue(SValueType.Vec4) { vec4 = vec4 };
            if (_obj is Color col)
                return new SValue(SValueType.Color) { col = col };
            if (_obj is Quaternion q)
                return new SValue(SValueType.Quaternion) { q = q };

            if (_obj is float f)
                return new SValue(SValueType.Float) { f = f };
            if (_obj is int i)
                return new SValue(SValueType.Integer) { i = i };
            if (_obj is bool b)
                return new SValue(SValueType.Bool) { b = b };
            if (_obj is double d)
                return new SValue(SValueType.Double) { d = d };

            return null;
        }
        public static object ToObject(this SValue _obj)
        {
            if (_obj == null) return null;

            if (_obj.type == SValueType.UnityObject)
                return _obj.unityObject;
            if (_obj.type == SValueType.Vec2)
                return _obj.vec2;
            if (_obj.type == SValueType.Vec3)
                return _obj.vec3;
            if (_obj.type == SValueType.Vec4)
                return _obj.vec4;
            if (_obj.type == SValueType.Color)
                return _obj.col;
            if (_obj.type == SValueType.Quaternion)
                return _obj.q;

            if (_obj.type == SValueType.Float)
                return _obj.f;
            if (_obj.type == SValueType.Integer)
                return _obj.i;
            if (_obj.type == SValueType.Bool)
                return _obj.b;
            if (_obj.type == SValueType.Double)
                return _obj.d;

            return null;
        }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<SValue> values = new List<SValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value.ToSValue());
            }
        }
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new Exception($"there are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], (TValue)values[i].ToObject());
        }
    }
    [Serializable]
    public class SerializableDictionarySO : SerializableDictionary<string, object> { }
}