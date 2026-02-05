# AI_NOTES.md

## 1. AI Tools Used
I used the following AI tools during development:

- Microsoft Copilot (primary assistant for debugging and architecture guidance)

---

## 2. Prompts That Helped Me the Most

### Prompt 1
“Help me design a backend API endpoint that returns aggregated weather data from stored JSON files.”

### Prompt 2
“Why is my Blazor Server component not responding to button clicks?”

### Prompt 3
“Explain why my HttpClient in Blazor Server gives ‘invalid request URI’ and how to fix it.”

---

## 3. Example Where AI Was Wrong or Not Ideal

One AI suggestion told me to fix my sorting issue by adding `StateHasChanged()` inside my sorting methods.  
This didn’t solve the problem because the real issue was that my Razor Component wasn’t interactive.

I detected the problem because:

- The console logs showed no click events firing.
- The UI never re-rendered even when I changed the data.
- Sorting logic was correct, but nothing happened visually.

I corrected it by adding:

```razor
@rendermode InteractiveServer
