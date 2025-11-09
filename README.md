# 🕵️ Spy vs Spy — Visibility Rules

Each spy is placed on a square grid (like a chessboard).

A spy can **see** another spy if they share a clear line of sight:

- **Same row** → horizontally aligned  
- **Same column** → vertically aligned  
- **Same diagonal** → equal row and column distance

Mathematically:

> Two spies can see each other if  
> `|row₁ − row₂| == |col₁ − col₂|`  
> or they share the same row or column.
