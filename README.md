# RelaxBot
---
Bot with intention to reduce stress in user but probably will make them more anxious due lack of respecting personal space :)
---
- To run you need generate own new token for bot, locate in program.cs token variable and paste into it.
- And then keep it running 
---
 Available commands: 
 !commands - gives available commands
 !status - gives status of bot variables 
 !planguage switch - switch ON&OFF P language translator for all messages 
 !suggestions switch - switch ON&OFF suggestions to random user 
 what to do today? - makes suggestion for activity 
 !quiz switch - switch quiz ON&OFF for random user 
 !breath - start breathing exercise 
 
 Examples:
 
> !status 
 
P language interpretator is OFF
Suggestions is OFF with frequency once in 30 minutes
Quiz is ON 

> !planguage switch

It will toggle p language tool on and off.
Rules of english version of P language is simple.
- all words or part of words which sounds like number are transformed to real numbers and then incermented by:
__mate lets play tennis__ will converted to __m9 lets play 11nis__

> !suggestions switch

Toggles suggestions on and off.
Suggestor time after time takes random user and gives idea what to do, like "taka a walk".

> what to do today?

This gives user suggestion to user what to do.

> !quiz switch

Toggles quiz mode on and off. 
Bot randomly choose user and asks it a question, and waits for correct answer. 
Till bot recieves correct answer user won't be able to participate in channel chat :)

> !breath

Initiates 2 minutes breating exercise with visual image to control breath.
