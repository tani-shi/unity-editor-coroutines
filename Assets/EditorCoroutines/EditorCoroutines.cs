// Copyright (c) 2020 Shintaro Tanikawa
//
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Coroutines
{
    public static class EditorCoroutines
    {
        static readonly Dictionary<int, Stack<IEnumerator>> _coroutineMap = new Dictionary<int, Stack<IEnumerator>> ();

        static EditorCoroutines ()
        {
            EditorApplication.update += Update;
        }

        public static IEnumerator StartCoroutine (IEnumerator routine)
        {
            if (routine.MoveNext ())
            {
                var hash = routine.GetHashCode ();
                _coroutineMap.Add (hash, new Stack<IEnumerator> ());
                _coroutineMap[hash].Push (routine);

                if (routine.Current is IEnumerator)
                {
                    foreach (var r in ExecuteRecursively (routine.Current as IEnumerator))
                    {
                        _coroutineMap[hash].Push (r);
                    }
                }
                else
                {
                    _coroutineMap[hash].Push (routine);
                }
            }
            return routine;
        }

        public static void StopCoroutine (IEnumerator routine)
        {
            var hash = routine.GetHashCode ();
            if (_coroutineMap.ContainsKey (hash))
            {
                _coroutineMap.Remove (hash);
            }
        }

        public static void StopAllCoroutines ()
        {
            _coroutineMap.Clear ();
        }

        static void Update ()
        {
            for (int i = 0; i < _coroutineMap.Count; i++)
            {
                var kv = _coroutineMap.ElementAt (i);
                while (kv.Value.Count > 0)
                {
                    var routine = kv.Value.Peek ();
                    if (routine.MoveNext ())
                    {
                        if (routine.Current is IEnumerator)
                        {
                            foreach (var r in ExecuteRecursively (routine.Current as IEnumerator))
                            {
                                kv.Value.Push (r);
                            }
                        }
                        break;
                    }
                    else
                    {
                        routine = kv.Value.Pop ();
                    }
                }
            }
        }

        static IEnumerable<IEnumerator> ExecuteRecursively (IEnumerator routine)
        {
            while (routine.MoveNext ())
            {
                yield return routine;

                if (routine.Current is IEnumerator)
                {
                    routine = routine.Current as IEnumerator;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}

#endif