# EditorCoroutines

Provides a method to start your coroutines in UnityEditor.

## Getting started 

1. Append a line in Packages/manifest.json

```
"com.tani-shi.unity-editor-coroutines": "https://github.com/tani-shi/unity-editor-coroutines.git#1.0.0"
```

2. Implement EditorCoroutines in your scripts.

```
var coroutine = YourCoroutine();

// Start your coroutine
UnityEditor.Coroutines.EditorCoroutines.StartCoroutine(coroutine);

// Stop your coroutine
UnityEditor.Coroutines.EditorCoroutines.StopCoroutine(coroutine);
```

## License

MIT
