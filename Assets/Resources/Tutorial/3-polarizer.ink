INCLUDE global.ink

Hi, bro!
+ [I know you, you are a lens]
    I am not a lens, man. I am a polarizer.
+ [Who are you?]
    I am a polarizer, man. 
- In general, I could filt polarization light. #title:Polarizer #speaker:Polarizer
What do you want, man?
->choices

=== choices ===
+ [Tell me more about yourself, bro!]->polarizer
+ {isPolarizerTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== polarizer ===
To be continued... #title:Polarizer Knowledge
~ isPolarizerTutFinish = true
* [CONTINUE]->repeat

=== quiz ===
To be continued... #title:Polarizer Quiz
* [CONTINUE]->repeat

=== repeat ===
Do you have any other things want to know? #title:Polarizer 
->choices

=== end ===
You could invoke me again by clicking any polarizers. May you have a good learning experience!
* [LEAVE]->DONE